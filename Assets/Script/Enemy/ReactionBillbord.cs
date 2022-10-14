using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionBillbord : MonoBehaviour
{
    [SerializeField]
    private Transform _camTransform;

    private void Awake()
    {
        _camTransform = FindObjectOfType<FollowPlayer>().transform;
    }

    void Update()
    {
        Vector3 cameraDirection = transform.position - _camTransform.position;

        transform.forward = cameraDirection;
    }
}
