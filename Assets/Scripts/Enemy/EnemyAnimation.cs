using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Transform player;
    SpriteRenderer sr;
    Animator animator;
    EnemyStats stats;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().transform;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (transform.position.x > player.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

    }


    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
}
