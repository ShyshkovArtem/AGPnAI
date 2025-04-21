using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOfWhispersBehaviour : MeleeBehaviour
{
    private float rotationSpeed;
    private float currentAngle;
    private float radius = 1.5f;
    private float lastAngle;


    private Transform player;

    List<GameObject> markedEnemies;


    public void Initialize(float angleOffset, float spinSpeed)
    {
        player = transform.parent;
        currentAngle = angleOffset;
        rotationSpeed = spinSpeed;
    }


    protected override void Start()
    {
        destroyAfterSeconds = Mathf.Infinity; //persist forever (unless destroyed manually)
        base.Start();

        markedEnemies = new List<GameObject>();
    }


    void Update()
    {
        if (player == null) return;

        currentAngle += rotationSpeed * Time.deltaTime;

        // Normalize angle to 0–360 range
        if (currentAngle > 360f)
            currentAngle -= 360f;

        // Detect rotation wrap-around (completed a full circle)
        if (currentAngle < lastAngle)
        {
            markedEnemies.Clear();
        }

        lastAngle = currentAngle;

        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        transform.position = player.position + offset;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !markedEnemies.Contains(collision.gameObject))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);

            markedEnemies.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Prop") && !markedEnemies.Contains(collision.gameObject))
        {
            if (collision.gameObject.TryGetComponent(out BreakabkeProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());

                markedEnemies.Add(collision.gameObject);
            }
        }
    }
}

