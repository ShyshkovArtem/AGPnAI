using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base script for all projectile weapons
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;


    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    
    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += -45;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;
    }
}
