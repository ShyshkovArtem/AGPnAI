using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    public float invincibilityDuration = 0.5f;
    private float timer = 0f;

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    public bool CanTakeDamage() => timer <= 0;

    public void TriggerInvincibility()
    {
        timer = invincibilityDuration;
    }
}

