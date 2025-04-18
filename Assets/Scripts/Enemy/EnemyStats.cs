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
    float currentHealth;
    [HideInInspector]
    float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);   //Color of the damage flash
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.5f; //Time it takes for the enemy to fade
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }


    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        movement = GetComponent<EnemyMovement>();
    }


    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }


    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockDuration = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        //Create text pop up when enemy being attacked
        if (dmg > 0) GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);


        if (knockbackForce > 0)     //Apply knockback if it is not zero
        {
            //Get the direction of the knockback
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockDuration);
        }


        if (currentHealth <= 0)
        {
            Kill();
        }
    }


    IEnumerator DamageFlash()   //Makes the enemy flash when taking damage
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }


    public void Kill()
    {
        StartCoroutine(KillFade());
    }


    IEnumerator KillFade()      //Fades the enemy away frame by frame
    {
        //Waits for a frame 
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        //Loop that fires every frame
        while (t  < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            //Set the color for this frame
            sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        //Ref the script from the collided collider and use TakeDamage()
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        es.OnEnemyKill();
    }


    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }



}
