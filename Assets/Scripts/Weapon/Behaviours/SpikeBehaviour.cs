using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehaviour : MeleeBehaviour
{
    List<GameObject> markedEnemies;
    
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>(); 
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !markedEnemies.Contains(collision.gameObject))
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
