using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LevelRange
{
    public int Start;
    public int End;
    public int CapIncrease;
}

// Experience gain and level progression
public class PlayerExperience : MonoBehaviour
{
    [Header("Exp/Lvl")]
    [SerializeField] private int _currentExp;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _expCap;

    public int CurrentExp
    {
        get => _currentExp;
        private set => _currentExp = value;
    }
    public int CurrentLevel
    {
        get => _currentLevel;
        private set => _currentLevel = value;
    }
    public int ExpCap
    {
        get => _expCap;
        private set => _expCap = value;
    }


    [Header("Level Ranges")]
    [SerializeField] private List<LevelRange> _levelRanges;

    // Events for decoupled handling
    public event Action<int, int> ExpChanged;        // (currentExp, expCap)
    public event Action<int> LevelReached;            // new level reached

    private PlayerAttributes _attributes;


    private void Awake()
    {
        _attributes = GetComponent<PlayerAttributes>() ?? WarnMissing<PlayerAttributes>();
    }


    private void Start()
    {
        ExpCap = DetermineCapForLevel(CurrentLevel);
        PublishExpChange();
        PublishLevelReached();
    }


    // Add experience, apply multiplier, and check for level up.
    public void GainExperience(int baseAmount)
    {
        int modified = ExpCalculator.ApplyMultiplier(baseAmount, _attributes.CurrentExperienceMultiplier);
        CurrentExp += modified;
        PublishExpChange();

        while (CurrentExp >= ExpCap)
        {
            LevelUp();
        }
    }


    private void LevelUp()
    {
        CurrentExp -= ExpCap;
        CurrentLevel++;
        ExpCap = DetermineCapForLevel(CurrentLevel);

        PublishLevelReached();
        PublishExpChange();

        GameManager.instance?.StartLevelUp();
    }


    private int DetermineCapForLevel(int level)
    {
        foreach (var range in _levelRanges)
        {
            if (level >= range.Start && level <= range.End)
                return range.CapIncrease;
        }
        Debug.LogWarning($"No LevelRange covers level {level} on {gameObject.name}", this);
        return _levelRanges.Count > 0 ? _levelRanges[0].CapIncrease : 100;
    }


    private void PublishExpChange()
    {
        ExpChanged?.Invoke(CurrentExp, ExpCap);
    }


    private void PublishLevelReached()
    {
        LevelReached?.Invoke(CurrentLevel);
    }


    private T WarnMissing<T>() where T : class
    {
        Debug.LogWarning($"{typeof(T).Name} is missing on {gameObject.name}", this);
        return null;
    }
}