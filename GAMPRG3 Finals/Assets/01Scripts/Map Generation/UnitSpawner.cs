using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Set Components")]
    public GameObject TestUnit;
    public GameObject[] TestPartyUnits;
    int Count = 0;

    public void SpawnUnits(GameObject StartPoint, List<GameObject> Tiles)
    {
        Instantiate(TestUnit, StartPoint.transform);

        foreach (GameObject AvailableTile in Tiles)
        {
            if (AvailableTile.transform.childCount == 0 && Count < TestPartyUnits.Length)
            {
                Instantiate(TestPartyUnits[Count], AvailableTile.transform);
                Count++;
            }
        }
    }   
}
