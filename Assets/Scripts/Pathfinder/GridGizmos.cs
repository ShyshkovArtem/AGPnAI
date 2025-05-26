using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(GridManager))]
public class GridGizmos : MonoBehaviour
{
    GridManager grid;
    float nodeDiameter;
    Vector2 worldBottomLeft;

    void OnEnable()
    {
        grid = GetComponent<GridManager>();
        // cache parameters so we don’t recalc every Gizmo draw
        nodeDiameter = grid.nodeRadius * 2;
        worldBottomLeft = (Vector2)transform.position
                        - Vector2.right * grid.gridWorldSize.x / 2
                        - Vector2.up * grid.gridWorldSize.y / 2;
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;
        // iterate the internal grid array by reflecting private fields:
        var field = typeof(GridManager)
                    .GetField("grid",
                              System.Reflection.BindingFlags.NonPublic |
                              System.Reflection.BindingFlags.Instance);
        var nodes2D = field.GetValue(grid) as System.Array;
        if (nodes2D == null) return;

        int sizeX = nodes2D.GetLength(0);
        int sizeY = nodes2D.GetLength(1);

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                var node = nodes2D.GetValue(x, y) as Node;
                Gizmos.color = node.walkable
                    ? new Color(0, 1, 0, .15f)   // green = free
                    : new Color(1, 0, 0, .3f);   // red   = blocked
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .05f));
            }
    }
}
