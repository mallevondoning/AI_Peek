using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private float _speed = 500f;
    [SerializeField]
    private float _rotSpeed = 180f;

    private Locomotion _locomotion = new Locomotion();

    private void Awake()
    {
        _locomotion.Setup(transform, _rotSpeed, _speed, _rigidbody);
    }

    private void FixedUpdate()
    {
        Vector2 inputDir = Vector2.zero;
        inputDir.y = Input.GetAxis("Vertical");
        inputDir.x = Input.GetAxis("Horizontal");
        InputManager.SetKeyboardDirection(inputDir);

        _locomotion.Move(InputManager.FinalDirection());
    }
}
