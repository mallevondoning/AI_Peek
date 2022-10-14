using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSouth : ISpyState
{
    public void Setup(EnemyController e) { }

    public void Tick(EnemyController e)
    {
        float normAngle = Vector3.Angle(e.transform.forward, Vector3.back) / 180f;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        float finalRotSpeed = Mathf.Lerp(e.RotSpeed * 0.5f, e.RotSpeed * 4f, normAngle);

        e.transform.rotation = Quaternion.RotateTowards(e.transform.rotation, targetRotation, finalRotSpeed * Time.deltaTime);
    }

    public ISpyState Transition(EnemyController e)
    {
        if (e.transform.forward == Vector3.back)
            return new Wait();
        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
