using System.Collections;
using UnityEngine;

/// <summary>
/// Moves this GameObject to a random nearby point every few seconds.
/// Combine with your DynamicObstacle component to have the grid auto-update.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class RandomObstacleMover : MonoBehaviour
{
    [Tooltip("Max distance (in world units) from current position to move each interval")]
    public float moveRadius = 0.5f;
    [Tooltip("How long to take (in seconds) to move to the new point")]
    public float moveDuration = 0.5f;
    [Tooltip("Delay (in seconds) between movements")]
    public float moveInterval = 1.0f;

    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        var collider = GetComponent<Collider2D>();

        while (true)
        {
            // pick a random target within moveRadius of the starting point
            Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
            Vector2 targetPos = startPosition + randomOffset;
            Vector2 origin = transform.position;

            // move smoothly over moveDuration
            float elapsed = 0f;
            while (elapsed < moveDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / moveDuration);
                transform.position = Vector2.Lerp(origin, targetPos, t);
                yield return null;
            }

            // wait before picking next point
            yield return new WaitForSeconds(moveInterval);
        }
    }
}
