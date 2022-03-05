using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    //public GameObject Tile;
    public bool isOccupied = false;
    public float mod1;
    public float mod2;

    [System.Serializable]
    public enum attackType
    {
        undefined,
        physical,
        magical,
        hybrid
    }
    public attackType thisattack;

    //Testing
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetModifiers(thisattack);
            Debug.Log(mod1 + " " + mod2);
        }
    }

    
    public void SetModifiers(attackType thisAttack)
    {
        attackType attack = thisAttack;

        switch (attack)
        {
            case attackType.undefined:
                mod1 = 1f;
                mod2 = 1f;
                Debug.Log("Attack type is undefined, no values for attack and defense will be set");
                break;
            case attackType.physical:
                Debug.Log("Physical");
                mod1 = 1.3f;
                mod2 = 0.7f;
                break;
            case attackType.magical:
                Debug.Log("Magical");
                mod1 = 1.5f;
                mod2 = 1f;
                break;
            case attackType.hybrid:
                Debug.Log("Hybrid");
                mod1 = 0f;
                mod2 = 0f;
                break;
        }
    }

    public int DamageCalc(int attackStat, int defenseStat)
    {
        int damage = Mathf.FloorToInt(((float)attackStat * mod1) - ((float)defenseStat * mod2));
        if (damage <= 1)
        {
            damage = 1;
        }

        Debug.Log(damage);
        return(damage);
    }
}
