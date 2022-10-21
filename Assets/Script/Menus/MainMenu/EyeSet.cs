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

    Vector3 _openLidPos = Vector3.zero;
    Vector3 _closedLidPos = Vector3.zero;
    Vector3 _neutralEyesPos = Vector3.zero;
    Vector3 _rightEyesPos = Vector3.zero;
    Vector3 _leftEyesPos = Vector3.zero;

    private void Awake()
    {
        timer = 0f;
        classState = 0;
        actionRandom = -1;
        actionState = 0;
        waitTimer = 0f;

        _openLidPos = new Vector3(2f, 5f, -2f);
        _closedLidPos = new Vector3(2f, 4.4f, -2f);
        _rightEyesPos = new Vector3(0.35f, 0f, 0f);
        _leftEyesPos = new Vector3(-0.25f, 0f, 0f);
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
                _eyeLid.transform.localPosition = Vector3.MoveTowards(_eyeLid.transform.position, _closedLidPos, _lidSpeed * Time.deltaTime);

                if (_eyeLid.transform.localPosition == _closedLidPos)
                    actionState++;
                break;
            case 1:
                actionState += WaitingTimer();
                break;
            case 2:
                _eyeLid.transform.localPosition = Vector3.MoveTowards(_eyeLid.transform.position, _openLidPos, _lidSpeed * Time.deltaTime);

                if (_eyeLid.transform.localPosition == _openLidPos)
                    ResetState();
                break;
        }
    }
    public void MoveEyesRight()
    {
        switch (actionState)
        {
            case 0:
                _eyes.transform.localPosition = Vector3.MoveTowards(_eyes.transform.position, _rightEyesPos, _eyeSpeed * Time.deltaTime);

                if (_eyes.transform.localPosition == _rightEyesPos)
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
                _eyes.transform.localPosition = Vector3.MoveTowards(_eyes.transform.position, _leftEyesPos, _eyeSpeed * Time.deltaTime);

                if (_eyes.transform.localPosition == _leftEyesPos)
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
