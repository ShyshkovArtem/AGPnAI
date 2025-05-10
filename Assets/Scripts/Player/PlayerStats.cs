using System;
using UnityEngine;

// Orchestrates player health, damage, healing, and death events
public class PlayerStats : MonoBehaviour
{
    // === Dependencies ===
    private IHealthManager _healthManager;
    private IInvincibilityManager _invincibilityManager;
    private PlayerAttributes _attributes;
    private InventoryManager _inventory;
    private PlayerExperience _experience;

    [Header("Effects")]
    public ParticleSystem damageEffect;

    // Events for decoupled handling
    public event Action<float, float> HealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        _healthManager = GetComponent<IHealthManager>() ?? WarnMissing<IHealthManager>();
        _invincibilityManager = GetComponent<IInvincibilityManager>() ?? WarnMissing<IInvincibilityManager>();
        _attributes = GetComponent<PlayerAttributes>() ?? WarnMissing<PlayerAttributes>();
        _inventory = GetComponent<InventoryManager>() ?? WarnMissing<InventoryManager>();
        _experience = GetComponent<PlayerExperience>() ?? WarnMissing<PlayerExperience>();
    }

    private void Start()
    {
        PublishHealthChange();                          // Initialize UI with current values
    }


    public void TakeDamage(float rawDamage)             // Applies damage to the player, triggers effects and events.
    {
        if (!_invincibilityManager.CanTakeDamage())
            return;

        float reduced = DamageCalculator.Reduce(rawDamage, _attributes.CurrentDamageReductionPercent);
        _healthManager.TakeDamage(reduced);

        SpawnDamageEffect();
        _invincibilityManager.TriggerInvincibility();

        PublishHealthChange();

        if (_healthManager.GetCurrentHealth() <= 0f)
            HandleDeath();
    }


    public void Heal(float amount)                      // Heals the player and notifies listeners.
    {
        _healthManager.Heal(amount);
        PublishHealthChange();
    }


    private void SpawnDamageEffect()
    {
        if (damageEffect != null)
            Instantiate(damageEffect, transform.position, Quaternion.identity);
    }


    private void PublishHealthChange()
    {
        HealthChanged?.Invoke(_healthManager.GetCurrentHealth(), _healthManager.GetMaxHealth());
    }


    private void HandleDeath()
    {
        OnDeath?.Invoke();
    }


    // Logs a warning if a required component is missing
    private T WarnMissing<T>() where T : class
    {
        Debug.LogWarning($"{typeof(T).Name} is missing on {gameObject.name}", this);
        return null;
    }
}





