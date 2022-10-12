using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ISpyState
{
    private int timeToWait = 5;
    private float timeWaited;

    public void Setup()
    {
        timeToWait = Random.Range(5, 11);
        Debug.Log(timeToWait);
    }

    public void Tick(EnemyController e)
    {
        timeWaited += e.TickTimer;
    }

    public ISpyState Transition(EnemyController e)
    {
        if (timeWaited >= timeToWait)
        {
            int rotationRNG = Random.Range(0, 4);
            switch (rotationRNG)
            {
                case 0:
                    if (e.transform.forward != Vector3.forward)
                        return new TurnNorth();
                    else
                        break;
                case 1:
                    if (e.transform.forward != Vector3.right)
                        return new TurnEast();
                    else
                        break;
                case 2:
                    if (e.transform.forward != Vector3.back)
                        return new TurnSouth();
                    else
                        break;
                case 3:
                    if (e.transform.forward != Vector3.left)
                        return new TurnWest();
                    else
                        break;
                default:
                    Debug.Log("Outside random range");
                    break;
            }
        }

        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
