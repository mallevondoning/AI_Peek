using System.Collections;
using System.Collections.Generic;
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

        _isInitialized = true;
    }
}
