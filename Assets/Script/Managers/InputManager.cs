using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    private static float _dirX;
    public static float DirX { get { return _dirX; } 
                               private set { value = _dirX; } }

    private static float _dirY;
    public static float DirY { get { return _dirY; } 
                               private set { value = _dirY; } }


    private static float keyboardXPrivate;
    private static float keyboardYPrivate;

    private static float joystickXPrivate;
    private static float joystickYPrivate;

    public static void SetKeyboardDirection(float keyboardX, float keyboardY)
    {
        keyboardXPrivate = keyboardX;
        keyboardYPrivate = keyboardY;
    }
    public static void SetKeyboardDirection(Vector2 keyboardDirection)
    {
        keyboardXPrivate = keyboardDirection.x;
        keyboardYPrivate = keyboardDirection.y;
    }

    public static void SetJoyStickDirection(float joystickX, float joystickY)
    {
        joystickXPrivate = joystickX;
        joystickYPrivate = joystickY;
    }
    public static void SetJoyStickDirection(Vector2 joystickDirection)
    {
        joystickXPrivate = joystickDirection.x;
        joystickYPrivate = joystickDirection.y;
    }

    public static Vector3 FinalDirection()
    {
        _dirX = keyboardXPrivate + joystickXPrivate;
        _dirY = keyboardYPrivate + joystickYPrivate;

        _dirX = Mathf.Clamp(_dirX,-1f,1f);
        _dirY = Mathf.Clamp(_dirY,-1f,1f);

        return new Vector3(DirX,0,DirY).normalized;
    }
}
    