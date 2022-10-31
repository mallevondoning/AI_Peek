using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInputField : MonoBehaviour
{
    InputField testField;

    private void Update()
    {
        testField.text.ToLower();

        switch (testField.text)
        {
            case "help":
                break;
            default:
                break;
        }
    }
}
