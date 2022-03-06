using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public bool IsAtStart = true;
    public bool IsAtTavern = false;
    public bool IsInGame = false;

    public GameObject Camera;
    public Vector3 CamPos;

    public GameObject TitleScreenUI;
    public GameObject TavernUI;
    public GameObject GameUI;

    Vector3 TitlePosition = new Vector3(-150, 0, -10);
    Vector3 TavernPosition = new Vector3(-100, 0, -10);
    public void GoToTitle()
    {
        IsAtStart = true;
        IsAtTavern = false;
        IsInGame = false;
        Camera.transform.position = new Vector3(-150, 0, -10);
        TitleScreenUI.SetActive(true);
        TavernUI.SetActive(false);
        GameUI.SetActive(false);
        Camera.GetComponent<CameraController>().StartGame = false;
    }

    public void GoToTavern()
    {
        IsAtStart = false;
        IsAtTavern = true;
        IsInGame = false;
        Camera.transform.position = new Vector3(-100, 0, -10);
        TitleScreenUI.SetActive(false);
        TavernUI.SetActive(true);
        GameUI.SetActive(false);
        Camera.GetComponent<CameraController>().StartGame = false;
    }

    public void StartGame()
    {
        IsAtStart = false;
        IsAtTavern = false;
        IsInGame = true;
        TitleScreenUI.SetActive(false);
        TavernUI.SetActive(false);
        GameUI.SetActive(true);
        Camera.GetComponent<CameraController>().StartGame = true;
    }
}
