using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMovement : MonoBehaviour
{
    public Transform workPoint;
    public Transform homePoint;
    public Tilemap tilemap;
    public GameTimeManager gameTimeManager; // Ссылка на GameTimeManager

    public float stopDistance = 1.5f;  // Дистанция остановки перед игроком
    public NPCDialog npcDialog;        // Ссылка на скрипт для диалогов

    private bool atWork = false;
    private bool isStopped = false;    // NPC остановился перед игроком
    private Coroutine moveCoroutine;

    private void Update()
    {
        int currentHour = gameTimeManager.GetCurrentHour();

        if (!isStopped)  // Останавливать логику движения, если NPC остановлен
        {
            if (currentHour == 8 && !atWork)
            {
                MoveTo(workPoint.position);
                atWork = true;
            }
            else if (currentHour == 17 && atWork)
            {
                MoveTo(homePoint.position);
                atWork = false;
            }
        }
    }

    private void MoveTo(Vector3 destination)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToTarget(destination));
    }

    private IEnumerator MoveToTarget(Vector3 destination)
    {
        Vector3Int[] path = FindPath(transform.position, destination);

        foreach (var tilePos in path)
        {
            Vector3 worldPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0);
            while (Vector3.Distance(transform.position, worldPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, worldPos, 2f * Time.deltaTime);

                if (PlayerInRange())
                {
                    StopAndWaitForDialog();  // Остановка, если игрок рядом
                    yield break;  // Прервать корутину движения
                }
                yield return null;
            }
        }

        yield return new WaitForSeconds(3f);  // Пауза на точке
    }

    private bool PlayerInRange()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= stopDistance;
    }

    private void StopAndWaitForDialog()
    {
        isStopped = true;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        StartCoroutine(WaitForDialog(5f));
    }

    private IEnumerator WaitForDialog(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (PlayerInRange())  // Проверяем, всё ли ещё рядом игрок
        {
            npcDialog.StartDialog();  // Запуск диалога
        }

        isStopped = false;  // NPC снова может двигаться
    }

    // Алгоритм A* для поиска пути
    private Vector3Int[] FindPath(Vector3 startWorld, Vector3 destinationWorld)
    {
        Vector3Int start = tilemap.WorldToCell(startWorld);
        Vector3Int destination = tilemap.WorldToCell(destinationWorld);

        List<Vector3Int> openList = new List<Vector3Int> { start };
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float> { [start] = Heuristic(start, destination) };

        while (openList.Count > 0)
        {
            Vector3Int current = GetLowestScoreCell(openList, fScore);
            if (current == destination) return ReconstructPath(cameFrom, current);

            openList.Remove(current);
            closedList.Add(current);

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor)) continue;

                float tentativeGScore = gScore[current] + 1;

                if (!openList.Contains(neighbor))
                    openList.Add(neighbor);
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = tentativeGScore + Heuristic(neighbor, destination);
            }
        }

        return new Vector3Int[0];  // Путь не найден
    }

    private Vector3Int GetLowestScoreCell(List<Vector3Int> openList, Dictionary<Vector3Int, float> fScore)
    {
        Vector3Int lowest = openList[0];
        float lowestScore = fScore.GetValueOrDefault(lowest, float.MaxValue);

        foreach (Vector3Int cell in openList)
        {
            float score = fScore.GetValueOrDefault(cell, float.MaxValue);
            if (score < lowestScore)
            {
                lowest = cell;
                lowestScore = score;
            }
        }

        return lowest;
    }

    private IEnumerable<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        yield return new Vector3Int(cell.x + 1, cell.y, cell.z);
        yield return new Vector3Int(cell.x - 1, cell.y, cell.z);
        yield return new Vector3Int(cell.x, cell.y + 1, cell.z);
        yield return new Vector3Int(cell.x, cell.y - 1, cell.z);
    }

    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);  // Манхэттенское расстояние
    }

    private Vector3Int[] ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        return totalPath.ToArray();
    }
}
