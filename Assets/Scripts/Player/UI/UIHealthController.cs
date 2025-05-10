using UnityEngine;


// Updates the UI health bar in response to health changes
public class UIHealthController : MonoBehaviour
{
    private IUIHealth _uiHealth;
    private PlayerStats _stats;

    private void Awake()
    {
        _uiHealth = GetComponent<IUIHealth>() ?? WarnMissing<IUIHealth>();
        _stats = GetComponent<PlayerStats>() ?? WarnMissing<PlayerStats>();
    }

    private void OnEnable()
    {
        _stats.HealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        _stats.HealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        _uiHealth.UpdateHealthBar(current, max);
    }

    private T WarnMissing<T>() where T : class
    {
        Debug.LogWarning($"{typeof(T).Name} is missing on {gameObject.name}", this);
        return null;
    }
}
