using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;


    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            am.SetBool("IsMoving", true);
            SpriteDirectionCheck();
        }
        else
        {
            am.SetBool("IsMoving", false);
        }
    }


    void SpriteDirectionCheck()
    {
        if (pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;   
        }
    }


    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        if (!am) am = GetComponent<Animator>();
        am.keepAnimatorStateOnDisable = controller;
    }
}
