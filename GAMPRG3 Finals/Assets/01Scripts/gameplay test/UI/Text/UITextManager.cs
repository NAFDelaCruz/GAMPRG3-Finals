using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextManager : MonoBehaviour
{
    public UnitSelector UnitSelector;

    public GameObject SelectedUnit;

    public GameObject EXP;
    public GameObject LVL;
    public GameObject HP;
    public GameObject AP;
    public GameObject STR;
    public GameObject INT;
    public GameObject DEF;
    public GameObject CON;
    public GameObject AGI;

    // Update is called once per frame
    void Update()
    {
        if (UnitSelector.SelectedUnit != null)
        {
            SelectedUnit = UnitSelector.SelectedUnit;
        }

        if (SelectedUnit != null)
        {

            EXP.GetComponent<TMP_Text>().text = "" + SelectedUnit.GetComponent<EntityStats>().EXP + " / " + SelectedUnit.GetComponent<EntityStats>().MaxEXP;

            LVL.GetComponent<TMP_Text>().text = "LVL: " + SelectedUnit.GetComponent<EntityStats>().Level;

            HP.GetComponent<TMP_Text>().text = "" + SelectedUnit.GetComponent<EntityStats>().Curr_HP + " / " + SelectedUnit.GetComponent<EntityStats>().HP;

            AP.GetComponent<TMP_Text>().text = "" + SelectedUnit.GetComponent<EntityStats>().AP + " / " + SelectedUnit.GetComponent<EntityStats>().Max_AP;

            STR.GetComponent<TMP_Text>().text = "STR: " + SelectedUnit.GetComponent<EntityStats>().STR;

            INT.GetComponent<TMP_Text>().text = "INT: " + SelectedUnit.GetComponent<EntityStats>().INT;

            DEF.GetComponent<TMP_Text>().text = "DEF: " + SelectedUnit.GetComponent<EntityStats>().DEF;

            CON.GetComponent<TMP_Text>().text = "CON: " + SelectedUnit.GetComponent<EntityStats>().CON;

            AGI.GetComponent<TMP_Text>().text = "AGI: " + SelectedUnit.GetComponent<EntityStats>().AGI;
        }
    }
}
