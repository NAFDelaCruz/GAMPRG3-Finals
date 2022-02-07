using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject TestUnit;
    public GameObject Map;
    public GameObject[] TestPartyUnits;
    public List<GameObject> SpawnTiles;
    public Collider2D[] Tiles;

    GenerateMap generateMap;

    void Start()
    {
        generateMap = Map.GetComponent<GenerateMap>();
        Instantiate(TestUnit, generateMap.StartPoint.gameObject.transform);

        Physics2D.SyncTransforms();
        Tiles = Physics2D.OverlapBoxAll(generateMap.StartPoint.transform.position, new Vector2(1.5f, 1.5f), 0f);
        
        foreach (Collider2D tile in Tiles)
        {
            SpawnTiles.Add(tile.gameObject);
        }

        Sort();

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

    void Sort()
    {
        SpawnTiles.Sort(SortByCoord);
    }

    static int SortByCoord(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }
}
