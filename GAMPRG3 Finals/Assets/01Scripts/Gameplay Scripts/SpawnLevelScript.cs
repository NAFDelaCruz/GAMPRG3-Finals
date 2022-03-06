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
    public DifficultyManager difficultyManager;
    public EntityStats entityStats;

    int currentLevel;
    int setLevel;
    private void Start()
    {
        entityStats = this.gameObject.GetComponent<EntityStats>();
        levellingScript = this.gameObject.GetComponent<LevellingScript>();

        SetNewLevel();
    }

    private void Update()
    {
        if (currentLevel <= setLevel)
        {
            levellingScript.LevelUp();
            currentLevel++;
        }
        else
        {
            entityStats.Curr_HP = entityStats.HP;
            SetRewardEXP();
            Destroy(this); //Destroy this script
        }
    }

    private void SetNewLevel()
    {
        setLevel = Mathf.FloorToInt(Random.Range(difficultyManager.Difficulty-3,difficultyManager.Difficulty+4));
    }

    public void SetRewardEXP()
    {
        entityStats.RewardEXP = Mathf.FloorToInt(1.5f * entityStats.Level);
    }
}
