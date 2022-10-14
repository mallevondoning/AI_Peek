using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAccses : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    public void IsOccupiedFunc()
    {
        IsOccupied = true;
    }

    public void IsNotOccupied()
    {
        IsOccupied = false;
    }

    public void switchOccupationState()
    {
        IsOccupied = !IsOccupied;
    }
}
