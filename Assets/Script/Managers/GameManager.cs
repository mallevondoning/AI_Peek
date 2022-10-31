using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<string> LevelList = new List<string>();

    private void Awake()
    {
        Instance = this;
    }
}
