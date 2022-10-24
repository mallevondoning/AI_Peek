using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
    [SerializeField]
    private Button _option;
    [SerializeField]
    private Color _disableColor;

    private void Awake()
    {
        _option.interactable = false;
        _option.GetComponentInChildren<TextMeshProUGUI>().color = _disableColor;
    }

    public void StartButton(string sceneName)
    {
        LoadScene(sceneName);
        LoadManager.Instance.LoadSceneFunc("HUD", true);
    }
    public void OptionButton(string sceneName)
    {
        LoadScene(sceneName);
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
