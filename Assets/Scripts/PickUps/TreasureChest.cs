using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoryManager inventoryManager;


    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OpenTreasure();
            Destroy(gameObject);
        }
    }


    public void OpenTreasure()
    {
        if (inventoryManager.GetPossibleEvolutions().Count <= 0)
        {
            Debug.Log("No avaible evolutions");
            return;
        }

        WeaponEvolutionBlueprint toEvolve = inventoryManager.GetPossibleEvolutions()[Random.Range(0, inventoryManager.GetPossibleEvolutions().Count)];
        inventoryManager.EvolveWeapon(toEvolve);
    }
}
