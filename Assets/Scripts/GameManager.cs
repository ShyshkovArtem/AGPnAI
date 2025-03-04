using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    //Different states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;



    void Awake()
    {
        DisableScreens();
    }


    void Update()
    {
       switch (currentState)
        {
            case GameState.Gameplay:
                //Code of the gameplay state
                CheckForPauseAndResume();
                break;

            case GameState.Paused:
                //Codfe of the paused state
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                //Code of the gameover state
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        } 
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
    }
}
