using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : WeaponController
{
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedMelee = Instantiate(weaponData.prefab);
        spawnedMelee.transform.position = transform.position;   //Assign position to the player
        spawnedMelee.transform.parent = transform;
    }
}
