using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPassiveItem : PassiveItem
{

    protected override void ApplyModifier()
    {
        playerAttributes.CurrentMoveSpeed = playerAttributes.BaseMoveSpeed * (1 + passiveItemData.Multipler / 100f);
    }

}
