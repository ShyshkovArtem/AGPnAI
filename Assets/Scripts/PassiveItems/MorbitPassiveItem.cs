using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorbitPassiveItem : PassiveItem
{
    private float healAmount;

    protected override void ApplyModifier()
    {
        healAmount = passiveItemData.HealthPerKill;

        EnemyStats.OnAnyEnemyKilled += Heal;
    }

    void Heal()
    {
        playerStats.Heal(healAmount);
    }


    private void OnDestroy()            // Unsubscribe to avoid memory leaks
    {
        EnemyStats.OnAnyEnemyKilled -= Heal;
    }
}
