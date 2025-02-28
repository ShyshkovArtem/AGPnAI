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
        weaponLevels[slotIndex] = weapon.weaponData.Level;
    }

    
    public void AddPassiveItem (int slotIndex, PassiveItem passiveItem) //Add a passive item to the slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
    }


    public void LevelUpWeapon(int slotIndex)    //Lvl up the weapon in the slot
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];

            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("NO NEXT LEVEL FOR" + weapon.name);
                return;
            }

            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;
        }
    }


    public void LevelUpPassiveItem(int slotIndex)   //Lvl up the passive item in the slot
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];

            if (!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogError("NO NEXT LEVEL FOR" + passiveItem.name);
                return;
            }

            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;
        }
    }
}
