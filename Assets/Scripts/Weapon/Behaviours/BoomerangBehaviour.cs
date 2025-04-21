using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehaviour : ProjectileWeaponBehaviour
{
    private Vector3 startDirection;
    private Vector3 startPosition;
    private bool returning = false;

    private float moveDistance = 5f; // How far it travels before returning

    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

    public void Initialize(Vector3 direction)
    {
        startDirection = direction.normalized;
        startPosition = transform.position;
        DirectionChecker(startDirection);
    }

    protected override void Start()
    {
        destroyAfterSeconds = Mathf.Infinity;
        base.Start();
    }

    void Update()
    {
        if (!returning)
        {
            transform.position += startDirection * currentMoveSpeed * Time.deltaTime;

            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                returning = true;
            }
        }
        else
        {
            Vector3 returnDir = (FindObjectOfType<PlayerMovement>().transform.position - transform.position).normalized;
            transform.position += returnDir * currentMoveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, FindObjectOfType<PlayerMovement>().transform.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }

        // Optional spin
        transform.Rotate(0f, 0f, 720f * Time.deltaTime); // Fast spin
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !damagedEnemies.Contains(collision.gameObject))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);
            damagedEnemies.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Prop") && !damagedEnemies.Contains(collision.gameObject))
        {
            if (collision.TryGetComponent(out BreakabkeProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                damagedEnemies.Add(collision.gameObject);
            }
        }
    }
}


