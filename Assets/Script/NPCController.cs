using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCController : MonoBehaviour
{
    public Tilemap tilemap; // Тайлмап для поиска пути
    public Transform home;  // Точка дома NPC
    public Transform office; // Точка офиса
    public float moveSpeed = 2f; // Скорость передвижения

    private Vector3Int currentTarget; // Цель текущего движения
    private bool goingToWork = true; // Переключение между офисом и домом
    private bool isMoving = false; // Флаг передвижения
    private List<Vector3Int> path = new List<Vector3Int>(); // Путь из тайлов

    void Start()
    {
        SetTarget(office.position); // Устанавливаем цель — офис
        StartCoroutine(DailyRoutine()); // Запуск корутины с расписанием
    }

    void SetTarget(Vector3 targetPosition)
    {
        Vector3Int cellTarget = tilemap.WorldToCell(targetPosition);
        path = FindPath(transform.position, tilemap.CellToWorld(cellTarget));
    }

    IEnumerator DailyRoutine()
    {
        while (true)
        {
            // Если закончил движение, переключаемся на новую цель
            if (!isMoving)
            {
                if (goingToWork)
                {
                    SetTarget(office.position); // Идём на работу
                }
                else
                {
                    SetTarget(home.position); // Возвращаемся домой
                }

                StartCoroutine(MoveAlongPath()); // Запуск движения
                goingToWork = !goingToWork; // Переключаем цель
            }

            yield return new WaitForSeconds(10f); // Задержка между сменами (примерно день)
        }
    }

    List<Vector3Int> FindPath(Vector3 start, Vector3 end)
    {
        Vector3Int startCell = tilemap.WorldToCell(start);
        Vector3Int endCell = tilemap.WorldToCell(end);

        Queue<Vector3Int> openSet = new Queue<Vector3Int>();
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();

        openSet.Enqueue(startCell);
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            if (current == endCell)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                if (!openSet.Contains(neighbor))
                {
                    openSet.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }

            closedSet.Add(current);
        }

        return new List<Vector3Int>(); // Путь не найден
    }

    List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            cell + Vector3Int.up,
            cell + Vector3Int.down,
            cell + Vector3Int.left,
            cell + Vector3Int.right
        };

        neighbors.RemoveAll(n => !tilemap.HasTile(n));
        return neighbors;
    }

    List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true;

        foreach (var cell in path)
        {
            Vector3 target = tilemap.CellToWorld(cell) + new Vector3(0.5f, 0.5f, 0);
            while (Vector2.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
    }
}
