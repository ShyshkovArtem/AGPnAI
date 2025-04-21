using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();

        Vector3 baseDirection = ((Vector3)pm.lastMovedVector != Vector3.zero) ? (Vector3)pm.lastMovedVector : Vector3.right;
        int level = weaponData.Level;
        int boomerangCount = 1;

        if (level >= 5)
            boomerangCount = 3;
        else if (level >= 3)
            boomerangCount = 2;

        float spreadAngle = 20f; // degrees between each boomerang

        for (int i = 0; i < boomerangCount; i++)
        {
            float angleOffset = 0;

            if (boomerangCount > 1)
                angleOffset = (i - (boomerangCount - 1) / 2f) * spreadAngle;

            // Rotate direction by offset
            Vector3 rotatedDir = Quaternion.Euler(0, 0, angleOffset) * baseDirection;

            GameObject boomerang = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
            boomerang.GetComponent<BoomerangBehaviour>().Initialize(rotatedDir.normalized);
        }
    }
}
