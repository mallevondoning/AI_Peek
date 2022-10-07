using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Awake()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            Debug.Log("Forward");
        if (Input.GetKey(KeyCode.D))
            Debug.Log("Right");
        if (Input.GetKey(KeyCode.A))
            Debug.Log("Left");
        if (Input.GetKey(KeyCode.S))
            Debug.Log("Back");
    }
}
