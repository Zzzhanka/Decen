using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class AstarPathfinding
{
    public static List<Vector3> FindPath(Vector3 start, Vector3 target, Tilemap walkableTilemap)
    {
        Vector3Int startCell = walkableTilemap.WorldToCell(start);
        Vector3Int targetCell = walkableTilemap.WorldToCell(target);

        // Открытый список для узлов, которые нужно проверить
        List<Vector3Int> openList = new List<Vector3Int> { startCell };
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();

        // Карты для отслеживания родителя узла и стоимости
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> cost = new Dictionary<Vector3Int, float>
        {
            { startCell, 0 }
        };

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => (int)(cost[a] - cost[b])); // Сортируем по наименьшей стоимости
            Vector3Int current = openList[0];

            if (current == targetCell) break; // Если достигли цели

            openList.Remove(current);
            closedList.Add(current);

            // Проверяем соседние клетки (вверх, вниз, влево, вправо)
            foreach (var neighbor in GetNeighbors(current, walkableTilemap))
            {
                if (closedList.Contains(neighbor)) continue;

                float newCost = cost[current] + Vector3Int.Distance(current, neighbor);

                if (!openList.Contains(neighbor) || newCost < cost[neighbor])
                {
                    cost[neighbor] = newCost;
                    cameFrom[neighbor] = current;
                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }

        // Восстанавливаем путь
        List<Vector3> path = new List<Vector3>();
        Vector3Int currentStep = targetCell;

        while (currentStep != startCell)
        {
            path.Add(walkableTilemap.CellToWorld(currentStep));
            currentStep = cameFrom[currentStep];
        }

        path.Reverse();
        return path;
    }

    private static List<Vector3Int> GetNeighbors(Vector3Int cell, Tilemap tilemap)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        foreach (var direction in directions)
        {
            Vector3Int neighbor = cell + direction;
            if (tilemap.HasTile(neighbor)) // Проверяем, есть ли тайл
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
}
