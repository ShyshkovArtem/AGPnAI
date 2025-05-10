using UnityEngine;

public class CircleColliderManager : IColliderManager
{
    private CircleCollider2D _playerCollider;
    private PlayerAttributes _playerAttributes;

    public CircleColliderManager(CircleCollider2D collider, PlayerAttributes attributes)
    {
        _playerCollider = collider;
        _playerAttributes = attributes;
    }

    public void UpdateColliderRadius()
    {
        if (_playerCollider == null)
        {
            Debug.LogWarning("CircleCollider2D reference is null.");
            return;
        }
        if (_playerAttributes == null)
        {
            Debug.LogWarning("PlayerAttributes reference is null.");
            return;
        }
        _playerCollider.radius = _playerAttributes.CurrentMagnet;
    }
}

