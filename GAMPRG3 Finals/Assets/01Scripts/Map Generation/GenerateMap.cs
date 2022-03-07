using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Special thanks to Danndx 2021 (youtube.com/danndx) for authoring the base code
//From video: youtu.be/qNZ-0-7WuS8

//This script has been heavily modified by Niel Angelo F. Dela Cruz for the purpose of the game

public class GenerateMap : MonoBehaviour
{

    [Header("Set Components")]
    UnitSpawner UnitSpawnerScript;
    EnemySpawner EnemySpawnerScript;

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
    [HideInInspector]
    public int MapWidth;
    [HideInInspector]
    public int MapHeight;
    [Tooltip("Controls how detailed the map is. Recommended value: 4-20")]
    public float Magnification;
    [Tooltip("Shifts tiles to the left (-) or right (+).")]
    public int XOffset = 0; // <- +>
    [Tooltip("Shifts tiles down (-) or up (+).")]
    public int YOffset = 0; // v- +^
    float DefaultSize = 20;
    float SizeScaleFactor = 1.1f;
    float VariationFactor = 0.25f;
    float ScaledSize;
    float Variation;

    [Header("Start and End Points")]
    [HideInInspector]
    public Collider2D[] Tiles;
    public List<GameObject> ValidTiles;
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
        UnitSpawnerScript = GetComponent<UnitSpawner>();
        EnemySpawnerScript = GetComponent<EnemySpawner>();
        PrefabChances.Add(TerrainPrefabsChanceValues);
        TilePrefabs.Add(TerrainPrefabs);

        CreateTileSet();
    }
    
    public void GenerateDimensions(int Difficulty)
    {
        ScaledSize = Mathf.FloorToInt(DefaultSize + (Difficulty * SizeScaleFactor) - (0.5f * Difficulty));
        RandomizeDimension(ScaledSize);
    }

    public void RandomizeDimension(float scaledSize)
    {
        Variation = Mathf.FloorToInt((scaledSize * VariationFactor) / 2);
        MapHeight = Mathf.FloorToInt((Random.Range(scaledSize - Variation, scaledSize + Variation)));
        MapWidth = Mathf.FloorToInt((Random.Range(scaledSize - Variation, scaledSize + Variation)));
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
            
            if (x == MapWidth - 1)
            {
                Physics2D.SyncTransforms();
                LocateStart();
            }
        }
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
            Tile.GetComponent<Tiles>().XCoordinate = x;
            Tile.GetComponent<Tiles>().YCoordinate = y;
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

    void LocateStart()
    {
        bool IsStartInvalid = true;

        while (IsStartInvalid)
        {
            int StartX = Random.Range(0, MapWidth - 1);
            int StartY = Random.Range(0, MapHeight - 1);
            ValidTiles = new List<GameObject>();

            Tiles = Physics2D.OverlapBoxAll(new Vector2(StartX, StartY), new Vector2(1.5f, 1.5f), 0f);

            foreach (Collider2D tile in Tiles)
            {
                if (!tile.GetComponent<Tiles>().IsObstacle)
                    ValidTiles.Add(tile.gameObject);
            }

            if (ValidTiles.Count >= 4 && !GameObject.Find(StartX + "" + StartY).GetComponent<Tiles>().IsObstacle)
            {
                StartPoint = GameObject.Find(StartX + "" + StartY);
                IsStartInvalid = false;
                StartPoint.GetComponent<SpriteRenderer>().color = Color.red;
                LocateEnd();
            }
        }
    }

    void LocateEnd()
    {
        bool IsEndInvalid = true;

        while (IsEndInvalid)
        {
            int EndX = Random.Range(0, MapWidth - 1);
            int EndY = Random.Range(0, MapHeight - 1);

            if (!GameObject.Find(EndX + "" + EndY).GetComponent<Tiles>().IsObstacle && Vector2.Distance(StartPoint.transform.position, new Vector2(EndX, EndY)) > MapWidth/2)
            {
                EndPoint = GameObject.Find(EndX + "" + EndY);
                IsEndInvalid = false;
                EndPoint.GetComponent<SpriteRenderer>().color = Color.blue;
                UnitSpawnerScript.SpawnUnits(StartPoint, ValidTiles);
                EnemySpawnerScript.SpawnEnemies();
            }
        }

    }
}