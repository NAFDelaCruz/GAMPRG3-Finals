using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Special thanks to Danndx 2021 (youtube.com/danndx) for authoring the base code
//From video: youtu.be/qNZ-0-7WuS8

//This script has been modified by Niel Angelo Dela Cruz for the purpose of the game

public class GenerateMap : MonoBehaviour
{

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

    [Header("Map Void Variables")]
    public int VoidCount;
    public int MinVoidSize;
    public int MaxVoidSize;

    [Tooltip("Controls how detailed the map is. Recommended value: 4-20")]
    public float Magnification;

    [Tooltip("Shifts tiles to the left (-) or right (+).")]
    public int XOffset = 0; // <- +>
    [Tooltip("Shifts tiles down (-) or up (+).")]
    public int YOffset = 0; // v- +^
    
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
        //CreateVoids();
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
    }

    int GetIdUsingPerlin(int x, int y)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalised Perlin value
            to the number of tiles available. **/

        float RawPerlin = Mathf.PerlinNoise(
            (x - XOffset) / Magnification,
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
        
        GameObject Tile = Instantiate(TilePrefab, gameObject.transform);
        /*
        Tiles TilesScript = TilePrefab.GetComponent<Tiles>();
        TilesScript.XCoordinate = x;
        TilesScript.YCoordinate = y;
        */
        Tile.name = string.Format("{0}{1}", x, y);
        Tile.transform.localPosition = new Vector3(x, y, 0);
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

    void CountVoids()
    {
        for (int Index = 1; Index < VoidCount; Index++)
        {

            int X = Random.Range(0, MapWidth - 1);
            int Y = Random.Range(0, MapHeight - 1);
            int XVoidSize = Random.Range(MinVoidSize, MaxVoidSize);
            int YVoidSize = Random.Range(MinVoidSize, MaxVoidSize);

            int Size = YVoidSize;

            if (XVoidSize > YVoidSize)
                Size = XVoidSize;
            CreateVoids(Size);
        }
    }

    void CreateVoids(int Size)
    {
        for (int X = 0; X < Size; X++)
        {
               
        }
    }
}