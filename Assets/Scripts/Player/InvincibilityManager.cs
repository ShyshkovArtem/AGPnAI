using UnityEngine;

public class InvincibilityManager : MonoBehaviour, IInvincibilityManager
{
    private bool _isInvincible;
    private float _invincibilityDuration = 0.5f;

    public bool CanTakeDamage()
    {
        return !_isInvincible;
    }

    public void TriggerInvincibility()
    {
        _isInvincible = true;
        Invoke("DisableInvincibility", _invincibilityDuration);
    }

    private void DisableInvincibility()
    {
        _isInvincible = false;
    }
}

