using System;
using UnityEditor.U2D.Animation;
using UnityEngine;


/// Holds runtime player stats and broadcasts changes via events.
/// Separates data, logic, and UI update concerns.

public class PlayerAttributes : MonoBehaviour
{
    [Header("Character Data")]
    [Tooltip("ScriptableObject defining base stat values.")]
    [SerializeField] private CharacterScriptableObject _characterData;

    // Base stats (from ScriptableObject)
    public float BaseHealth { get; private set; }
    public float BaseRecovery { get; private set; }
    public float BaseMoveSpeed { get; private set; }
    public float BaseMight { get; private set; }
    public float BaseProjectileSpeed { get; private set; }
    public float BaseMagnet { get; private set; }
    public float BaseDamageReductionPercent { get; private set; }
    public float BaseExperienceMultiplier { get; private set; }

    // Current stats
    private float _currentHealth;
    private float _currentRecovery;
    private float _currentMoveSpeed;
    private float _currentMight;
    private float _currentProjectileSpeed;
    private float _currentMagnet;
    private float _currentDamageReductionPercent;
    private float _currentExperienceMultiplier;

    public float CurrentHealth
    {
        get => _currentHealth;
        set => SetStat(ref _currentHealth, value, HealthChanged, "Health");
    }
    public float CurrentRecovery
    {
        get => _currentRecovery;
        set => SetStat(ref _currentRecovery, value, RecoveryChanged, "Recovery");
    }
    public float CurrentMoveSpeed
    {
        get => _currentMoveSpeed;
        set => SetStat(ref _currentMoveSpeed, value, MoveSpeedChanged, "Move Speed");
    }
    public float CurrentMight
    {
        get => _currentMight;
        set => SetStat(ref _currentMight, value, MightChanged, "Might");
    }
    public float CurrentProjectileSpeed
    {
        get => _currentProjectileSpeed;
        set => SetStat(ref _currentProjectileSpeed, value, ProjectileSpeedChanged, "Projectile Speed");
    }
    public float CurrentMagnet
    {
        get => _currentMagnet;
        set => SetStat(ref _currentMagnet, value, MagnetChanged, "Magnet");
    }
    public float CurrentDamageReductionPercent
    {
        get => _currentDamageReductionPercent;
        set => SetStat(ref _currentDamageReductionPercent, value, DamageReductionChanged, "Damage Reduction");
    }
    public float CurrentExperienceMultiplier
    {
        get => _currentExperienceMultiplier;
        set => SetStat(ref _currentExperienceMultiplier, value, ExperienceMultiplierChanged, "Exp Multiplier");
    }

    // Events - broadcast stat name and new value (or specialized if desired)
    public event Action<float> HealthChanged;
    public event Action<float> RecoveryChanged;
    public event Action<float> MoveSpeedChanged;
    public event Action<float> MightChanged;
    public event Action<float> ProjectileSpeedChanged;
    public event Action<float> MagnetChanged;
    public event Action<float> DamageReductionChanged;
    public event Action<float> ExperienceMultiplierChanged;

    // Generic setter with change check and event invoke
    private void SetStat(ref float field, float value, Action<float> changedEvent, string statName)
    {
        if (Mathf.Approximately(field, value)) return;
        field = value;
        changedEvent?.Invoke(field);
    }

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        if (_characterData == null)
            Debug.LogError("CharacterData is not assigned", this);

        // Cache InventoryManager
        _inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        Initialize(_characterData);
    }


    /// Initializes base and current stats from the provided CharacterData, and spawns starting weapon.
    public void Initialize(CharacterScriptableObject data)
    {
        if (data == null) return;

        // Load base values
        BaseHealth = data.MaxHealth;
        BaseRecovery = data.Recovery;
        BaseMoveSpeed = data.MoveSpeed;
        BaseMight = data.Might;
        BaseProjectileSpeed = data.ProjectileSpeed;
        BaseMagnet = data.Magnet;
        BaseDamageReductionPercent = data.DamageReductionPercent;
        BaseExperienceMultiplier = data.ExperienceMultiplier;

        // Set current to base
        CurrentHealth = BaseHealth;
        CurrentRecovery = BaseRecovery;
        CurrentMoveSpeed = BaseMoveSpeed;
        CurrentMight = BaseMight;
        CurrentProjectileSpeed = BaseProjectileSpeed;
        CurrentMagnet = BaseMagnet;
        CurrentDamageReductionPercent = BaseDamageReductionPercent;
        CurrentExperienceMultiplier = BaseExperienceMultiplier;

        // Spawn weapon
        if (_inventoryManager != null)
            _inventoryManager.SpawnWeapon(data.StartingWeapon);
    }
}