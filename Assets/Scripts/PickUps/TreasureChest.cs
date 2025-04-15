using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    InventoreManager inventory;


    private void Start()
    {
        inventory = FindObjectOfType<InventoreManager>();
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
        if (inventory.GetPossibleEvolutions().Count <= 0)
        {
            Debug.Log("No avaible evolutions");
            return;
        }

        WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolutions()[Random.Range(0, inventory.GetPossibleEvolutions().Count)];
        inventory.EvolveWeapon(toEvolve);
    }
}
