using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnd : MonoBehaviour
{
    public GameObject Map;
    GenerateMap generateMap;

    int x_border;
    int y_border;

    public GameObject StartPoint;
    public GameObject EndPoint;

    public bool test;
    void Start()
    {
        generateMap = Map.GetComponent<GenerateMap>();
        x_border = generateMap.MapWidth;
        y_border = generateMap.MapHeight;
        LocateValidTile();
        LocateValidTile();
    }

    void LocateValidTile()
    {
        int rand_x = Random.Range(0, x_border);
        int rand_y = Random.Range(0, y_border);
        string tileName = rand_x.ToString() + rand_y.ToString();
        //Debug.Log(tileName);
        GetTileObject(tileName);
    }

    void GetTileObject(string tileID) 
    {
        Debug.Log(tileID);
        GameObject targetTile = Map.transform.Find(tileID).gameObject;
        if (StartPoint == null)
        {
            StartPoint = targetTile;
        }
        if(StartPoint != null)
        {
            EndPoint = targetTile;
        }
        //if (targetTile.GetComponent<Tiles>().Obstacle == true)
        //{
        //    LocateValidTile();
        //}
        //else
        //{

        //}
    }

    //void HighlightTile()
}
