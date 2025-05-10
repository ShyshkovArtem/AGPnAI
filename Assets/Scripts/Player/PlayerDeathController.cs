using UnityEngine;

// Handles player death and game-over logic
public class PlayerDeathController : MonoBehaviour
{
    private PlayerStats _stats;
    private InventoryManager _inventory;
    private PlayerExperience _experience;


    private void Awake()
    {
        _stats = GetComponent<PlayerStats>() ?? WarnMissing<PlayerStats>();
        _inventory = GetComponent<InventoryManager>() ?? WarnMissing<InventoryManager>();
        _experience = GetComponent<PlayerExperience>() ?? WarnMissing<PlayerExperience>();
    }

    private void OnEnable()
    {
        _stats.OnDeath += ProcessDeath;
    }


    private void OnDisable()
    {
        _stats.OnDeath -= ProcessDeath;
    }


    private void ProcessDeath()
    {
        if (GameManager.instance != null && !GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
            GameManager.instance.AssignReachedLvlUI(_experience.CurrentLevel);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(
                _inventory.weaponUISlots,
                _inventory.passiveItemUISlots
            );
        }
        Debug.Log("Player dead");
    }


    private T WarnMissing<T>() where T : class
    {
        Debug.LogWarning($"{typeof(T).Name} is missing on {gameObject.name}", this);
        return null;
    }
}
