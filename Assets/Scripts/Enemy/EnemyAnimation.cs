using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Transform player;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().transform;
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
}
