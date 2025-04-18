using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsPassiveItem : PassiveItem
{

    protected override void ApplyModifier()
    {
        player.CurrentMoveSpeed = player.BaseMoveSpeed * (1 + passiveItemData.Multipler / 100f);
    }

}
