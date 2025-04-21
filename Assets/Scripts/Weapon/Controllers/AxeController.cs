using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : WeaponController
{
    public int numberOfAxes = 1; // Can be increased with level

    protected override void Attack()
    {
        base.Attack();

        for (int i = 0; i < numberOfAxes; i++)
        {
            GameObject axe = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
            axe.transform.parent = null;

            // Give it a random direction
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            axe.GetComponent<ProjectileWeaponBehaviour>().DirectionChecker(randomDir);
        }
    }
}
