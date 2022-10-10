using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickConvertion : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick _joystick;

    private void Update()
    {
        InputManager.SetJoyStickDirection(_joystick.Direction);
    }
}
