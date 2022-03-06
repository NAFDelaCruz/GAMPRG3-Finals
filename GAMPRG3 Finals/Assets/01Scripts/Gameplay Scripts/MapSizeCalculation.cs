using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSizeCalculation : MonoBehaviour
{
    public DifficultyManager DifficultyManager;

    public int Difficulty;
    public float DefaultSize = 20;
    public float SizeScaleFactor = 1.1f;
    public float VariationFactor = 0.25f;

    float scaledSize;
    float variation;

   

    public int GenHeight;
    public int GenWidth;
    public void GenerateDimensions(int Difficulty)
    {
        scaledSize = Mathf.FloorToInt(DefaultSize + (Difficulty * SizeScaleFactor) - (0.5f * Difficulty));
        RandomizeDimension(scaledSize);
    }

    public void RandomizeDimension(float scaledSize)
    {
        variation = Mathf.FloorToInt((scaledSize * VariationFactor) / 2);
        GenHeight = Mathf.FloorToInt((Random.Range(scaledSize - variation, scaledSize + variation)));
        GenWidth = Mathf.FloorToInt((Random.Range(scaledSize - variation, scaledSize + variation)));
    }
}
