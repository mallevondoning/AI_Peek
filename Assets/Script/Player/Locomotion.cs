using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion
{
    private Vector3 _dir;
    public Vector3 Dir { get { return _dir; } }

    private float _rotSpeed;
    private float _speed;
    private Transform _transform;
    private Transform _artTransform;
    private Rigidbody _rigidbody;

    public void Setup(Transform transform,Transform artTransform, float rotSpeed, float speed, Rigidbody rigidbody)
    {
        _transform = transform;
        _artTransform = artTransform;
        _rotSpeed = rotSpeed;
        _speed = speed;
        _rigidbody = rigidbody;
    }

    public void Move(Vector3 normalizedTargetDir)
    {
        //get a normalzied angle between the current forward dirction and the input direction
        float normalziedAngle = Vector3.Angle(_artTransform.forward, normalizedTargetDir) / 180f;

        //check that we have any input from the player before we rotate
        if (normalizedTargetDir.magnitude > 0f)
        {
            //rotate the player
            Quaternion targetRotation = Quaternion.LookRotation(normalizedTargetDir, Vector3.up);
            float rotSpeed = Mathf.Lerp(_rotSpeed * 0.5f, 720f, normalziedAngle);
            _artTransform.rotation = Quaternion.RotateTowards(_artTransform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
        }

        float finalSpeed = Mathf.Lerp(_speed, _speed * 0.25f, normalziedAngle);
        normalizedTargetDir *= finalSpeed * Time.fixedDeltaTime;

        //set where the velocity of the object should be
        _dir.x = normalizedTargetDir.x;
        _dir.y = _rigidbody.velocity.y;
        _dir.z = normalizedTargetDir.z;

        _rigidbody.velocity = Dir;
    }
}
