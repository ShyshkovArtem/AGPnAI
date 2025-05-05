using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerAttributes.CurrentMight *= 1 + passiveItemData.Multipler / 100f;
    }
}
