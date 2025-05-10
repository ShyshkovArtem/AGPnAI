using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : WeaponController
{
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSpear = Instantiate(weaponData.Prefab);
        spawnedSpear.transform.position = transform.position; //Assigned spawned spear position to the player
        spawnedSpear.GetComponent<SpearBehaviour>().DirectionChecker(pm.LastMovedDirection);  //Reference plater move direction
    }
}
