public interface IPlayerStats
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    float CurrentDamageReductionPercent { get; }

    void TakeDamage(float dmg);
    void Heal(float amount);
}



