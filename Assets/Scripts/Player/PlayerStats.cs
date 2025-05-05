using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private PlayerAttributes playerAttributes;
    private PlayerInvincibility playerInvincibility;
    private PlayerExperience playerExperience;
    private InventoryManager inventoryManager;

    public ParticleSystem damageEffect;

    [Header("UI")]
    public Image healthBar;

    void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerInvincibility = GetComponent<PlayerInvincibility>();
        playerExperience = GetComponent<PlayerExperience>();
        inventoryManager = GetComponent<InventoryManager>();

        UpdateHealthBar();
    }


    private void Start()
    {

    }


    public void TakeDamage(float dmg)
    {
        if (playerInvincibility.CanTakeDamage())
        {
            float reducedDamage = dmg * (1 - playerAttributes.CurrentDamageReductionPercent / 100f);
            playerAttributes.CurrentHealth -= reducedDamage;

            //If there is assigned damage effect, play it
            if (damageEffect) Instantiate(damageEffect, transform.position, Quaternion.identity);

            playerInvincibility.TriggerInvincibility();


            if (playerAttributes.CurrentHealth <= 0)
            {
                Die();
            }

            UpdateHealthBar();
        }
    }


    void UpdateHealthBar()
    {
        healthBar.fillAmount = playerAttributes.CurrentHealth / playerAttributes.baseHealth;
    }


    public void Heal(float amount)
    {
        playerAttributes.CurrentHealth = Mathf.Min(playerAttributes.CurrentHealth + amount, playerAttributes.baseHealth);
        UpdateHealthBar();
    }


    private void Die()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
            GameManager.instance.AssignReachedLvlUI(playerExperience.level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventoryManager.weaponUISlots, inventoryManager.passiveItemUISlots);
        }

        Debug.Log("Player dead");
    }
}



