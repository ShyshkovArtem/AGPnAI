using UnityEngine;
using UnityEngine.UI;

public class UIHealth: MonoBehaviour, IUIHealth
{
    public Image healthBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
}

