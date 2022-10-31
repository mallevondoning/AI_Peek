using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;

    private static float _extraWait;

    private void Awake()
    {
        Instance = this;

        _extraWait = 0.5f;
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (SceneManager.GetActiveScene() != null)
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        yield return new WaitForSeconds(_extraWait);
    }
    private IEnumerator LoadSceneCoroutineAddative(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        yield return new WaitForSeconds(_extraWait);
    }
    private IEnumerator UnloadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);

        yield return new WaitForSeconds(_extraWait);
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
    public void UnloadSceneFunc(string sceneName)
    {
        StartCoroutine(UnloadSceneCoroutine(sceneName));
    }

    public void FadeInLoading()
    {
        DataManager.LoadingAnimator.Play(DataManager.LoadingAnimations[0]);
    }
    public void FadeOutLoading()
    {
        DataManager.LoadingAnimator.Play(DataManager.LoadingAnimations[1]);
    }
}
