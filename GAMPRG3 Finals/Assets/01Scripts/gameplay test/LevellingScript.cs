using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevellingScript : MonoBehaviour
{
    public EntityStats entityStats;

    int statIncrease;
    void Start()
    {
        entityStats = this.gameObject.GetComponent<EntityStats>();
        if (entityStats == null)
        {
            Debug.Log("No stat script found");
        }
        else
        {
            Debug.Log("Script Found");
        }
    }

    public void LevelUp()
    {
        entityStats.EXP = entityStats.EXP - entityStats.MaxEXP;
        entityStats.Level++;
        StatTest(entityStats.HP_B);
        //Insert for other stats
        //AGI
        //STR
        //INT
        //DEF
        //CON
    }
    
    

    public float GenerateNumber()
    {
        float rng = Random.Range(1, 0);
        return rng;
    }

    public int StatTest(float statBias)
    {
        bool continueIncrease = true;
        float currentChance = 0;
        int statIncrease = 0;
        float rng = GenerateNumber();
        //Checks if this stat should increase
        if (rng > entityStats.HP_B)
        {
            while (continueIncrease)
            {
                rng = GenerateNumber();
                if (rng >= currentChance)
                {
                    statIncrease++;
                    currentChance = currentChance * 2; //0.0 -> 0.125 -> 0.25 > 0.5 -> 1
                    if (currentChance == 0)
                    {
                        currentChance = 0.125f;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        return statIncrease;
    }
}
