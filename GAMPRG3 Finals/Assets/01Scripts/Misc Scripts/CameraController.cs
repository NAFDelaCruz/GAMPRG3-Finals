﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public SceneChanger SceneChanger;

    public bool StartGame = false;

    GameObject gameCamera;
    public float MovementSensitivity;

    public GameObject Map;
    GenerateMap generateMap;
    float xBorder;
    float yBorder;

    bool setToStart = false;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = this.gameObject;
        generateMap = Map.GetComponent<GenerateMap>();
        xBorder = generateMap.MapWidth;
        yBorder = generateMap.MapHeight;
    }

    // Update is called once per frame
    void Update()
    {

        if (StartGame)
        {
            if (!setToStart)
            {
                gameCamera.transform.position = generateMap.StartPoint.transform.position + new Vector3(0, 0, -10);
                if (gameCamera.transform.position.x == generateMap.StartPoint.transform.position.x)
                {
                    setToStart = true;
                }
            }
            //Using WASD input system
            if (Input.GetKey(KeyCode.W)) //Up
            {
                gameCamera.transform.position += new Vector3(0, MovementSensitivity * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.S)) //Down
            {
                gameCamera.transform.position -= new Vector3(0, MovementSensitivity * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.A)) //Left
            {
                gameCamera.transform.position -= new Vector3(MovementSensitivity * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.D)) //Right
            {
                gameCamera.transform.position += new Vector3(MovementSensitivity * Time.deltaTime, 0, 0);
            }

            if (this.transform.position.x > xBorder)
            {
                this.transform.position = new Vector3(xBorder, this.transform.position.y, this.transform.position.z);
            }
            if (this.transform.position.x < 0)
            {
                this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
            }
            if (this.transform.position.y > yBorder)
            {
                this.transform.position = new Vector3(this.transform.position.x, yBorder, this.transform.position.z);
            }
            if (this.transform.position.y < 0)
            {
                this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            }
        }

    }
}
