using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinder))]
public class AgentMover : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 3f;

    private Pathfinder pathfinder;
    private GridManager gridManager;
    private List<Vector2> path;
    private int targetIndex;

    // Expose these so your PathGizmos can draw them:
    public List<Vector2> Path => path;
    public int CurrentIndex => targetIndex;

    void Start()
    {
        // Cache references
        pathfinder = GetComponent<Pathfinder>();
        gridManager = FindObjectOfType<GridManager>();

        // Subscribe to grid-change events
        gridManager.OnGridChanged += OnGridChanged;

        // First path
        RecalculatePath();
    }

    void OnDestroy()
    {
        if (gridManager != null)
            gridManager.OnGridChanged -= OnGridChanged;
    }

    void Update()
    {
        if (path == null || targetIndex >= path.Count) return;

        Vector2 nextPoint = path[targetIndex];

        // Optional extra check: if somehow the next segment is now blocked, re-route immediately
        if (Physics2D.Linecast(transform.position, nextPoint, pathfinder.unwalkableMask))
        {
            RecalculatePath();
            return;
        }

        // Move along current path
        transform.position = Vector2.MoveTowards(
            transform.position,
            nextPoint,
            speed * Time.deltaTime
        );

        if ((Vector2)transform.position == nextPoint)
            targetIndex++;
    }

    private void OnGridChanged()
    {
        // Anytime the grid updates, just recalc
        RecalculatePath();
    }

    private void RecalculatePath()
    {
        path = pathfinder.FindPath(transform.position, target.position);
        targetIndex = 0;
    }
}
