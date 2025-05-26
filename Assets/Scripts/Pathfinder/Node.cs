using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a single cell in the grid and implements IHeapItem for use in the binary heap.
/// </summary>
public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;

    /// <summary>
    /// Combined cost: gCost (from start) + hCost (heuristic to target).
    /// </summary>
    public int fCost => gCost + hCost;

    // Implementation of IHeapItem<Node>:
    /// <summary>Index of this item in the heap array.</summary>
    public int HeapIndex { get; set; }

    /// <summary>
    /// Compare nodes by fCost; if equal, tie-break by hCost.
    /// </summary>
    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(other.hCost);
        return compare;
    }

    public Node(bool walkable, Vector2 worldPos, int x, int y)
    {
        this.walkable = walkable;
        worldPosition = worldPos;
        gridX = x;
        gridY = y;
    }
}
