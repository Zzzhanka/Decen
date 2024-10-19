using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMovement : MonoBehaviour
{
    public Transform workPoint;  // Точка работы
    public Transform homePoint;  // Точка дома
    public Tilemap tilemap;      // Тайлмап для маршрута

    private Vector3Int[] path;   // Маршрут NPC
    private int currentTarget = 0;
    private bool atDestination = false;

    private void Start()
    {
        StartCoroutine(MoveToTarget(workPoint.position));  // Сначала идём на работу
    }

    private IEnumerator MoveToTarget(Vector3 destination)
    {
        path = FindPath(transform.position, destination);

        foreach (var tilePos in path)
        {
            Vector3 worldPos = tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0);  // Центр тайла
            while (Vector3.Distance(transform.position, worldPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, worldPos, 2f * Time.deltaTime);
                yield return null;
            }
        }

        yield return new WaitForSeconds(3f);  // Постоять 3 секунды

        // Переход на следующий пункт назначения
        if (!atDestination)
        {
            atDestination = true;
            StartCoroutine(MoveToTarget(homePoint.position));  // Идём домой
        }
    }

    private Vector3Int[] FindPath(Vector3 start, Vector3 target)
    {
        Vector3Int startCell = tilemap.WorldToCell(start);
        Vector3Int targetCell = tilemap.WorldToCell(target);

        List<Vector3Int> openList = new List<Vector3Int>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
        gScore[startCell] = 0;

        openList.Add(startCell);

        while (openList.Count > 0)
        {
            Vector3Int current = GetLowestScoreCell(openList, gScore);
            if (current == targetCell)
                return ReconstructPath(cameFrom, current);

            openList.Remove(current);
            closedList.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + Vector3.Distance(current, neighbor);

                if (!openList.Contains(neighbor))
                    openList.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
            }
        }
        return new Vector3Int[0];  // Если путь не найден
    }

    private Vector3Int GetLowestScoreCell(List<Vector3Int> openList, Dictionary<Vector3Int, float> gScore)
    {
        Vector3Int lowest = openList[0];
        float lowestScore = gScore.ContainsKey(lowest) ? gScore[lowest] : Mathf.Infinity;

        foreach (var cell in openList)
        {
            float score = gScore.ContainsKey(cell) ? gScore[cell] : Mathf.Infinity;
            if (score < lowestScore)
            {
                lowest = cell;
                lowestScore = score;
            }
        }
        return lowest;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            new Vector3Int(cell.x + 1, cell.y, cell.z),
            new Vector3Int(cell.x - 1, cell.y, cell.z),
            new Vector3Int(cell.x, cell.y + 1, cell.z),
            new Vector3Int(cell.x, cell.y - 1, cell.z)
        };

        // Фильтруем только проходимые тайлы
        neighbors.RemoveAll(n => !tilemap.HasTile(n));
        return neighbors;
    }

    private Vector3Int[] ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath.ToArray();
    }
}
