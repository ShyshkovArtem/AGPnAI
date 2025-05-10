using UnityEngine;

// Concrete strategy: constant-speed pull
public class ConstantPullStrategy : IPullStrategy
{
    private readonly float _speed;
    public ConstantPullStrategy(float speed) => _speed = speed;
    public void Pull(Rigidbody2D targetRb, Vector2 sourcePosition)
    {
        if (targetRb == null) return;
        Vector2 direction = (sourcePosition - targetRb.position).normalized;
        // Use AddForce to apply a pulling force towards player
        targetRb.AddForce(direction * _speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
