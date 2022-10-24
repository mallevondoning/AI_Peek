using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Vector3 modifiedPos;

    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        transform.position = _playerTransform.position + modifiedPos;
    }
}
