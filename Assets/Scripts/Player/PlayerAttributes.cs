using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // Attributes
    private float currentHealth;
    private float currentRecovery;
    private float currentMoveSpeed;
    private float currentMight;
    private float currentProjectileSpeed;
    private float currentMagnet;
    private float currentDamageReductionPercent;
    private float currentExperienceMultiplier;


    // Base stats 
    public float baseHealth;
    public float baseRecovery;
    public float baseMoveSpeed;
    public float baseMight;
    public float baseProjectileSpeed;
    public float baseMagnet;
    public float baseDamageReductionPercent;
    public float baseExperienceMultiplier;


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


    InventoryManager inventoryManager;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();
    }

    private void Start()
    {
        Initialize(characterData);
        UpdateStatsDisplay();
        GameManager.instance.AssignChosenCharacterUI(characterData);
    }


    public void Initialize(CharacterScriptableObject characterData)         // Initialize from CharacterData
    {
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
        currentDamageReductionPercent = characterData.DamageReductionPercent;
        currentExperienceMultiplier = 1f;

        baseHealth = currentHealth;
        baseRecovery = currentRecovery;
        baseMoveSpeed = currentMoveSpeed;
        baseMight = currentMight;
        baseProjectileSpeed = currentProjectileSpeed;
        baseMagnet = currentMagnet;
        baseDamageReductionPercent = currentDamageReductionPercent;
        baseExperienceMultiplier = 1f;


        //Spawn the starting weapon
        inventoryManager = GetComponent<InventoryManager>();
        inventoryManager.SpawnWeapon(characterData.StartingWeapon);
    }


    public void UpdateStatsDisplay()
    {
        //Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
        GameManager.instance.currentDamageReductionPercent.text = "Damage reduction: " + currentDamageReductionPercent + "%";
    }
}

