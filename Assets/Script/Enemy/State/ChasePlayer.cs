using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : ISpyState
{

    public void Setup(EnemyController e)
    {
        throw new System.NotImplementedException();
    }

    public void Tick(EnemyController e)
    {
        throw new System.NotImplementedException();
    }

    public ISpyState Transition(EnemyController e)
    {
        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
