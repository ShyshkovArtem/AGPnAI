using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base script for all projectile weapons
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    //Current Stats 
    protected float currentDamage;
    protected float currentMoveSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;


    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentMoveSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }


    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }


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


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Refference to the collided collider script and use TakeDamage() from it
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());
            ReducePierce();
        }
        else if (collision.CompareTag("Prop"))
        {
            if (collision.gameObject.TryGetComponent(out BreakabkeProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }


    void ReducePierce() //Destroy after pierce reaches 0
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
