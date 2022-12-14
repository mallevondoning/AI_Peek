using KevinCastejon.ConeMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactToPlayer : ISpyState
{
    Image meter = null;

    float maxValue = 0f;
    float currentValue = 0f;
    float valueMultiplier = 1f;
    float normValue = 0f;

    public void Setup(EnemyController e)
    {
        meter = GameObject.Find("QuestionMarkMeter").GetComponent<Image>();
        maxValue = e.MaxSusicionLevel;
    }

    public void Tick(EnemyController e)
    {
        if (e.CanSeePlayer != null)
        {
            float normDistance = Vector3.Distance(e.transform.position, e.CanSeePlayer.transform.position) / e.LookTrigger.height;
            valueMultiplier = Mathf.LerpUnclamped(e.HighestPoint, e.LowestPoint, normDistance);
        }

        if (e.CanSeePlayer != null)
            currentValue += e.TickTimer * valueMultiplier;
        else
            currentValue -= e.TickTimer;

        currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
        normValue = currentValue / maxValue;

        meter.fillAmount = normValue;
    }

    public ISpyState Transition(EnemyController e)
    {
        if (normValue >= 1f)
        {
            e.SetChaseActive(true);
            e.SetReactActive(false);

            if (e.WasMoving)
                e.GoBackPosition = e.CurrentPoint.transform.position;
            else
                e.GoBackPosition = e.transform.position;

            return new ChasePlayer();
        }
        else if (normValue >= 0.5f && e.CanSeePlayer == null)
        {
            e.SetLookActive(true);
            e.SetReactActive(false);

            if (e.WasMoving)
                e.GoBackPosition = e.CurrentPoint.transform.position;
            else
                e.GoBackPosition = e.transform.position;

            return new LookAround();
        }
        else if (normValue <= 0f && e.CanSeePlayer == null)
        {
            e.SetReactActive(false);
            e.SetChaseActive(false);
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
