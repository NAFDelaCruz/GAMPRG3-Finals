using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedUnitUIManager : MonoBehaviour
{
    [Header("Set Reference Components")]
    public UnitSelector UnitSelector;
    public GameObject PlayerUI;

    [Header("Set UI Components")]
    public GameObject EXP;
    public GameObject LVL;
    public GameObject HP;
    public GameObject AP;
    public GameObject STR;
    public GameObject INT;
    public GameObject DEF;
    public GameObject CON;
    public GameObject AGI;
    public GameObject DamageType;
    public GameObject RangeType;
    public GameObject RangeValue;
    public GameObject TypeIcon;

    [Header("Set UI Image Components")]
    public Image UnitSprite;
    public Image HPFill;
    public Image APFill;
    public Image EXPFill;

    public Sprite PhysM;
    public Sprite PhysR;
    public Sprite MagiM;
    public Sprite MagiR;

    [Header("UI Component Values")]
    float EXPFillPercent;
    float HPFillPercent;
    float APFillPercent;

    // Update is called once per frame
    void Update()
    {
        if (UnitSelector.SelectedUnit != null)
        {
            PlayerUI.gameObject.SetActive(true);

            UnitSprite.sprite = UnitSelector.SelectedUnit.GetComponent<SpriteRenderer>().sprite;

            EXPFillPercent = (float)UnitSelector.SelectedUnitStats.EXP / (float)UnitSelector.SelectedUnitStats.MaxEXP;
            EXPFill.GetComponent<Image>().fillAmount = EXPFillPercent;

            APFillPercent = (float)UnitSelector.SelectedUnitStats.AP / (float)UnitSelector.SelectedUnitStats.Max_AP;
            APFill.GetComponent<Image>().fillAmount = APFillPercent;

            HPFillPercent = (float)UnitSelector.SelectedUnitStats.Curr_HP / (float)UnitSelector.SelectedUnitStats.HP;
            HPFill.GetComponent<Image>().fillAmount = HPFillPercent;

            EXP.GetComponent<TMP_Text>().text = "" + UnitSelector.SelectedUnitStats.EXP + " / " + UnitSelector.SelectedUnitStats.MaxEXP;

            LVL.GetComponent<TMP_Text>().text = "LVL: " + UnitSelector.SelectedUnitStats.Level;

            HP.GetComponent<TMP_Text>().text = "" + UnitSelector.SelectedUnitStats.Curr_HP + " / " + UnitSelector.SelectedUnitStats.HP;

            AP.GetComponent<TMP_Text>().text = "" + UnitSelector.SelectedUnitStats.AP + " / " + UnitSelector.SelectedUnitStats.Max_AP;

            STR.GetComponent<TMP_Text>().text = "STR: " + UnitSelector.SelectedUnitStats.STR;

            INT.GetComponent<TMP_Text>().text = "INT: " + UnitSelector.SelectedUnitStats.INT;

            DEF.GetComponent<TMP_Text>().text = "DEF: " + UnitSelector.SelectedUnitStats.DEF;

            CON.GetComponent<TMP_Text>().text = "CON: " + UnitSelector.SelectedUnitStats.CON;

            AGI.GetComponent<TMP_Text>().text = "AGI: " + UnitSelector.SelectedUnitStats.AGI;

            RangeValue.GetComponent<TMP_Text>().text = "Range: " + UnitSelector.SelectedUnitStats.AttackRange;

            //Damage and Range type
            if (UnitSelector.SelectedUnitStats.IsPhysical && UnitSelector.SelectedUnitStats.IsMelee) //Phys - Melee
            {
                DamageType.GetComponent<TMP_Text>().text = "Physical";
                RangeType.GetComponent<TMP_Text>().text = "Melee";
                TypeIcon.GetComponent<Image>().sprite = PhysM;
            }
            if (UnitSelector.SelectedUnitStats.IsPhysical && !UnitSelector.SelectedUnitStats.IsMelee) //Phys - Ranged
            {
                DamageType.GetComponent<TMP_Text>().text = "Physical";
                RangeType.GetComponent<TMP_Text>().text = "Ranged";
                TypeIcon.GetComponent<Image>().sprite = PhysR;
            }
            if (!UnitSelector.SelectedUnitStats.IsPhysical && UnitSelector.SelectedUnitStats.IsMelee) //Magi - Melee
            {
                DamageType.GetComponent<TMP_Text>().text = "Magic";
                RangeType.GetComponent<TMP_Text>().text = "Melee";
                TypeIcon.GetComponent<Image>().sprite = MagiM;
            }
            if (!UnitSelector.SelectedUnitStats.IsPhysical && !UnitSelector.SelectedUnitStats.IsMelee) //Magi - Ranged
            {
                DamageType.GetComponent<TMP_Text>().text = "Magic";
                RangeType.GetComponent<TMP_Text>().text = "Ranged";
                TypeIcon.GetComponent<Image>().sprite = MagiR;
            }

        }
        else if (UnitSelector.SelectedUnit == null)
        {
            PlayerUI.gameObject.SetActive(false);
        }
    }
}
