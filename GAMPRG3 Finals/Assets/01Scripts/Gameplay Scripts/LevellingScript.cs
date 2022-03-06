using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevellingScript : MonoBehaviour
{
    [Header("Set Components")]
    public EntityStats entityStats;

    [Header("HP Stat Values")]
    public float HPStartChance;
    public float HPBaseChance;
    public float HPChanceMultiplier;

    [Header("AGI Stat Values")]
    public float AGIStartChance;
    public float AGIBaseChance;
    public float AGIChanceMultiplier;

    [Header("SRT Stat Values")]
    public float STRStartChance;
    public float STRBaseChance;
    public float STRChanceMultiplier;

    [Header("INT Stat Values")]
    public float INTStartChance;
    public float INTBaseChance;
    public float INTChanceMultiplier;

    [Header("DEF Stat Values")]
    public float DEFStartChance;
    public float DEFBaseChance;
    public float DEFChanceMultiplier;

    [Header("CON Stat Values")]
    public float CONStartChance;
    public float CONBaseChance;
    public float CONChanceMultiplier;

    [Header("Tracking Variables")]
    int statIncrease;

    void Start()
    {
        entityStats = gameObject.GetComponent<EntityStats>();
    }

    public void LevelUp()
    {
        entityStats.EXP = entityStats.EXP - entityStats.MaxEXP;
        entityStats.MaxEXP = Mathf.FloorToInt(((entityStats.MaxEXP * 1.5f) - (entityStats.MaxEXP * 0.2f)));
        entityStats.Level++;

        entityStats.HP += StatHP(entityStats.HP_B);
        entityStats.AGI += StatAGI(entityStats.AGI_B);
        entityStats.STR += StatAGI(entityStats.STR_B);
        entityStats.INT += StatAGI(entityStats.INT_B);
        entityStats.DEF += StatAGI(entityStats.DEF_B);
        entityStats.CON += StatAGI(entityStats.CON_B);

        if (entityStats.Max_AP <= 8) //Increases Max AP every 2 levels Up to a maximum of 8
        {
            entityStats.AP_LevelUp ++;
            if (entityStats.AP_LevelUp == 2)
            {
                entityStats.AP_LevelUp = 0;
                entityStats.Max_AP++;
            }
        }
    }
    
    public float GenerateNumber()
    {
        float rng = Random.Range(1.0f, 0.0f);
        return rng;
    }

    public int IncreaseStat(float startChance, float baseChance, float chanceMultiplier)
    {
        int statIncrease = 0;

            float rng = GenerateNumber();
            //Debug.Log(rng);
            switch (rng)
            {
                case var expression when (rng > 0 && rng < 0.4f):
                    statIncrease = 1;
                    break;
                case var expression when (rng >= 0.4f && rng < 0.6f):
                    statIncrease = 2;
                    break;
                case var expression when (rng >= 0.6f && rng < 0.9f):
                    statIncrease = 3;
                    break;
                case var expression when (rng >= .9f):
                    statIncrease = 4;
                    break;
                default:
                    break;
            }
        return statIncrease;
    }

    public int StatHP(float statBias)
    {
        float rng = GenerateNumber();
        int statIncrease = new int();

        //Checks if this stat should increase
        if (rng > statBias)
            statIncrease = IncreaseStat(HPStartChance, HPBaseChance, HPChanceMultiplier);

        return statIncrease;
    }

    public int StatAGI(float statBias)
    {
        float rng = GenerateNumber();
        int statIncrease = new int();

        //Checks if this stat should increase
        if (rng > statBias)
            statIncrease = IncreaseStat(AGIStartChance, AGIBaseChance, AGIChanceMultiplier);

        return statIncrease;
    }

    public int StatSTR(float statBias)
    {
        float rng = GenerateNumber();
        int statIncrease = new int();

        //Checks if this stat should increase
        if (rng > statBias)
            statIncrease = IncreaseStat(STRStartChance, STRBaseChance, STRChanceMultiplier);

        return statIncrease;
    }

    public int StatINT(float statBias)
    {
        float rng = GenerateNumber();
        int statIncrease = new int();

        //Checks if this stat should increase
        if (rng > statBias)
            statIncrease = IncreaseStat(0, 0.125f, 2);

        return statIncrease;
    }

    public int StatDEF(float statBias)
    {
        float rng = GenerateNumber();
        int statIncrease = new int();

        //Checks if this stat should increase
        if (rng > statBias)
            statIncrease = IncreaseStat(DEFStartChance, DEFBaseChance, DEFChanceMultiplier);

        return statIncrease;
    }

    public int StatCON(float statBias)
    {
        float rng = GenerateNumber();
        int statIncrease = new int();

        //Checks if this stat should increase
        if (rng > statBias)
            statIncrease = IncreaseStat(CONStartChance, CONBaseChance, CONChanceMultiplier);

        return statIncrease;
    }
}
