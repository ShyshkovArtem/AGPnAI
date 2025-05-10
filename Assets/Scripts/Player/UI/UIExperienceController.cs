using TMPro;
using UnityEngine;
using UnityEngine.UI;

// UI controller for experience bar
public class UIExperienceController : MonoBehaviour
{
    [SerializeField] private Image _expBar;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private PlayerExperience _experience;

    private void Awake()
    {
        if (_experience == null)
            Debug.LogWarning("PlayerExperience reference not set!", this);
    }

    private void OnEnable()
    {
        _experience.ExpChanged += UpdateExpBar;
        _experience.LevelReached += UpdateLevelText;
    }

    private void OnDisable()
    {
        _experience.ExpChanged -= UpdateExpBar;
        _experience.LevelReached -= UpdateLevelText;
    }

    private void UpdateExpBar(int current, int cap)
    {
        if (_expBar != null)
            _expBar.fillAmount = (float)current / cap;
    }

    private void UpdateLevelText(int level)
    {
        if (_levelText != null)
            _levelText.text = $"LVL {level}";
    }
}
