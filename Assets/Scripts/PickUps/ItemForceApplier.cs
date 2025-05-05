using UnityEngine;

public class ItemForceApplier
{
    private Rigidbody2D rb;
    private float pullSpeed;

    public ItemForceApplier(Rigidbody2D rb, float pullSpeed)
    {
        this.rb = rb;
        this.pullSpeed = pullSpeed;
    }

    public void ApplyForce(Vector2 attractorPosition)
    {
        Vector2 forceDirection = (attractorPosition - (Vector2)rb.transform.position).normalized;
        rb.AddForce(forceDirection * pullSpeed);
    }
}

