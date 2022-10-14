using KevinCastejon.ConeMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactToPlayer : ISpyState
{
    Image meter = null;

    float maxValue = 5f;
    float currentValue = 0f;
    float valueMultiplier = 1f;
    float normValue = 0f;

    public void Setup(EnemyController e)
    {
        meter = GameObject.Find("QuestionMarkMeter").GetComponent<Image>();
        maxValue = 5f;
    }

    public void Tick(EnemyController e)
    {
        if (e.CanSeePlayer != null)
        {
            float normDistance = Vector3.Distance(e.transform.position, e.CanSeePlayer.transform.position) / e.LookArea.GetComponent<Cone>().ConeHeight;
            valueMultiplier = Mathf.Lerp(2f, 0.25f, normDistance);
        }

        if (e.CanSeePlayer != null)
            currentValue += Time.deltaTime * valueMultiplier;
        else
            currentValue -= Time.deltaTime;

        currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
        normValue = currentValue / maxValue;

        meter.fillAmount = normValue;
    }

    public ISpyState Transition(EnemyController e)
    {
        if (normValue >= 1f)
        {

        }
        else if (normValue >= 0.5f && e.CanSeePlayer == null)
        {

        }
        else if (normValue <= 0f && e.CanSeePlayer == null)
        {

        }
        return null;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
