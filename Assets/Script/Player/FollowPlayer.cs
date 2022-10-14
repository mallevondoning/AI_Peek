using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        transform.position = _playerTransform.position + new Vector3(0, 8, -4);
    }
}
