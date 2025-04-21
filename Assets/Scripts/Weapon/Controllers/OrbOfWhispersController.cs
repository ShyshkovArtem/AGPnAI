using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOfWhispersController : WeaponController
{
    public int orbCount = 1;

    protected override void Start()
    {
        base.Start();
        SpawnOrbs();
    }

    void SpawnOrbs()
    {
        for (int i = 0; i < orbCount; i++)
        {
            float angle = (360f / orbCount) * i;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject orb = Instantiate(weaponData.Prefab, transform.position, rotation, transform);
            orb.GetComponent<OrbOfWhispersBehaviour>().Initialize(angle, weaponData.Speed);
        }
    }
}
