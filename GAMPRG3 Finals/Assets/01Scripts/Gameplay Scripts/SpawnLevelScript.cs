using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevelScript : MonoBehaviour
{

    //To be attached to enemy prefabs
    //will set a predetermined random level based on what stage the player is on
    //Calls the level up function until the current level is set (for the purpose of random stats)
    //When the set level is reached the script is destroyed and the enemy will act as normal

    public LevellingScript levellingScript;
    public EntityStats entityStats;
    GameObject GameManager;
    DifficultyManager difficultyManager;

    int currentLevel;
    int setLevel;
    private void Start()
    {
        entityStats = this.gameObject.GetComponent<EntityStats>();
        levellingScript = this.gameObject.GetComponent<LevellingScript>();
        GameManager = GameObject.Find("Game Manager");
        difficultyManager = GameManager.GetComponent<DifficultyManager>();
        SetNewLevel();
    }

    private void Update()
    {
        if (currentLevel < setLevel)
        {
            levellingScript.LevelUp();
            currentLevel++;
        }
        else
        {
            entityStats.Curr_HP = entityStats.HP;
            entityStats.EXP = 0;
            //SetMaxEXP();
            SetRewardEXP();
            Destroy(this,2); //Destroy this script
        }
    }

    private void SetNewLevel()
    {
        setLevel = Mathf.FloorToInt(Random.Range(difficultyManager.Difficulty-3,difficultyManager.Difficulty+4));
    }

    public void SetRewardEXP()
    {
        entityStats.RewardEXP = Mathf.FloorToInt(1.5f * Mathf.Pow(entityStats.Level,2));
    }

    //public void SetMaxEXP()
    //{
    //    entityStats.MaxEXP = Mathf.FloorToInt(((entityStats.MaxEXP * 1.5f) - (entityStats.MaxEXP * 0.2f)));
    //}
}
