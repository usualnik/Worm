using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileConnectivityChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _objectTileMap;
    [SerializeField] private TileBase _objectTile;

    private Vector3Int[] _directions = new Vector3Int[]
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0)
    };

    public List<HashSet<Vector3Int>> FindConnectedComponents()
    {
        var allComponents = new List<HashSet<Vector3Int>>();
        var visited = new HashSet<Vector3Int>();

        var allTiles = GetAllOccupiedTiles();
        Debug.Log($"Всего тайлов: {allTiles.Count}");

        foreach (var tilePos in allTiles)
        {
            if (!visited.Contains(tilePos))
            {
                var component = FloodFill(tilePos, visited);
                allComponents.Add(component);
                Debug.Log($"Найден компонент из {component.Count} тайлов");
            }
        }

        return allComponents;
    }

    private HashSet<Vector3Int> GetAllOccupiedTiles()
    {
        var occupiedTiles = new HashSet<Vector3Int>();

        foreach (var pos in _objectTileMap.cellBounds.allPositionsWithin)
        {
            if (_objectTileMap.GetTile(pos) == _objectTile)
            {
                occupiedTiles.Add(pos);
            }
        }

        return occupiedTiles;
    }

    private HashSet<Vector3Int> FloodFill(Vector3Int startPos, HashSet<Vector3Int> visited)
    {
        var component = new HashSet<Vector3Int>();
        var queue = new Queue<Vector3Int>();

        queue.Enqueue(startPos);
        visited.Add(startPos);
        component.Add(startPos);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var direction in _directions)
            {
                var neighbor = current + direction;

                if (!visited.Contains(neighbor) &&
                    _objectTileMap.GetTile(neighbor) == _objectTile)
                {
                    visited.Add(neighbor);
                    component.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return component;
    }

    public bool IsGrounded(HashSet<Vector3Int> component)
    {
        foreach (var tilePos in component)
        {
            // Считаем тайл заземленным если он в самом низу или под ним нет тайла
            if (tilePos.y <= -4) // Нижняя граница уровня
                return true;

            var below = tilePos + new Vector3Int(0, -1, 0);
            if (_objectTileMap.GetTile(below) == null && tilePos.y > -4)
                continue;
            else
                return true;
        }
        return false;
    }

    // Метод для отладки - визуализирует компоненты разными цветами
    public void DebugVisualizeComponents(List<HashSet<Vector3Int>> components)
    {
        for (int i = 0; i < components.Count; i++)
        {
            var color = Color.HSVToRGB(i / (float)components.Count, 1f, 1f);
            foreach (var tilePos in components[i])
            {
                Debug.DrawLine(
                    _objectTileMap.CellToWorld(tilePos),
                    _objectTileMap.CellToWorld(tilePos) + Vector3.one,
                    color,
                    3f
                );
            }
        }
    }
}