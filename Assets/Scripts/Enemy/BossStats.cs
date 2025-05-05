using System.Collections;
using UnityEngine;

public class BossStats : EnemyStats
{
    [Header("Boss Features")]
    public float blockRange = 5f;               // If boss is farther than this, it blocks
    public float postAttackBlockDuration = 2f;    // Time after attack to auto block
    public float damageReductionFactor = 0.4f;    // 60% blocked ? 40% taken

    private bool isBlocking = false;
    private float blockTimer = 0f;

    public float rageThreshold = 0.3f;
    public float rageSpeedMultiplier = 1.5f;
    public float rageDamageMultiplier = 1.5f;
    public bool isInRage = false;

    private Transform player;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        // Check distance-based block
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > blockRange)
        {
            StartBlocking();
        }

        // Update block timer
        if (isBlocking)
        {
            blockTimer -= Time.deltaTime;
            if (blockTimer <= 0f)
            {
                StopBlocking();
            }
        }
    }

    public override void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockDuration = 0.2f)
    {
        float finalDamage = dmg;

        if (isBlocking)
        {
            finalDamage *= damageReductionFactor;  // Apply 60% reduction
            Debug.Log("Boss blocked! Reduced damage to: " + finalDamage);
            animator?.SetTrigger("Shield");
        }

        base.TakeDamage(finalDamage, sourcePosition, knockbackForce, knockDuration);

        if (!isInRage && currentHealth / enemyData.MaxHealth <= rageThreshold)
        {
            EnterRageMode();
        }
    }

    private void StartBlocking()
    {
        if (!isBlocking)
        {
            isBlocking = true;
            blockTimer = 0.5f;
            animator?.SetBool("IsBlocking", true); // Start looped animation
        }
    }

    public void TriggerPostAttackBlock()
    {
        isBlocking = true;
        blockTimer = postAttackBlockDuration;
        animator?.SetBool("IsBlocking", true);
    }

    private void StopBlocking()
    {
        isBlocking = false;
        animator?.SetBool("IsBlocking", false); // Exit looped animation
    }

    private void EnterRageMode()
    {
        isInRage = true;
        Debug.Log("Boss entered RAGE MODE!");
        currentMoveSpeed *= rageSpeedMultiplier;
        currentDamage *= rageDamageMultiplier;
        animator?.SetTrigger("Rage");
    }
}


