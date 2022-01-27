using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Special thanks to Danndx 2021 (youtube.com/danndx) for authoring the base code
//From video: youtu.be/qNZ-0-7WuS8

//This script has been heavily modified by Niel Angelo F. Dela Cruz for the purpose of the game

public class GenerateMap : MonoBehaviour
{
    [Header("Default Values")]
    public GameObject VoidTilePrefab;

    //For each set create these
    [Header("CaveTiles")]
    [Tooltip("Set chances from highest to lowest.")]
    public List<float> TerrainPrefabsChanceValues;
    [Tooltip("Set in order of chances of float values.")]
    public List<GameObject> TerrainPrefabs;
    //Name List names according to set name

    [Header("Map Variables")]
    public int MapWidth;
    public int MapHeight;

    [Tooltip("Controls how detailed the map is. Recommended value: 4-20")]
    public float Magnification;

    [Tooltip("Shifts tiles to the left (-) or right (+).")]
    public int XOffset = 0; // <- +>
    [Tooltip("Shifts tiles down (-) or up (+).")]
    public int YOffset = 0; // v- +^

    [Header("Start and End Points")]
    public GameObject StartPoint;
    public GameObject EndPoint;
    string tileName;

    [Header("Map Void Variables")]
    [Tooltip("Recommened Values: 0-4.")]
    public int VoidPerlinOctaves;
    [Tooltip("Recommened Values: 0-2.")]
    public float VoidLacunarity;
    [Tooltip("Scale with Void Octaves, 4:100")]
    public float VoidPerlinMagnification;
    [Tooltip("Recommened Values: 0-3.5, the lower the value the lesser likely isolated islands spawn.")]
    public float VoidPerlinSelector;
    [Tooltip("Shifts tiles to the left (-) or right (+).")]
    public int VoidPerlinXOffset;
    [Tooltip("Shifts tiles down (-) or up (+).")]
    public int VoidPerlinYOffset;
    
    List<List<float>> PrefabChances = new List<List<float>>();
    List<List<GameObject>> TilePrefabs = new List<List<GameObject>>();
    List<Dictionary<float, GameObject>> TileSets = new List<Dictionary<float, GameObject>>();
    GameObject TilePrefab;
    void Start()
    {
        //Each set for a list add here
        PrefabChances.Add(TerrainPrefabsChanceValues);
        TilePrefabs.Add(TerrainPrefabs);

        CreateTileSet();
        Generate();
    }

    void CreateTileSet()
    {
        int SetNumber = 0;

        foreach (List<GameObject> Sets in TilePrefabs)
        {
            Dictionary<float, GameObject> PairTemp = new Dictionary<float, GameObject>();
            int ChanceIndex = 0;

            foreach (GameObject Tile in Sets)
            {
                PairTemp.Add(PrefabChances[SetNumber][ChanceIndex], Tile);
                ChanceIndex++;
            }

            TileSets.Add(PairTemp);
            SetNumber++;
        }
    }

    void Generate()
    {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/

        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                int TileSetID = GetIdUsingPerlin(x, y);
                Dictionary<float, GameObject> SelectedSet = TileSets[TileSetID];
                float TotalWeight = GetTotalWeight(SelectedSet);
                GameObject TilePrefab = SelectTile(SelectedSet, TotalWeight);
                CreateTile(TilePrefab, x, y);
            }
        }
        LocateValidTile(0);
        LocateValidTile(1);     
    }

    int GetIdUsingPerlin(int x, int y)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/

        float RawPerlin = Mathf.PerlinNoise(
            (x - YOffset) / Magnification,
            (y - YOffset) / Magnification
        );

        float ClampPerlin = Mathf.Clamp01(RawPerlin); // Thanks: youtu.be/qNZ-0-7WuS8&lc=UgyoLWkYZxyp1nNc4f94AaABAg
        float ScaledPerlin = Mathf.Clamp(ClampPerlin * TileSets.Count, 0, TileSets.Count - 1);

        return Mathf.FloorToInt(ScaledPerlin);
    }

    void CreateTile(GameObject TilePrefab, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/
        float VoidPerlin = new float();
        float Frequency = 1;

        for (int Index = 0; Index < VoidPerlinOctaves; Index++)
        {
            float X = (x - VoidPerlinXOffset) / VoidPerlinMagnification * Frequency;
            float Y = (y - VoidPerlinXOffset) / VoidPerlinMagnification * Frequency;

            VoidPerlin = Mathf.PerlinNoise(X, Y);

            Frequency *= VoidLacunarity;
        }

        if (VoidPerlin > VoidPerlinSelector)
        {
            GameObject Tile = Instantiate(TilePrefab, gameObject.transform);
            Tile.name = string.Format("{0}{1}", x, y);
            Tile.transform.localPosition = new Vector3(x, y, 0);
        }
        else
        {
            GameObject Tile = Instantiate(VoidTilePrefab, gameObject.transform);
            Tile.name = string.Format("{0}{1}", x, y);
            Tile.transform.localPosition = new Vector3(x, y, 0);
        }
    }

    GameObject SelectTile(Dictionary<float, GameObject> SelectedSet, float TotalWeight)
    {
        float Key = new float();
        float RandomValue = Random.Range(0, TotalWeight);

        foreach (float Chances in SelectedSet.Keys)
        {
            if (RandomValue <= Chances)
            {
                Key = Chances;
                break;
            }
            else
            {
                RandomValue -= Chances;
            }
        }

        return SelectedSet[Key];
    }

    float GetTotalWeight(Dictionary<float, GameObject> SelectedSet)
    {
        float TotalWeight = new float();

        foreach (float Chances in SelectedSet.Keys)
        {
            TotalWeight += Chances;
        }

        return TotalWeight;
    }

    void LocateValidTile(int selector)
    {
        int rand_x = Random.Range(0, MapWidth);
        int rand_y = Random.Range(0, MapHeight);
        tileName = rand_x.ToString() + rand_y.ToString();

        GameObject targetTile = GameObject.Find(tileName).gameObject;
        if (targetTile.GetComponent<Tiles>().Obstacle == true)
        {
            Debug.Log("Target tile is invalid selecting a new tile");
            LocateValidTile(selector);
        }

        //Debug.Log(tileName);
        GetTileObject(tileName, selector);
    }

    void GetTileObject(string tileName, int selector)
    {
        Debug.Log(tileName);
        GameObject targetTile = GameObject.Find(tileName).gameObject;
        if (selector == 0)
        {
            Debug.Log("Start Point");
            StartPoint = targetTile;
            targetTile.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (selector == 1)
        {
            Debug.Log("End Point");
            EndPoint = targetTile;
            targetTile.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}