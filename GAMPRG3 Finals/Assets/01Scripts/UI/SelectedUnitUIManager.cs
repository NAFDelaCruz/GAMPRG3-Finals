using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedUnitUIManager : MonoBehaviour
{
    [Header("Set Reference Components")]
    public UnitSelector UnitSelector;
    public Canvas PlayerUI;

    [Header("Set UI Text Components")]
    public GameObject EXP;
    public GameObject LVL;
    public GameObject HP;
    public GameObject AP;
    public GameObject STR;
    public GameObject INT;
    public GameObject DEF;
    public GameObject CON;
    public GameObject AGI;

    [Header("Set UI Image Components")]
    public Image UnitSprite;
    public Image HPFill;
    public Image APFill;
    public Image EXPFill;

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


        }
        else if (UnitSelector.SelectedUnit == null)
        {
            PlayerUI.gameObject.SetActive(false);
        }
    }
}
