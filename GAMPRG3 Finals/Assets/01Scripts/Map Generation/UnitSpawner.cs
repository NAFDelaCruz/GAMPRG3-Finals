using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject TestUnit;
    public GameObject Map;
    public GameObject[] TestPartyUnits;
    public List<GameObject> SpawnTiles;

    GenerateMap generateMap;

    void Start()
    {
        generateMap = Map.GetComponent<GenerateMap>();
        Instantiate(TestUnit, generateMap.StartPoint.gameObject.transform);

        int StartX = generateMap.StartPoint.GetComponent<Tiles>().XCoordinate;
        int StartY = generateMap.StartPoint.GetComponent<Tiles>().YCoordinate;

        SpawnTiles.Add(GameObject.Find(StartX + "" + (StartY + 1)));
        SpawnTiles.Add(GameObject.Find(StartX + "" + (StartY - 1)));
        SpawnTiles.Add(GameObject.Find((StartX - 1) + "" + StartY));
        SpawnTiles.Add(GameObject.Find((StartX + 1) + "" + StartY));

        int Index = 0;
        foreach (GameObject Unit in TestPartyUnits)
        {
            Instantiate(TestPartyUnits[Index], SpawnTiles[Index].gameObject.transform);
            Index++;
        }
    }
}
