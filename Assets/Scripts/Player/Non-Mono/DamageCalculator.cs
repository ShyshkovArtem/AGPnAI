

// Static class for damage reduction logic
public static class DamageCalculator
{
    public static float Reduce(float damage, float reductionPercent)
    {
        return damage * (1f - reductionPercent / 100f);
    }
}

