using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpyState
{
    void Setup(EnemyController e);
    void Tick(EnemyController e);
    ISpyState Transition(EnemyController e) { return null; }
    void Exit();
}
