using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    //Different states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stats Displays")]
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentRecoveryDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentMightDisplay;
    public TextMeshProUGUI currentProjectileSpeedDisplay;
    public TextMeshProUGUI currentMagnetDisplay;
    public TextMeshProUGUI currentDamageReductionPercent;


    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI reachedLvlDisplay;
    public TextMeshProUGUI timeSurviveDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(5);
    public List<Image> chosenPassiveItemsUI = new List<Image>(5);


    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public TextMeshProUGUI stopwatchDisplay;


    //Flag for game over
    public bool isGameOver = false;

    //Flag for pöayer choosing upfrades
    public bool choosingUpgrades;

    //Reference to the player's  game object
    public GameObject playerObject;

    void Awake()
    {
        //Check to warn if there is another singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }

        DisableScreens();
    }


    void Update()
    {
       switch (currentState)
        {
            case GameState.Gameplay:
                //Code of the gameplay state
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                //Codfe of the paused state
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                //Code of the gameover state
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;    //Stop the game
                    Debug.Log("GAME IS OVER");
                    DisplayResults();
                }
                break;

            case GameState.LevelUp:
                //Code of the level up state
                if (!choosingUpgrades)
                {
                    choosingUpgrades = true;
                    Time.timeScale = 0f;    //Stop time while choosing upgrades
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;


            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        } 
    }


    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if (textFont) tmPro.font = textFont;
        rect.position = referenceCamera.WorldToScreenPoint(target.position);

        Destroy(textObj, duration);

        textObj.transform.SetParent(instance.damageTextCanvas.transform);

        //Pan the text upwards and fade it away over time
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while (t < duration)
        {
            yield return w;
            t += Time.deltaTime;

            // Safeguard checks
            if (tmPro == null || rect == null || target == null)
                yield break;

            //Fade the text
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);

            //Pan the text upwards
            yOffset += speed * Time.deltaTime;
            rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
        }

        if (textObj != null)
            GameObject.Destroy(textObj);
    }


    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        //If there is no canva set, end function 
        if (!instance.damageTextCanvas) return;

        //Find a relevant camera
        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }



    //Method to change the state of the game
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }


    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;    //Stop the game
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }


    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }


    //Method to check for pause and resume input
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }


    public void GameOver()
    {
        timeSurviveDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }


    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }


    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.CharacterName;
    }

    public void AssignReachedLvlUI(int recheadLvlData)
    {
        reachedLvlDisplay.text = recheadLvlData.ToString();
    }


    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponData.Count != chosenWeaponsUI.Count || chosenPassiveItemsUI.Count != chosenPassiveItemsData.Count)
        {
            Debug.Log("Lists have different lenghts");
            return;
        }

        //Assign chosen weapons data to chosenWeaponsUI
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        //Assign chosen passive items data to chosenPassiveItemsUI
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }


    void UpdateStopwatch()
    {
        UpdateStopwatchDisplay();


        stopwatchTime += Time.deltaTime;

        if(stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }


    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }


    public void EndLevelUp()
    {
        choosingUpgrades = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
