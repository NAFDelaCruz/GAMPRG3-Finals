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

    [Header("Attack Type")]
    public bool IsMelee;
    public bool IsPhysical;

    [Header("Base Stats")]
    public int HP;
    public int Curr_HP;
    public int AGI; //Action Priority

    public int STR; //Physical Damage
    public int INT; //Magic Damage

    public int DEF; //Physical Resistance
    public int CON; //Magical Resistance

    [Header("Action Points")]
    public int AttackRange;
    public int Max_AP;
    public int AP;
    [HideInInspector]
    public int AP_LevelUp; //Starts at 0, used to detect if AP should be increased

    [Header("Stat Bias")]
    public float HP_B;
    public float AGI_B;
    public float STR_B;
    public float INT_B;
    public float DEF_B;
    public float CON_B;

    private void Start()
    {
        Curr_HP = HP;
        InvertBias();
    }

    public void InvertBias() //Converts Bias Values to the inverse for RNG test. ex. 0.8 bias gets converted to 0.2 for RNG test
    {
        HP_B = -HP_B + 1;
        AGI_B = -AGI_B + 1;
        STR_B = -STR_B + 1;
        INT_B = -INT_B + 1;
        DEF_B = -DEF_B + 1;
        CON_B = -CON_B + 1;
    }
}
