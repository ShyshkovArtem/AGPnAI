using UnityEngine;

// Listens to PlayerAttributes events and updates GameManager UI fields accordingly.

public class UIAttributesController : MonoBehaviour
{
    [SerializeField] private PlayerAttributes attributes;

    private void Awake()
    {
        if (attributes == null)
            attributes = FindObjectOfType<PlayerAttributes>();
    }

    private void OnEnable()
    {
        attributes.HealthChanged += h => UpdateDisplay(GameManager.instance.currentHealthDisplay, "Health: ", h);
        attributes.RecoveryChanged += v => UpdateDisplay(GameManager.instance.currentRecoveryDisplay, "Recovery: ", v);
        attributes.MoveSpeedChanged += v => UpdateDisplay(GameManager.instance.currentMoveSpeedDisplay, "Move speed: ", v);
        attributes.MightChanged += v => UpdateDisplay(GameManager.instance.currentMightDisplay, "Might: ", v);
        attributes.ProjectileSpeedChanged += v => UpdateDisplay(GameManager.instance.currentProjectileSpeedDisplay, "Projectile speed: ", v);
        attributes.MagnetChanged += v => UpdateDisplay(GameManager.instance.currentMagnetDisplay, "Magnet: ", v);
        attributes.DamageReductionChanged += v => UpdateDisplay(GameManager.instance.currentDamageReductionPercent, "Damage Reduction: ", v, suffix: "%");
    }

    private void OnDisable()
    {
        // Should mirror OnEnable subscriptions to unsubscribe
        attributes.HealthChanged -= h => UpdateDisplay(GameManager.instance.currentHealthDisplay, "Health: ", h);
        attributes.RecoveryChanged -= v => UpdateDisplay(GameManager.instance.currentRecoveryDisplay, "Recovery: ", v);
        attributes.MoveSpeedChanged -= v => UpdateDisplay(GameManager.instance.currentMoveSpeedDisplay, "Move speed: ", v);
        attributes.MightChanged -= v => UpdateDisplay(GameManager.instance.currentMightDisplay, "Might: ", v);
        attributes.ProjectileSpeedChanged -= v => UpdateDisplay(GameManager.instance.currentProjectileSpeedDisplay, "Projectile speed: ", v);
        attributes.MagnetChanged -= v => UpdateDisplay(GameManager.instance.currentMagnetDisplay, "Magnet: ", v);
        attributes.DamageReductionChanged -= v => UpdateDisplay(GameManager.instance.currentDamageReductionPercent, "Damage Reduction: ", v, suffix: "%");
    }

    private void UpdateDisplay(TMPro.TextMeshProUGUI textField, string prefix, float value, string suffix = "")
    {
        if (textField != null)
            textField.text = prefix + value + suffix;
    }
}