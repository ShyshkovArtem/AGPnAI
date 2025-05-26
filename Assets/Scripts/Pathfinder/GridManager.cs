using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Manages a 2D grid of nodes for pathfinding and provides helper methods.
/// Supports runtime updates and notifies listeners when the grid changes.
/// </summary>
public class GridManager : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    // Event fired whenever a node's walkability is updated
    public event Action OnGridChanged;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        Debug.Log($"[GridManager] Awake – creating {gridSizeX}×{gridSizeY} grid");
        CreateGrid();
    }

    /// <summary>
    /// Total number of nodes (for heap sizing).
    /// </summary>
    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position
                                 - Vector2.right * gridWorldSize.x / 2
                                 - Vector2.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft
                                     + Vector2.right * (x * nodeDiameter + nodeRadius)
                                     + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    /// <summary>
    /// Recomputes the node at the given world position and raises OnGridChanged if its walkability toggles.
    /// </summary>
    public void UpdateNodeAtPosition(Vector2 worldPos)
    {
        Node n = NodeFromWorldPoint(worldPos);
        bool nowWalkable = !Physics2D.OverlapCircle(n.worldPosition, nodeRadius, unwalkableMask);
        if (n.walkable != nowWalkable)
        {
            n.walkable = nowWalkable;
            OnGridChanged?.Invoke();
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        var neighbors = new List<Node>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int checkX = node.gridX + dx;
                int checkY = node.gridY + dy;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbors.Add(grid[checkX, checkY]);
            }
        }
        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector2 worldPos)
    {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    /// <summary>
    /// Allows external callers to force the grid-changed event.
    /// </summary>
    public void NotifyGridChanged()
    {
        OnGridChanged?.Invoke();
    }

}