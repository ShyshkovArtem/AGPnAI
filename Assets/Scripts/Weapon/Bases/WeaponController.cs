using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base script for all weapons

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected PlayerMovement pm;

    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration; //Set current cooldown to be cooldown duration
    }


    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)  //Automatic attack after cooldown
        {
            Attack();
        }
    }


    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }

    protected virtual void OnDestroy()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Orb"))
                Destroy(child.gameObject);
        }
    }
}
