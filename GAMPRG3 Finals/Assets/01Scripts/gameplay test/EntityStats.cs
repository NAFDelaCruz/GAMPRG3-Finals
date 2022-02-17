using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Levelling")]
    public int Level;
    public int EXP;
    public int MaxEXP;
    public int RewardEXP;
    [Header("Base Stats")]
    public int HP;
    public int AGI; //Action Priority

    public int STR; //Physical Damage
    public int INT; //Magic Damage

    public int DEF; //Physical Resistance
    public int CON; //Magical Resistance

    [Header("Action Points")]
    public int Max_AP;
    public int AP;

    [Header("Stat Bias")]
    public float HP_B;
    public float AGI_B;
    public float STR_B;
    public float INT_B;
    public float DEF_B;
    public float CON_B;
}
