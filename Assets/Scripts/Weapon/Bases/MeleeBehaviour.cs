using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base for all melee behaviour

public class MeleeBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    public float destroyAfterSeconds;

    //Current stats
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


    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Refference to the collided collider script and use TakeDamage() from it
        if (collision.CompareTag("Enemy"))
        {
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
        }
        else if (collision.CompareTag("Prop"))
        {
            if (collision.gameObject.TryGetComponent(out BreakabkeProps breakable))
            {
                breakable.TakeDamage(currentDamage);
            }
        }
    }
}
