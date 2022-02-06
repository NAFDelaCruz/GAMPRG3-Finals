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

        //NW N and NE Tiles
        SpawnTiles.Add(GameObject.Find(StartX - 1 + "" + (StartY + 1)));
        SpawnTiles.Add(GameObject.Find(StartX + "" + (StartY + 1)));
        SpawnTiles.Add(GameObject.Find(StartX + 1 + "" + (StartY + 1)));
        
        //W Player and E Tiles
        SpawnTiles.Add(GameObject.Find(StartX - 1 + "" + StartY));
        SpawnTiles.Add(GameObject.Find(StartX + "" + StartY));
        SpawnTiles.Add(GameObject.Find(StartX + 1 + "" + StartY));


        //SW S and SE Tiles
        SpawnTiles.Add(GameObject.Find(StartX - 1 + "" + (StartY - 1)));
        SpawnTiles.Add(GameObject.Find(StartX + "" + (StartY - 1)));
        SpawnTiles.Add(GameObject.Find(StartX + 1 + "" + (StartY - 1)));

        int TileIndex = 1;
        int UnitIndex = 0;
        
        foreach (GameObject Unit in TestPartyUnits)
        {
            if (SpawnTiles.Count <= TileIndex)
            {
                TileIndex = 1;
            }

            if (!SpawnTiles[TileIndex].GetComponent<Tiles>().IsObstacle)
            {
                Instantiate(TestPartyUnits[UnitIndex], SpawnTiles[TileIndex].gameObject.transform);
                SpawnTiles.Remove(SpawnTiles[TileIndex]);
                TileIndex++;
                UnitIndex++;
            }
            else if (SpawnTiles[TileIndex].GetComponent<Tiles>().IsObstacle && !SpawnTiles[0].GetComponent<Tiles>().IsObstacle)
            {
                SpawnTiles.Remove(SpawnTiles[TileIndex]);
                Instantiate(TestPartyUnits[UnitIndex], SpawnTiles[0].gameObject.transform);
                SpawnTiles.Remove(SpawnTiles[0]);
                UnitIndex++;
            }
            else if (SpawnTiles[0].GetComponent<Tiles>().IsObstacle)
            {
                SpawnTiles.Remove(SpawnTiles[0]);
                SpawnTiles.Remove(SpawnTiles[1]);
                Instantiate(TestPartyUnits[UnitIndex], SpawnTiles[TileIndex].gameObject.transform);
                SpawnTiles.Remove(SpawnTiles[TileIndex]);
                TileIndex++;
                UnitIndex++;
            }
        }
    }
}
