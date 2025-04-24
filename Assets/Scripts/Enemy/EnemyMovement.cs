using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats enemy;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 knockbackVelocity;
    private float knockbackDuration;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (knockbackDuration > 0)
        {
            rb.velocity = knockbackVelocity;
            knockbackDuration -= Time.fixedDeltaTime;

            // Optional: Stop knockback early when time runs out
            if (knockbackDuration <= 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.velocity = dir * enemy.currentMoveSpeed;
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);
        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackDuration = duration;
        knockbackVelocity = velocity;
    }
}
