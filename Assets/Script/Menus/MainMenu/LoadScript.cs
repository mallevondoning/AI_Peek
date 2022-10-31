using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour
{
    public void StartButton()
    {
        LoadManager.Instance.LoadSceneFunc(GameManager.Instance.LevelList[DataManager.Level]);
        if (!SceneManager.GetSceneByName("HUD").isLoaded)
            LoadManager.Instance.LoadSceneFunc("HUD", true);
    }
    public void ExitButton()
    {
        Debug.Log("You have quit the game");
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        LoadManager.Instance.LoadSceneFunc(sceneName);
    }
}
