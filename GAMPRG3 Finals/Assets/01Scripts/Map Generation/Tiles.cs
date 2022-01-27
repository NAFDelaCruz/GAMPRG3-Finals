using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int XCoordinate, YCoordinate;
    public string TileName;
    public bool IsObstacle;

    private void Start()
    {
        XCoordinate = (int)gameObject.transform.position.x;
        YCoordinate = (int)gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
