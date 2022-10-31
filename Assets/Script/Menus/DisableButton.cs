using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisableButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _button.interactable = false;
        _text.color = _button.colors.disabledColor;
    }
}
