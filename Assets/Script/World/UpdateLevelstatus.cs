using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLevelstatus : MonoBehaviour
{
    [SerializeField]
    private int _updatedLevelStatus;

    public void SetLevelStatus(int toLevel)
    {
        DataManager.Level = toLevel;

        if (DataManager.Level <= GameManager.Instance.LevelList.Count - 1)
            LoadManager.Instance.LoadSceneFunc(GameManager.Instance.LevelList[DataManager.Level]);
        else
            Debug.LogError("Update level status is outside of the bounds");
    }

    private void OnValidate()
    {
        if (_updatedLevelStatus < 0)
            _updatedLevelStatus = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerCheck = other.GetComponent<PlayerController>();

        if (playerCheck != null)
            SetLevelStatus(_updatedLevelStatus);
    }
}
