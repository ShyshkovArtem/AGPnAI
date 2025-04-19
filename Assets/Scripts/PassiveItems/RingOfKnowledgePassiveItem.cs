using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfKnowledgePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentExperienceMultiplier = passiveItemData.Multipler;
    }

}
