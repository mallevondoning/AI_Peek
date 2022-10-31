using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FirstTimeLoad
{
    private static bool _isInitialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void FirstSetupCheck()
    {
        if (_isInitialized)
            return;
        
        GameObject GameManagerObject = new GameObject("GameManager");

        GameManagerObject.AddComponent<GameManager>();
        GameManagerObject.AddComponent<LoadManager>();
        GameManagerObject.AddComponent<EventSystem>();
        GameManagerObject.AddComponent<StandaloneInputModule>();

        Object.DontDestroyOnLoad(GameManagerObject);

        int sceneAmount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneAmount; i++)
        {
            if (Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)).Contains("Level"))
            {
                GameManager.Instance.LevelList.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }

        //<fix>? need to load this scene for the loading system to work
        LoadManager.Instance.LoadSceneFunc("LoadingScene", true);
        //</fix>?

        _isInitialized = true;
    }
}
