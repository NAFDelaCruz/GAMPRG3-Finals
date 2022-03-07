using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    TurnManager TurnManagerScript;
    GenerateMap MapScript;

    private void Start()
    {
        MapScript = GetComponent<GenerateMap>();
        TurnManagerScript = GetComponent<TurnManager>();
        TurnManagerScript.TurnEnds.AddListener(CheckIfEnd);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void CheckIfEnd()
    {
        if (GameObject.FindGameObjectsWithTag("FriendlyUnit").Length < 11)
            Restart();

        /*
        if (MapScript.EndPoint.transform.childCount > 0 && MapScript.EndPoint.transform.GetChild(0).CompareTag("Friendly Unit"))
            Restart();
        */
    }
}
