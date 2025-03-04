using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    //Current Stats
    float currentHealth;
    float currentRecovery;
    public float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    public float currentMagnet;

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
        }
    }


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

    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;

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

        //Spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPassiveItemTest); 
        SpawnPassiveItem(secondPassiveItemTest);
    }


    void Start()
    {
        //Initialize the exp cap
        experienceCap = levelRanges[0].experienceCapIncrease;
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
        experience += amount;
        LevelUpChecker();
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
        }
    }


    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
        
    }


    public void Kill()
    {
        Debug.Log("YOU ARE DEAD");
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
