using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponScriptableObjects", menuName ="ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{

    public GameObject prefab;
    //Base stats for weapons
    public float damage;
    public float speed;
    public float cooldownDuration;
    public int pierce;


    void Update()
    {
        
    }
}
