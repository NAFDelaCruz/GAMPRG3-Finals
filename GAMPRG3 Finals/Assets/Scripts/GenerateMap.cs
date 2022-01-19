using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Authored by: Danndx 2021 (youtube.com/danndx)
//From video: youtu.be/qNZ-0-7WuS8

//Lines marked with /**/ 
//in the beginning are authored by Niel Angelo Dela Cruz

//All variables renamed by Niel Angelo Dela Cruz to follow proper C# coding

public class GenerateMap : MonoBehaviour
{
    Dictionary<int, GameObject> TileSet;
    Dictionary<int, GameObject> TileGroups;

    [Header("Reference Objects")]
    [Tooltip("Set in order of Landscape height, put Voids and Obstacles in the end.")]
    /**/
    public List<GameObject> TerrainPrefabs;

    [Header("Map Variables")]

    public int MapWidth = 160;
    public int MapHeight = 90;

    [Tooltip("Controls how detailed the map is. Recommended value: 4-20")]
    public float Magnification = 7.0f;

    [Tooltip("Shifts tiles to the left (-) or right (+).")]
    public int XOffset = 0; // <- +>
    [Tooltip("Shifts tiles down (-) or up (+).")]
    public int YOffset = 0; // v- +^

    List<List<int>> NoiseGrid = new List<List<int>>();
    List<List<GameObject>> TileGrid = new List<List<GameObject>>();

    void Start()
    {
        CreateTileset();
        //CreateTileGroups();
        Generate();
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/
/**/
        int Index = 0;

        TileSet = new Dictionary<int, GameObject>();
        foreach (GameObject TilePrefab in TerrainPrefabs)
        {
            TileSet.Add(Index, TilePrefab);
            Index++;
        }
        
/**/    
    }

    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/

        TileGroups = new Dictionary<int, GameObject>();

/**/    int Index = 0;

        foreach (KeyValuePair<int, GameObject> PrefabPairs in TileSet)
        {
            GameObject TileGroup = new GameObject(PrefabPairs.Value.name);
            TileGroup.transform.parent = gameObject.transform;
            TileGroup.transform.localPosition = new Vector3(0, 0, 0);
            TileGroups.Add(Index, TileGroup);
            Index++;
/**/    }

    }

    void Generate()
    {
        /** Generate a 2D grid using the Perlin noise fuction, storing it as
            both raw ID values and tile gameobjects **/

        for (int x = 0; x < MapWidth; x++)
        {
            NoiseGrid.Add(new List<int>());
            TileGrid.Add(new List<GameObject>());

            for (int y = 0; y < MapHeight; y++)
            {
                int TileID = GetIdUsingPerlin(x, y);
                NoiseGrid[x].Add(TileID);
                CreateTile(TileID, x, y);
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
        float ScaledPerlin = Mathf.Clamp(ClampPerlin * TileSet.Count, 0, TileSet.Count - 1);

        return Mathf.FloorToInt(ScaledPerlin);
    }

    void CreateTile(int TileID, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common
            tiles, set it's position and store the gameobject. **/

        GameObject TilePrefab = TileSet[TileID];
        //GameObject TileGroup = TileGroups[TileID];
        GameObject Tile = Instantiate(TilePrefab, gameObject.transform);
        Tiles TilesScript = TilePrefab.GetComponent<Tiles>();

        TilesScript.XCoordinate = x;
        TilesScript.YCoordinate = y;
        Tile.name = string.Format("{0}{1}", x, y);
        Tile.transform.localPosition = new Vector3(x, y, 0);

        TileGrid[x].Add(Tile);
    }

    void CreateVoids()
    {

    }
}