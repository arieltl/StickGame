using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        LoadLevel("Local Multiplayer");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
   void LoadLevel(string levelName) {
        StartCoroutine(LoadLevelAsync(levelName));
    }
    
    IEnumerator LoadLevelAsync(string levelName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        if (asyncLoad == null) {
            Debug.LogError("Failed to load level " + levelName);
            yield return null;
        }
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
        
}
