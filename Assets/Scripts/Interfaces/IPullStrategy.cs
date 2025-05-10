using UnityEngine;

// Strategy for applying pull force toward the player
public interface IPullStrategy
{
    void Pull(Rigidbody2D targetRb, Vector2 sourcePosition);
}