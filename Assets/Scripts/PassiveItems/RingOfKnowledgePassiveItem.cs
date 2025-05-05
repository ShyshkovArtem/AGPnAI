using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfKnowledgePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerAttributes.CurrentExperienceMultiplier = passiveItemData.Multipler;
    }

}
