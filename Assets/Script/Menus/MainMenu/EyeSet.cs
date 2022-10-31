using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSet : MonoBehaviour
{
    [SerializeField]
    private GameObject _eyeLid;
    [SerializeField]
    private GameObject _eyes;

    [SerializeField]
    private int _maxTimerLowInSec;
    [SerializeField]
    private int _maxTimerHighInSec;
    [SerializeField]
    private float _lidSpeed;
    [SerializeField]
    private float _eyeSpeed;
    [SerializeField]
    float actoinTime;

    private float timer;
    private int maxTimer;
    private int classState;
    private int actionRandom;
    private int actionState;
    float waitTimer;

    float _openLidY;
    float _closedLidY;
    float _rightEyesX;
    float _leftEyesX;
    Vector3 _neutralEyesPos = Vector3.zero;

    private void Awake()
    {
        timer = 0f;
        classState = 0;
        actionRandom = -1;
        actionState = 0;
        waitTimer = 0f;

        _openLidY = 5f;
        _closedLidY = 4.4f;
        _rightEyesX = 0.25f;
        _leftEyesX = -0.25f;
    }

    private void Update()
    {
        switch (classState)
        {
            case 0:
                if (timer <= 0f)
                    maxTimer = GetRandomTimer();

                timer += Time.deltaTime;

                if (timer >= maxTimer)
                    classState++;
                break;
            case 1:
                if (actionRandom < 0)
                    actionRandom = Random.Range(0, 3);

                switch (actionRandom)
                {
                    case 0:
                        MoveEyeLid();
                        break;
                    case 1:
                        MoveEyesRight();
                        break;
                    case 2:
                        MoveEyesLeft();
                        break;
                    default:
                        Debug.LogError("This action does not exsit");
                        break;
                }
                break;
            default:
                Debug.LogError("the EyeSet got outside the bound");
                break;
        }

    }

    public int GetRandomTimer()
    {
        return Random.Range(_maxTimerLowInSec, _maxTimerHighInSec + 1);
    }

    public void MoveEyeLid()
    {
        switch (actionState)
        {
            case 0:
                _eyeLid.transform.localPosition = Vector3.MoveTowards(_eyeLid.transform.position, new Vector3(_eyeLid.transform.position.x, _closedLidY, _eyeLid.transform.position.z), _lidSpeed * Time.deltaTime);

                if (_eyeLid.transform.localPosition.y <= _closedLidY)
                    actionState++;
                break;
            case 1:
                actionState += WaitingTimer();
                break;
            case 2:
                _eyeLid.transform.localPosition = Vector3.MoveTowards(_eyeLid.transform.position, new Vector3(_eyeLid.transform.position.x, _openLidY, _eyeLid.transform.position.z), _lidSpeed * Time.deltaTime);

                if (_eyeLid.transform.localPosition.y >= _openLidY)
                    ResetState();
                break;
        }
    }
    public void MoveEyesRight()
    {
        switch (actionState)
        {
            case 0:
                _eyes.transform.localPosition = Vector3.MoveTowards(_eyes.transform.position, new Vector3(_rightEyesX, _eyes.transform.localPosition.y, _eyes.transform.localPosition.z), _eyeSpeed * Time.deltaTime);

                if (_eyes.transform.localPosition.x == _rightEyesX)
                    actionState++;
                break;
            case 1:
                actionState += WaitingTimer();
                break;
            case 2:
                _eyes.transform.localPosition = Vector3.MoveTowards(_eyes.transform.position, _neutralEyesPos, _eyeSpeed * Time.deltaTime);

                if (_eyes.transform.localPosition == _neutralEyesPos)
                    ResetState();
                break;
        }
    }
    public void MoveEyesLeft()
    {
        switch (actionState)
        {
            case 0:
                _eyes.transform.localPosition = Vector3.MoveTowards(_eyes.transform.position, new Vector3(_leftEyesX, _eyes.transform.localPosition.y, _eyes.transform.localPosition.z), _eyeSpeed * Time.deltaTime);

                if (_eyes.transform.localPosition.x == _leftEyesX)
                    actionState++;
                break;
            case 1:
                actionState += WaitingTimer();
                break;
            case 2:
                _eyes.transform.localPosition = Vector3.MoveTowards(_eyes.transform.position, _neutralEyesPos, _eyeSpeed * Time.deltaTime);

                if (_eyes.transform.localPosition == _neutralEyesPos)
                    ResetState();
                break;
        }
    }


    public int WaitingTimer()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer >= actoinTime)
            return 1;
        return 0;
    }
    public void ResetState()
    {
        timer = 0f;
        classState = 0;
        actionRandom = -1;
        actionState = 0;
        waitTimer = 0f;
    }
}
