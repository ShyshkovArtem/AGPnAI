using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    //Current Stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;
    float currentDamageReductionPercent;
    float currentExperienceMultiplier;

    //Base Stats
    public float BaseMoveSpeed;
    public float BaseHealth;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {   
            //Cheeck if the value has changed
            if (currentHealth != value)
            {
                currentHealth = value;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
            }
        }
    }
    
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //Cheeck if the value has changed
            if (currentRecovery != value)
            {
                currentRecovery = value;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //Cheeck if the value has changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.currentMoveSpeedDisplay.text = "Move speed: " + currentMoveSpeed;
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //Cheeck if the value has changed
            if (currentMight != value)
            {
                currentMight = value;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            //Cheeck if the value has changed
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile speed: " + currentProjectileSpeed;
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            //Cheeck if the value has changed
            if (currentMagnet != value)
            {
                currentMagnet = value;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
            }
        }
    }

    public float CurrentDamageReductionPercent
    {
        get { return currentDamageReductionPercent; }
        set
        {
            //Cheeck if the value has changed
            if (currentDamageReductionPercent != value)
            {
                currentDamageReductionPercent = value;

                //Update UI
                if (GameManager.instance != null && GameManager.instance.currentDamageReductionPercent != null)
                {
                    GameManager.instance.currentDamageReductionPercent.text = "Damage Reduction: " + currentDamageReductionPercent + "%";
                }
            }
        }
    }

    public float CurrentExperienceMultiplier { get; set; } = 1f;
    #endregion

    public ParticleSystem damageEffect;

    //Exp and level
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

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoreManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;


    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoreManager>();

        //Assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;
        CurrentDamageReductionPercent = characterData.DamageReductionPercent;

        BaseMoveSpeed = CurrentMoveSpeed;
        BaseHealth = CurrentHealth;
        //Spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
    }


    void Start()
    {
        //Initialize the exp cap
        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
        GameManager.instance.currentDamageReductionPercent.text = "Damage reduction: " + currentDamageReductionPercent + "%";

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }


    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
    }

    public void IncreaseExperience(int amount)
    {
        int modifiedAmount = Mathf.RoundToInt(amount * CurrentExperienceMultiplier);
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
                if(level >= range.startLevel && level <= range.endLevel)
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


    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            float reducedDamage = dmg * (1 - CurrentDamageReductionPercent / 100f);
            CurrentHealth -= reducedDamage;

            //If there is assigned damage effect, play it
            if (damageEffect) Instantiate(damageEffect, transform.position, Quaternion.identity);

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
    }


    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }


    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
            GameManager.instance.AssignReachedLvlUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
        }
    }


    public void RestoreHealth (float amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }

        UpdateHealthBar();
    }


    public void SpawnWeapon(GameObject weapon)
    {
        //Checking if there are avaible inventory slots
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventroy slots are already full");
            return;
        }

        //Spawn starting weapon 
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);   //Set the weapon to be a child of the player 
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());   //Add the weapon the the inventory slot

        weaponIndex++;
    }


    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Checking if there are avaible inventory slots
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventroy slots are already full");
            return;
        }

        //Spawn starting passive item 
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);   //Set the passive item to be a child of the player 
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());   //Add the passive item the the inventory slot

        passiveItemIndex++;
    }
}
