using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChasePlayer : ISpyState
{
    int stateState = 0;
    float waitTimer = 0f;
    float maxWaitTimer = 0f;
    float cantSeeTimer = 0f;
    float maxCantSeeTimer = 0f;
    float rotSpeedMultiplier = 0f;
    //float speedMultiplier = 0f; <-- add if the chase speed of the enemies speed is to slow or fast

    public void Setup(EnemyController e)
    {
        stateState = 0;
        maxWaitTimer = 1f;
        maxCantSeeTimer = 2f;
        rotSpeedMultiplier = 0.5f;
    }

    public void Tick(EnemyController e)
    {
        switch (stateState)
        {
            case 0:
                waitTimer += Time.deltaTime;

                if (waitTimer >= maxWaitTimer)
                    stateState++;
                break;
            case 1:
                Vector3 playerPos;

                //sets position where the point is
                if (e.CanSeePlayer == null)
                {
                    PlayerController player = null;
                    Scene activeScene = SceneManager.GetActiveScene();
                    GameObject[] allObjectInScene = activeScene.GetRootGameObjects();

                    for (int i = 0; i < allObjectInScene.Length; i++)
                    {
                        PlayerController tempPlayer = allObjectInScene[i].GetComponent<PlayerController>();

                        if (tempPlayer != null)
                        {
                            player = tempPlayer;
                            break;
                        }
                    }

                    playerPos = player.transform.position;

                    cantSeeTimer += Time.deltaTime;
                }
                else
                {
                    playerPos = e.CanSeePlayer.transform.position;

                    cantSeeTimer -= Time.deltaTime;
                }

                //moving function 
                Vector3 moveToPoint = Vector3.MoveTowards(e.transform.position, playerPos, e.MoveSpeed * Time.deltaTime);

                //sets the position
                e.transform.position = moveToPoint;


                //rotation
                float normAngle = Vector3.Angle(e.transform.forward, playerPos) / 180f;
                float finalRotSpeed = Mathf.Lerp(e.RotSpeed * 0.5f, e.RotSpeed * 4f, normAngle);
                Vector3 targetDirection = playerPos - e.transform.position;

                Quaternion targetRotation;
                if (targetDirection != Vector3.zero)
                    targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                else
                    targetRotation = Quaternion.identity;

                Vector3 toRotateTowards = Quaternion.RotateTowards(e.transform.rotation, targetRotation, finalRotSpeed * rotSpeedMultiplier * Time.deltaTime).eulerAngles;
                toRotateTowards.x = 0f;

                e.transform.rotation = Quaternion.Euler(toRotateTowards);

                cantSeeTimer = Mathf.Clamp(cantSeeTimer, 0, maxCantSeeTimer);

                if (cantSeeTimer >= maxCantSeeTimer)
                    stateState++;
                break;
            case 2:
                Debug.LogError(e.name + " didn't transition correctly from chase player");
                break;
            default:
                Debug.LogError(e.name + " is outside of the bounds in the chase state");
                break;
        }

    }

    public ISpyState Transition(EnemyController e)
    {
        if (cantSeeTimer >= maxCantSeeTimer)
        {
            e.SetChaseActive(false);
            e.SetLookActive(true);

            return new LookAround();
        }
        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
