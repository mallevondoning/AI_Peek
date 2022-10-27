using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeWall : MonoBehaviour
{
    private BoxCollider _wallCollider;
    private Transform[] _transformList;
    private Transform _artTransform;

    private void Awake()
    {
        _wallCollider = GetComponent<BoxCollider>();

        _transformList = GetComponentsInChildren<Transform>();
        for (int i = 0; i < _transformList.Length; i++)
        {
            if (_transformList[i] != transform)
            {
                _artTransform = _transformList[i];
                break;
            }
        }
    }

    private void Update()
    {
        _artTransform.localScale = _wallCollider.size;
    }
}
