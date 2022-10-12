using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float TickTimer { get; private set; }
    public float RotSpeed { get; private set; }

    private ISpyState _enemyState;

    [SerializeField]
    private TestStates db_testStates;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        switch (db_testStates)
        {
            case TestStates.wait:
                _enemyState = new Wait();
                break;
            case TestStates.north:
                _enemyState = new TurnNorth();
                break;
            case TestStates.south:
                _enemyState = new TurnSouth();
                break;
            case TestStates.east:
                _enemyState = new TurnEast();
                break;
            case TestStates.west:
                _enemyState = new TurnWest();
                break;
            default:
                _enemyState = null;
                break;
        }

        RotSpeed = 180f;
        TickTimer = Time.deltaTime;
    }

    void Tick()
    {
        _enemyState.Tick(this);

        ISpyState newState = _enemyState.Transition(this);
        if (newState != null) 
        { 
            _enemyState = newState;
            _enemyState.Setup();
        }
    }

    IEnumerator TickLoop()
    {
        Tick();

        yield return new WaitForSeconds(TickTimer);
    }

    void Update()
    {
        StartCoroutine(TickLoop());
    }
}

enum TestStates
{
    NoneID = int.MaxValue,
    wait = 0,
    north = 1,
    south = 2,
    east = 3,
    west = 4,
}
