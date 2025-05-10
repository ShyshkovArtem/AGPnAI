using UnityEngine;

// Static class for experience calculation
public static class ExpCalculator
{
    public static int ApplyMultiplier(int baseAmount, float multiplier)
    {
        return Mathf.RoundToInt(baseAmount * multiplier);
    }
}
