using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base for all melee behaviour

public class MeleeBehaviour : MonoBehaviour
{
    public float destroyAfterSeconds;


    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

   
}
