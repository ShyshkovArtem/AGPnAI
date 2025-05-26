using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes A* pathfinding on a GridManager and smooths the resulting path.
/// </summary>
public class Pathfinder : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] public LayerMask unwalkableMask;

    private void Awake()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
    }

    /// <summary>
    /// Finds a path between two world positions and returns a smoothed list of waypoints.
    /// </summary>
    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = gridManager.NodeFromWorldPoint(startPos);
        Node targetNode = gridManager.NodeFromWorldPoint(targetPos);

        var openSet = new Heap<Node>(gridManager.MaxSize);
        var closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                var rawPath = RetracePath(startNode, targetNode);
                return SmoothPath(rawPath);
            }

            foreach (var neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else
                        openSet.UpdateItem(neighbor);
                }
            }
        }

        // No path found
        return new List<Vector2>();
    }

    /// <summary>
    /// Retraces the parent links from end to start to produce the raw path.
    /// </summary>
    private List<Vector2> RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Vector2>();
        Node current = endNode;
        while (current != startNode)
        {
            path.Add(current.worldPosition);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Smooths a raw waypoint list by skipping unnecessary intermediate points.
    /// </summary>
    private List<Vector2> SmoothPath(List<Vector2> path)
    {
        if (path.Count < 3)
            return path;

        var smooth = new List<Vector2> { path[0] };
        int lastIndex = 0;

        for (int i = 2; i < path.Count; i++)
        {
            Vector2 from = path[lastIndex];
            Vector2 to = path[i];

            // If linecast hits an obstacle, can't skip the previous point
            if (Physics2D.Linecast(from, to, unwalkableMask))
            {
                smooth.Add(path[i - 1]);
                lastIndex = i - 1;
            }
        }

        smooth.Add(path[path.Count - 1]);
        return smooth;
    }

    /// <summary>
    /// Heuristic cost: diagonal = 14, straight = 10.
    /// </summary>
    private int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        if (dx > dy)
            return 14 * dy + 10 * (dx - dy);
        return 14 * dx + 10 * (dy - dx);
    }
}
