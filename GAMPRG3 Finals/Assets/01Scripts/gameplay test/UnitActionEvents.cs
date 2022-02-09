using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UnitActionEvents : MonoBehaviour
{
    UnityEvent GetMoveTiles;
    UnityEvent ResetTiles;

    void Start()
    {
        GetMoveTiles.AddListener(GetTiles);
    }

    public virtual void GetTiles()
    {

    }

    public virtual void RemoveTiles()
    {

    }
}
