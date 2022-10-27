using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LookAround : ISpyState
{
    int stateState = 0;
    int pongState = 0;
    float waitTimer = 0f;
    float maxTimer = 0f;
    Vector3 lastSeenplayer;

    float normAngle = 0f;
    float finalRotSpeed = 0f;
    Vector3 targetDirection = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;
    Vector3 toRotateTowards = Vector3.zero;

    Vector3 neutralAngle = Vector3.zero;
    Vector3 turnAngle = Vector3.zero;

    public void Setup(EnemyController e)
    {
        if (e.CanSeePlayer != null)
            lastSeenplayer = e.CanSeePlayer.transform.position;
        else
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

            lastSeenplayer = player.transform.position;
        }

        stateState = 0;
        pongState = 0;
        waitTimer = 0f;
        maxTimer = 2f;

        turnAngle = new Vector3(0, 45f, 0);
    }

    public void Tick(EnemyController e)
    {
        switch (stateState)
        {
            case 0: //<-- wait for maxTimer time
                waitTimer += Time.deltaTime;

                if (waitTimer >= maxTimer)
                {
                    waitTimer = 0;
                    stateState++;
                }
                break;
            case 1: //<-- move towards where the enemy last seen the player
                e.transform.position = Vector3.MoveTowards(e.transform.position, lastSeenplayer, e.MoveSpeed * Time.deltaTime);

                //rotation
                normAngle = Vector3.Angle(e.transform.forward, lastSeenplayer) / 180f;
                finalRotSpeed = Mathf.Lerp(e.RotSpeed * 0.5f, e.RotSpeed * 4f, normAngle);
                targetDirection = lastSeenplayer - e.transform.position;

                if (targetDirection != Vector3.zero)
                    targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                else
                    targetRotation = Quaternion.identity;

                toRotateTowards = Quaternion.RotateTowards(e.transform.rotation, targetRotation, finalRotSpeed * Time.deltaTime).eulerAngles;
                toRotateTowards.x = 0f;

                e.transform.rotation = Quaternion.Euler(toRotateTowards);

                if (e.transform.position == lastSeenplayer)
                {
                    neutralAngle = e.transform.rotation.eulerAngles;
                    stateState++;
                }
                break;
            case 2:
                //<problem> does not turn corectly
                if (pongState % 2 == 0)
                    e.transform.rotation = Quaternion.RotateTowards(e.transform.rotation, Quaternion.Euler(neutralAngle + turnAngle), e.RotSpeed * Time.deltaTime);
                else
                    e.transform.rotation = Quaternion.RotateTowards(e.transform.rotation, Quaternion.Euler(neutralAngle - turnAngle), e.RotSpeed * Time.deltaTime);

                if (e.transform.rotation.eulerAngles == neutralAngle + turnAngle || e.transform.rotation.eulerAngles == neutralAngle - turnAngle)
                    pongState++;

                if (pongState > 4)
                    e.transform.rotation = Quaternion.RotateTowards(e.transform.rotation, Quaternion.Euler(neutralAngle), e.RotSpeed * Time.deltaTime);

                if (e.transform.rotation.eulerAngles == neutralAngle && pongState > 4)
                    stateState++;
                //</problem>
                break;
            case 3: //<-- wait for maxTimer time
                waitTimer += Time.deltaTime;

                if (waitTimer >= maxTimer)
                {
                    waitTimer = 0;
                    stateState++;
                }
                break;
            case 4: //<-- move back to orginal pla
                e.transform.position = Vector3.MoveTowards(e.transform.position, e.GoBackPosition, e.MoveSpeed * Time.deltaTime);

                //rotation
                normAngle = Vector3.Angle(e.transform.forward, e.GoBackPosition) / 180f;
                finalRotSpeed = Mathf.Lerp(e.RotSpeed * 0.5f, e.RotSpeed * 4f, normAngle);
                targetDirection = e.GoBackPosition - e.transform.position;

                if (targetDirection != Vector3.zero)
                    targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                else
                    targetRotation = Quaternion.identity;

                toRotateTowards = Quaternion.RotateTowards(e.transform.rotation, targetRotation, finalRotSpeed * Time.deltaTime).eulerAngles;
                toRotateTowards.x = 0f;

                e.transform.rotation = Quaternion.Euler(toRotateTowards);

                if (e.transform.position == e.GoBackPosition)
                    stateState++;
                break;
            case 5:
                Debug.LogError(e.name + " didn't transition correctly from look around");
                break;
            default:
                Debug.LogError(e.name +" is outside of the bounds in the look state");
                break;
        }
    }

    public ISpyState Transition(EnemyController e) { 
        if (e.CanSeePlayer != null)
        {
            e.SetChaseActive(true);
            e.SetLookActive(false);

            return new ChasePlayer();
        }
        else if (stateState == 5)
        {
            e.SetLookActive(false);

            if (e.WasMoving)
                return new MoveToPoint();
            else
                return new Wait();
        }
        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
