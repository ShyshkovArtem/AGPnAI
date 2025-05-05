using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGems : Pickup
{
    public int experinceGranted;

    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }

        PlayerExperience playerExperience = FindObjectOfType<PlayerExperience>();
        playerExperience.GainExperience(experinceGranted);
    }
}
