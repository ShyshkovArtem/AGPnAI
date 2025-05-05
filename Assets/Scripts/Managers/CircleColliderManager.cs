using UnityEngine;

public class CircleColliderManager : IColliderManager
{
    private CircleCollider2D playerCollider;
    private PlayerAttributes playerAttributes;

    public CircleColliderManager(CircleCollider2D collider, PlayerAttributes attributes)
    {
        playerCollider = collider;
        playerAttributes = attributes;
    }

    public void UpdateColliderRadius()
    {
        playerCollider.radius = playerAttributes.CurrentMagnet;
    }
}

