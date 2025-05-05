using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    [Header("Exp/Lvl")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class for defining a level range and the corresponding exp cap for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    [Header("UI")]
    public Image expBar;
    public TextMeshProUGUI levelText;

    public List<LevelRange> levelRanges;

    //References
    private PlayerAttributes playerAttributes;


    void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
    }


    private void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        UpdateExpBar();
        UpdateLevelText();
    }


    public void GainExperience(int amount)
    {
        int modifiedAmount = Mathf.RoundToInt(amount * playerAttributes.CurrentExperienceMultiplier);
        experience += modifiedAmount;


        LevelUpChecker();
        UpdateExpBar();
    }


    public void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }


    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }


    void UpdateLevelText()
    {
        levelText.text = "LVL " + level.ToString();
    }
}

