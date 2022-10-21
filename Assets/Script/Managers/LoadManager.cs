using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public IEnumerator LoadScene(string sceneName, bool WithHUD)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadSceneWithHUD(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName,true));
    }
    public void LoadSceneWithoutHUD(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName, false));
    }
}
