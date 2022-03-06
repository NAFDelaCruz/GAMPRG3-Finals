using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public float mod1;
    public float mod2;
    
    public void SetModifiers(bool isPhysical)
    {
        if (isPhysical)
        {
            mod1 = 1.3f;
            mod2 = 0.7f;
        }
        else
        {
            mod1 = 1.5f;
            mod2 = 1f;
        }
    }

    public int DamageCalc(int attackStat, int defenseStat)
    {
        int damage = Mathf.FloorToInt(((float)attackStat * mod1) - ((float)defenseStat * mod2));
        if (damage <= 1)
        {
            damage = 1;
        }
        
        return(damage);
    }
}
