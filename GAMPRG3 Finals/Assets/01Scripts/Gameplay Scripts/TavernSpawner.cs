using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernSpawner : MonoBehaviour
{
    //public SceneChanger SceneChanger;
    public GameObject[] Spawnlist;

    public GameObject[] SpawnLocations;

    public void SpawnTavern()
    {
        for (int i = 0; i < SpawnLocations.Length; i++)
        {
            int rng = Random.Range(0, Spawnlist.Length);
            GameObject spawnedUnitPrefab = Instantiate(Spawnlist[rng], transform.position, Quaternion.identity) as GameObject;
            spawnedUnitPrefab.transform.parent = SpawnLocations[i].transform;
            spawnedUnitPrefab.transform.position = SpawnLocations[i].transform.position;
        }
    }

    public void ClearTavern()
    {
        for (int i = 0; i < SpawnLocations.Length; i++)
        {
            GameObject.Destroy(SpawnLocations[i].transform.GetChild(0).gameObject);
        }
    }
}
