using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    public int Difficulty;
    public GameObject DifficultyNumber;
    //To be used for enemy stat generation and for map size variability
    //Also used to determine enemy spawning level range

    public void IncreaseDifficulty()
    {
        Difficulty++;
        UpdateDifficultyNumber();
    }
    public void DecreaseDifficulty()
    {
        if (Difficulty > 1)
        {
            Difficulty--;
        }
        UpdateDifficultyNumber();
    }

    public void UpdateDifficultyNumber()
    {
        DifficultyNumber.GetComponent<TMP_Text>().text = "" + Difficulty;
    }
}