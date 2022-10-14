using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ISpyState
{
    List<int> rotationIndexList = new List<int>();

    private int timeToWait = 5;
    private float timeWaited = 0f;
    private bool hasMoved = true;

    public void Setup(EnemyController e)
    {
        timeToWait = Random.Range(5, 11);
        hasMoved = e.HasMoved;

        for (int i = 0; i < e.RaycastDirectionList.Count; i++)
        {
            if (e.RaycastDirectionList[i].Count > 0)
                rotationIndexList.Add(i);
        }
    }

    public void Tick(EnemyController e)
    {
        timeWaited += e.TickTimer;
    }

    public ISpyState Transition(EnemyController e)
    {
        if (e.CanSeePlayer != null)
        {
            return new ReactToPlayer();
        }
        else if (timeWaited >= timeToWait)
        {
            int isMovingRNG = Random.Range(0, 2);

            //<problem> think this is borken, works for now//
            int rotationIndexRNG = Random.Range(0, rotationIndexList.Count);
            int rotateTo = rotationIndexList[rotationIndexRNG];
            //</problem>//

            if (hasMoved)
            {
                e.UpdateRaycast();
                e.HasMoved = false;
            }

            switch (isMovingRNG)
            {
                case 0:
                    e.HasMoved = true;
                    return new MoveToPoint();
                case 1:
                    switch (rotateTo)
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
                            Debug.Log("rotationRNG outside random range");
                            break;
                    }
                    break;
                default:
                    Debug.Log("isMovingRNG outside random range");
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
