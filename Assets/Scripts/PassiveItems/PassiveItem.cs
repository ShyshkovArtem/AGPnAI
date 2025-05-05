using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerAttributes playerAttributes;
    protected PlayerStats playerStats;
    public PassiveItemScriptableObject passiveItemData;


    protected virtual void ApplyModifier()
    {
        //Apply the boost value to the stat in the child classes
    }


    void Start()
    {
        playerAttributes = FindObjectOfType<PlayerAttributes>();
        playerStats = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }

    
    void Update()
    {
        
    }
}
