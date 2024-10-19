using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class NPCMovement : MonoBehaviour
{
    public Transform workPoint;  // Точка работы
    public Transform homePoint;  // Точка дома
    public Transform lunchPoint; // Точка обеда (например, кафе)
    public Tilemap tilemap;
    public GameTimeManager gameTimeManager;  // Ссылка на GameTimeManager
    public float killDistance = 1.5f;        // Расстояние для убийства NPC
    public GameObject witnessPrefab;         // Префаб свидетеля
    public GameObject killButton;            // Кнопка для убийства
    public TextMeshProUGUI questText;        // Текст задания для отображения

    private bool atWork = false;
    private bool atLunch = false;
    private bool isDead = false;

    private void Start()
    {
        killButton.SetActive(false);
    }
    private void Update()
    {
        int currentHour = gameTimeManager.GetCurrentHour();

        // Начало рабочего дня в 8:00
        if (currentHour == 8 && !atWork && !atLunch)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(workPoint.position));
            atWork = true;
        }

        // Обед в 12:00
        if (currentHour == 12 && atWork && !atLunch)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(lunchPoint.position));
            atLunch = true;
        }

        // Возвращение на работу после обеда в 13:00
        if (currentHour == 13 && atLunch)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(workPoint.position));
            atLunch = false;
        }

        // Возвращение домой в 17:00
        if (currentHour == 17 && atWork)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(homePoint.position));
            atWork = false;
        }

        // Проверка на смерть NPC
        if (isDead)
        {
            HandleWitnesses();
        }
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
                yield return null;
            }
        }

        yield return new WaitForSeconds(3f);  // Пауза на точке
    }

    private void HandleWitnesses()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1000f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("NPC") && collider.gameObject != this.gameObject)
            {
                Instantiate(witnessPrefab, collider.transform.position, Quaternion.identity);
            }
        }
    }

    public void KillNPC()
    {
        if (!isDead && Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= killDistance)
        {
            isDead = true;
            killButton.SetActive(false);
            UpdateQuestText("NPC убит");
            Destroy(gameObject);
        }
    }

    private void UpdateQuestText(string questName)
    {
        if (questText != null)
        {
            questText.text = questName;
        }
    }

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

        return new Vector3Int[0];
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
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
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
