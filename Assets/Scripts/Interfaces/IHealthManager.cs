public interface IHealthManager
{
    void TakeDamage(float damage);
    void Heal(float amount);
    float GetCurrentHealth();
    float GetMaxHealth();
    void SetHealth(float value);
}

