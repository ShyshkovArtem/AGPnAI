using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        // If running in the Unity Editor
        EditorApplication.isPlaying = false;
#else
        // If running in a build
        Application.Quit();
#endif
    }
}
