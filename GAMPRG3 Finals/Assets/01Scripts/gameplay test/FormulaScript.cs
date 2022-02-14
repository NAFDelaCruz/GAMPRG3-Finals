using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaScript : MonoBehaviour
{
    public GameObject Tile;
    public bool isOccupied = false;

    public enum attackType
    { 
        physical,
        magical,
        hybrid,
        heal
    }

    public void GetEntity(string tileName)
    {
        Tile = GameObject.Find(tileName);
        if (Tile.transform.childCount != 0)
        {
            isOccupied = true;
        }
    }

    public static int PhysicalAttack(int A_STR, int D_DEF) //Takes the units strength [STR] and uses enemy defense [DEF]
    {
        int damage = Mathf.FloorToInt((float)A_STR - ((float)D_DEF * 0.8f));
        if (damage <= 0)
        {
            damage = 1;
        }
        return (damage);
    }

    public static int MagicAttack(int A_INT, int D_CON) //Takes units Intellect [INT] and uses enemy Constitution [CON]
    { 
        int damage = Mathf.FloorToInt((float)A_INT - ((float)D_CON * 0.9f));
        if (damage <= 0)
        {
            damage = 1;
        }
        return (damage);
    }
}
