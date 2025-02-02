using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehaviour : ProjectileWeaponBehaviour
{
    SpearController sc;

    protected override void Start()
    {
        base.Start();
        sc = FindObjectOfType<SpearController>();
    }

    
    void Update()
    {
        transform.position += direction * sc.speed * Time.deltaTime; // Set movement of the spear
    }
}
