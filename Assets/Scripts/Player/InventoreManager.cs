using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoreManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(5);
    public int[] weaponLevels = new int[5]; 
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(5);
    public int[] passiveItemLevels = new int[5];

    
    public void AddWeapon (int slotIndex, WeaponController weapon)  //Add a weapon to the slot
    {
        weaponSlots[slotIndex] = weapon;
    }

    
    public void AddPassiveItem (int slotIndex, PassiveItem passiveItem) //Add a passive item to the slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
    }


    public void LevelUpWeapon(int slotIndex)    //Lvl up the weapon in the slot
    {

    }


    public void LevelUpPassiveItem(int slotIndex)   //Lvl up the passive item in the slot
    {

    }
}
