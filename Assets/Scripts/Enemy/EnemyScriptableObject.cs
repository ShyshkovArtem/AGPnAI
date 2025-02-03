using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="EnemyScriptableObjects" , menuName ="ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //Base stats for enemies
    public float moveSpeed;
    public float maxHealth;
    public float damage;
    
    void Update()
    {
        
    }
}
