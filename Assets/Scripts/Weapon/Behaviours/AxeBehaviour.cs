using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBehaviour : ProjectileWeaponBehaviour
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.position += direction.normalized * currentMoveSpeed * Time.deltaTime;
    }
}
