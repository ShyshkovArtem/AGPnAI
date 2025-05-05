using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;


    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
