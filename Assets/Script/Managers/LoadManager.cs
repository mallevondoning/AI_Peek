using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
    public IEnumerator LoadSceneCoroutineAddative(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
    }

    public void LoadSceneFunc(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public void LoadSceneFunc(string sceneName, bool isAdditive)
    {
        if (!isAdditive)
            StartCoroutine(LoadSceneCoroutine(sceneName));
        else
            StartCoroutine(LoadSceneCoroutineAddative(sceneName));
    }
}
