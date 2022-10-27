using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : ISpyState
{
    List<PointAccses> pointList = new List<PointAccses>();

    int goToPoint = 0;
    float maxDistance = 0f;
    bool isStuck = false;
    bool wasMoving = false;


    Vector3 pointPos = Vector3.zero;

    public void Setup(EnemyController e) 
    {
        wasMoving = e.WasMoving;

        if (!e.WasMoving)
        {
            int loop = 0; // fail safe variable
            int randomPoint = 0; //<-- temp varibale for RNG regarding point

            //finds all point in the EnemyController scripts
            for (int i = 0; i < e.RaycastDirectionList.Count; i++)
            {
                for (int u = 0; u < e.RaycastDirectionList[i].Count; u++)
                {
                    var currentTemp = e.RaycastDirectionList[i][u].GetComponent<PointAccses>();

                    if (currentTemp != null)
                        pointList.Add(currentTemp);
                }
            }

            //fail safe if the spy get stuck 
            if (e.StuckCounter<10)
            {
                if (pointList.Count > 0)
                {
                    do
                    {
                        randomPoint = Random.Range(0, pointList.Count);

                        loop++;
                        if (loop > 1000)
                        {
                            Debug.LogError(e.name+" has no free points");
                            isStuck = true;
                            return;
                        }
                    } while (pointList[randomPoint].IsOccupied);
                    goToPoint = randomPoint; //<-- sets witch point is in focus
                }
                else
                {
                    isStuck = true;
                    return;
                }
            }
            else
            {
                Debug.LogError(e.name+" is in a corner with no free points or no point at all");
                isStuck = true;
                return;
            }

            if (e.CurrentPoint != null)
                e.CurrentPoint.IsNotOccupied();

            //sets the max distance to use in a lerp later
            pointPos = pointList[goToPoint].transform.position;
        }
        else
        {
            pointPos = e.CurrentPoint.transform.position;
        }

        pointPos.y = e.transform.position.y; //<-- sets so the y axis does not matter
        maxDistance = Vector3.Distance(e.transform.position, pointPos);

        //sets do this point is occupied
        if (!wasMoving)
            pointList[goToPoint].IsOccupiedFunc();
        else
            e.CurrentPoint.IsOccupiedFunc();

        e.WasMoving = false;
    }

    public void Tick(EnemyController e)
    {

        if (!isStuck)
        {

            //movment

            //sets the normalized value lerp
            float normDistance = Vector3.Distance(e.transform.position, pointPos) / maxDistance;

            //moving function 
            Vector3 moveToPoint = Vector3.MoveTowards(e.transform.position, pointPos, e.MoveSpeed * Time.deltaTime);

            //sets the position
            e.transform.position = moveToPoint;


            //rotation
            float normAngle = Vector3.Angle(e.transform.forward, pointPos) / 180f;
            float finalRotSpeed = Mathf.Lerp(e.RotSpeed * 0.5f, e.RotSpeed * 4f, normAngle);
            Vector3 targetDirection = pointPos - e.transform.position;

            Quaternion targetRotation;
            if (targetDirection != Vector3.zero)
                targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            else
                targetRotation = Quaternion.identity;

            Vector3 toRotateTowards = Quaternion.RotateTowards(e.transform.rotation, targetRotation, finalRotSpeed * Time.deltaTime).eulerAngles;
            toRotateTowards.x = 0f;

            e.transform.rotation = Quaternion.Euler(toRotateTowards);
            
            e.ConstantRaycastUpdate(targetDirection);
        }
    }

    public ISpyState Transition(EnemyController e)
    {
        if (isStuck)
        {
            Debug.LogWarning(e.name+" got stuck");
            e.StuckCounter++;
            return new Wait();
        }
        else if (e.CanSeePlayer != null)
        {
            //add code it know it should back to this state
            e.WasMoving = true;

            if (!wasMoving)
                e.CurrentPoint = pointList[goToPoint];

            e.GoBackPosition = e.CurrentPoint.transform.position;

            e.SetReactActive(true);
            return new ReactToPlayer();
        }
        else if ((wasMoving && e.transform.position.x == e.CurrentPoint.transform.position.x && e.transform.position.z == e.CurrentPoint.transform.position.z) || 
                (!wasMoving && e.transform.position.x == pointList[goToPoint].transform.position.x && e.transform.position.z == pointList[goToPoint].transform.position.z))
        {
            e.StuckCounter = 0;
            if (!wasMoving)
                e.CurrentPoint = pointList[goToPoint];

            int controlDir = Mathf.RoundToInt(e.transform.rotation.eulerAngles.y / 90f);
            Vector3 controlRot = e.transform.rotation.eulerAngles;
            e.transform.rotation = Quaternion.Euler(new Vector3(controlRot.x, controlDir * 90f, controlRot.z));

            e.UpdateRaycast();

            return new Wait();
        }
        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
