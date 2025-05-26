using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentMover))]
public class PathGizmos : MonoBehaviour
{
    AgentMover mover;

    void OnEnable()
    {
        mover = GetComponent<AgentMover>();
    }

    void OnDrawGizmos()
    {
        if (mover == null || mover.Path == null) return;

        List<Vector2> path = mover.Path;
        Gizmos.color = Color.cyan;
        for (int i = mover.CurrentIndex; i < path.Count; i++)
        {
            Gizmos.DrawSphere(path[i], 0.1f);
            if (i < path.Count - 1)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
