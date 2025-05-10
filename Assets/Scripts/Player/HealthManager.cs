using UnityEngine;

public class HealthManager : MonoBehaviour, IHealthManager
{
    private PlayerAttributes _attributes;

    private void Awake()
    {
        _attributes = GetComponent<PlayerAttributes>();

        if (_attributes == null)
        {
            Debug.LogError("PlayerAttributes component is missing on this GameObject!", this);
        }
        else
        {
            Debug.Log("PlayerAttributes component found.");
        }
    }

    public void TakeDamage(float damage)
    {
        _attributes.CurrentHealth -= damage;
    }

    public void Heal(float amount)
    {
        _attributes.CurrentHealth = Mathf.Min(_attributes.CurrentHealth + amount, _attributes.BaseHealth);
    }

    public float GetCurrentHealth()
    {
        return _attributes.CurrentHealth;
    }

    public float GetMaxHealth()
    {
        return _attributes.BaseHealth;
    }

    public void SetHealth(float value)
    {
        _attributes.CurrentHealth = value;
    }
}

