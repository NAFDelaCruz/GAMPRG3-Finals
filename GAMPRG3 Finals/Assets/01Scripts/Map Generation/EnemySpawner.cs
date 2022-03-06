using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Parameters")]
    [Tooltip("The distance of which enemies will spawn away from the player party")]
    public float GraceDistance;
    int NumberOfCamps;
    GenerateMap MapScript;

    [Header("Enemy Collections")]
    [Tooltip("Insert Prefabs of each type here in order of highest spawn chance to lowest.")]
    public List<GameObject> Cave;
    [Tooltip("Input spawn chanes of each type here in same order, total must equate to 1.0f.")]
    public List<float> CaveChance; 
    public List<GameObject> Demons; 
    public List<float> DemonsChance; 
    public List<GameObject> Elemental; 
    public List<float> ElementalChance; 
    public List<GameObject> Mythicals; 
    public List<float> MythicalsChance; 
    public List<GameObject> Undead;
    public List<float> UndeadChance;

    [Header("Collection Chance Values")]
    [Tooltip("Insert chances of each group here in order of the collections above. (Cave chance, Demon chance, etc.)")]
    public List<float> PossibleEnemyTypesChances;
    [Tooltip("Insert minimum unit count of each group here in order of the collections above. (Cave chance, Demon chance, etc.)")]
    public List<int> PossibleEnemyTypesGroupMin;
    [Tooltip("Insert maximum unit count of each group here in order of the collections above. (Cave chance, Demon chance, etc.)")]
    public List<int> PossibleEnemyTypesGroupMax;

    List<Dictionary<float, GameObject>> PossibleEnemyTypes = new List<Dictionary<float, GameObject>>();
    List<List<GameObject>> EnemyTypes = new List<List<GameObject>>();
    List<List<float>> EnemyTypeChances = new List<List<float>>();

    private void Start()
    {
        EnemyTypes.Add(Cave);
        EnemyTypes.Add(Demons);
        EnemyTypes.Add(Elemental);
        EnemyTypes.Add(Mythicals);
        EnemyTypes.Add(Undead);
        EnemyTypeChances.Add(CaveChance);
        EnemyTypeChances.Add(DemonsChance);
        EnemyTypeChances.Add(ElementalChance);
        EnemyTypeChances.Add(MythicalsChance);
        EnemyTypeChances.Add(UndeadChance);

        CreateEnemySets();

        MapScript = GetComponent<GenerateMap>();
        NumberOfCamps = Mathf.FloorToInt(0.03125f * (MapScript.MapWidth * MapScript.MapHeight));
    }

    public void SpawnEnemies()
    {
        for (int Count = 0; Count < NumberOfCamps; Count++)
        {
            int TypeRNG = Random.Range(0, PossibleEnemyTypes.Count - 1);
            float RNG = Random.Range(0f, 1f);
            int GroupAmount = Random.Range(PossibleEnemyTypesGroupMin[TypeRNG], PossibleEnemyTypesGroupMax[TypeRNG]);
            
            if (PossibleEnemyTypesChances[TypeRNG] > RNG)
            {
                for (int UnitCount = 0; UnitCount < GroupAmount; UnitCount++)
                {
                    float TotalWeight = GetTotalWeight(PossibleEnemyTypes[TypeRNG]);
                    GameObject SelectedEnemy = SelectEnemy(PossibleEnemyTypes[TypeRNG], TotalWeight);
                    List<GameObject> ValidTiles = ValidateSpawnArea(GroupAmount);

                    Instantiate(SelectedEnemy, ValidTiles[UnitCount].transform);
                }
            }
            else if (PossibleEnemyTypesChances[TypeRNG] < RNG)
            {
                Count--; //skips the type but maintains count
            }
        }
    }

    GameObject SelectEnemy(Dictionary<float, GameObject> SelectedSet, float TotalWeight)
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

    List<GameObject> ValidateSpawnArea(int GroupAmount)
    {
        List<GameObject> ValidTiles = new List<GameObject>();

        while (ValidTiles.Count < GroupAmount)
        {
            int StartX = Random.Range(0, MapScript.MapWidth - 1);
            int StartY = Random.Range(0, MapScript.MapHeight - 1);
            ValidTiles = new List<GameObject>();
            Collider2D[] Tiles = null;

            if (Vector2.Distance(MapScript.StartPoint.transform.position, new Vector2(StartX, StartY)) > GraceDistance)
            {
                Tiles = Physics2D.OverlapBoxAll(new Vector2(StartX, StartY), new Vector2(1.5f, 1.5f), 0f);

                foreach (Collider2D Tile in Tiles)
                {
                    if (!Tile.gameObject.GetComponent<Tiles>().IsObstacle && Tile.gameObject.transform.childCount == 0)
                        ValidTiles.Add(Tile.gameObject);
                }
            }
        }

        return ValidTiles;
    }
    
    void CreateEnemySets()
    {
        int SetNumber = 0;

        foreach (List<GameObject> EnemyPrefabs in EnemyTypes)
        {
            Dictionary<float, GameObject> PairTemp = new Dictionary<float, GameObject>();
            int ChanceIndex = 0;

            foreach (GameObject Enemy in EnemyPrefabs)
            {
                PairTemp.Add(EnemyTypeChances[SetNumber][ChanceIndex], Enemy);
                ChanceIndex++;
            }

            PossibleEnemyTypes.Add(PairTemp);
            SetNumber++;
        }
    }
}
