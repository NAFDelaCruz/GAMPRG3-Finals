using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int NumberOfCamps;
    GenerateMap MapScript;
    public List<Dictionary<float, GameObject>> PossibleEnemyTypes;
    public List<float> PossibleEnemyTypesChances;

    public List<GameObject> Cave; 
    public List<float> CaveChance; 
    public List<GameObject> Demons; 
    public List<float> DemonsChance; 
    public List<GameObject> Elemental; 
    public List<float> ElementalChance; 
    public List<GameObject> Mythicals; 
    public List<float> MythicalsChance; 
    public List<GameObject> Undead;
    public List<float> UndeadChance;

    public List<List<GameObject>> EnemyTypes;
    public List<List<float>> EnemyTypeChances;

    private void Start()
    {
        NumberOfCamps = Mathf.FloorToInt(0.0625f * (MapScript.MapWidth * MapScript.MapHeight));

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

    public void SpawnEnemies()
    {
        for (int Count = 0; Count < NumberOfCamps; Count++)
        {
            int TypeRNG = Random.Range(0, PossibleEnemyTypes.Count - 1);
            float RNG = Random.Range(0f, 1f);
            
            if (PossibleEnemyTypesChances[TypeRNG] > RNG)
            {
                
            }
            else if (PossibleEnemyTypesChances[TypeRNG] < RNG)
            {
                Count--; //skips the type but maintains 
            }
        }
    }
    
}
