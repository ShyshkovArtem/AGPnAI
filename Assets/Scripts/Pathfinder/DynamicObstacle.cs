using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DynamicObstacle : MonoBehaviour
{
    private GridManager grid;
    private Vector2 lastPosition;
    private const float ThresholdSqr = 0.0001f;

    void Awake()
    {
        grid = FindObjectOfType<GridManager>();
        lastPosition = transform.position;
    }

    void OnEnable()
    {
        // initial notify
        grid.NotifyGridChanged();
    }

    void OnDisable()
    {
        grid.NotifyGridChanged();
    }

    void LateUpdate()
    {
        Vector2 current = transform.position;
        if ((current - lastPosition).sqrMagnitude > ThresholdSqr)
        {
            // force a grid-changed event on any movement
            grid.NotifyGridChanged();
            lastPosition = current;
        }
    }
}
