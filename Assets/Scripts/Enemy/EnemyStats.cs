using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //Current stats
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector]
    public float currentAttackCooldown;

    [Header("Attack Settings")]
    private float lastAttackTime = Mathf.NegativeInfinity;
    public bool isAttacking;

    public float despawnDistance = 20f;
    private Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);   //Color of the damage flash
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.5f; //Time it takes for the enemy to fade
    private Color originalColor;
    private SpriteRenderer spriteRender;
    private EnemyMovement enemyMovement;

    Animator animator;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currentAttackCooldown = enemyData.AttackCooldown;
    }


    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;

        spriteRender = GetComponent<SpriteRenderer>();
        originalColor = spriteRender.color;

        enemyMovement = GetComponent<EnemyMovement>();

        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }


    public virtual void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockDuration = 0.2f)
    {
        currentHealth -= dmg;

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        //StartCoroutine(DamageFlash());

        //Create text pop up when enemy being attacked
        if (dmg > 0) GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);


        if (knockbackForce > 0)     //Apply knockback if it is not zero
        {
            //Get the direction of the knockback
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            enemyMovement.Knockback(dir.normalized * knockbackForce, knockDuration);
        }


        if (currentHealth <= 0)
        {
            Kill();
        }
    }


    public void DealDamageToPlayer()
    {
        Debug.Log("Attempting to deal damage");

        if (player == null) return;

        // Only deal damage if player is still in range
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Player")); // Adjust radius
        if (hit != null)
        {
            Debug.Log("Player hit!");
            PlayerStats playerStats = hit.GetComponent<PlayerStats>();
            playerStats?.TakeDamage(currentDamage);
        }

        lastAttackTime = Time.time;
    }


    IEnumerator DamageFlash()   //Makes the enemy flash when taking damage
    {
        spriteRender.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRender.color = originalColor;
    }

    public static event Action OnAnyEnemyKilled;

    public void Kill()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death"); 
        }

        OnAnyEnemyKilled?.Invoke();

        // Stop movement
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // Fade out + destroy after animation ends
        StartCoroutine(KillAfterAnimation());
    }


    IEnumerator KillAfterAnimation()
    {
        yield return new WaitForSeconds(0.8f); // Duration of death animation
        StartCoroutine(KillFade());
    }


    IEnumerator KillFade()      //Fades the enemy away frame by frame
    {
        //Waits for a frame 
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = spriteRender.color.a;

        //Loop that fires every frame
        while (t  < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            //Set the color for this frame
            spriteRender.color = new Color (spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        //Ref the script from the collided collider and use TakeDamage()
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + currentAttackCooldown)
            {
                //PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
                //player.TakeDamage(currentDamage);

                lastAttackTime = Time.time;
                isAttacking = true;

                // Trigger attack animation
                GetComponent<EnemyAnimation>().PlayAttack();
            }
        }
    }


    private void LateUpdate()
    {
        isAttacking = false; // Reset each frame unless re-triggered
    }


    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
        {
            es.OnEnemyKill();
        }
        else
        {
            Debug.LogWarning("EnemySpawner not found in the scene.");
        }
    }



    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[UnityEngine.Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }



}
