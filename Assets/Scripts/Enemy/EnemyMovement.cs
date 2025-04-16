using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (knockbackDuration > 0)  //if currently being knockedback, then procces the knockback
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else    //Otherwise, move the enemy towards the player
        {
            transform .position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        }

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);    //move to the player
    }


    public void Knockback(Vector2 velocity, float duration)   //Should be called from other scripts to create knockback
    {
        if (knockbackDuration > 0) return;

        knockbackDuration = duration;
        knockbackVelocity = velocity;
    }
}
