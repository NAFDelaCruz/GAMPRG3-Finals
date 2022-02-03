using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject TestUnit;
    public GameObject Map;
    GenerateMap generateMap;
    // Start is called before the first frame update
    void Start()
    {
        generateMap = Map.GetComponent<GenerateMap>();
        Instantiate(TestUnit, generateMap.StartPoint.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
