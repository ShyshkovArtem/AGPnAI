using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGems : Pickup, ICollectible
{
    public int experinceGranted;

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experinceGranted);
    }
}
