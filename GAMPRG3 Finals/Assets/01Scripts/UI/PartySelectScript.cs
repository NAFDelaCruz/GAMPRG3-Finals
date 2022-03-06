using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PartySelectScript : MonoBehaviour
{
    [Header("Set Reference Components")]
    public GameObject PlayerUI;
    public UnitSpawner PartyList;
    GameObject SelectedUnit;
    EntityStats SelectedUnitStats;
    RaycastHit hit;
    
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
    public Sprite PhysM;
    public Sprite PhysR;
    public Sprite MagiM;
    public Sprite MagiR;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (PartyList.TestPartyUnits.Contains(hit.collider.gameObject) && hit.collider)
                {
                    hit.collider.gameObject.transform.parent.GetComponent<SpriteRenderer>().color = Color.white;
                    PartyList.TestPartyUnits.Remove(hit.collider.gameObject);
                }
                else if (PartyList.TestPartyUnits.Count < 4)
                {
                    SelectedUnit = hit.collider.gameObject;
                    SelectedUnitStats = SelectedUnit.GetComponent<EntityStats>();
                    SelectedUnit.transform.parent.GetComponent<SpriteRenderer>().color = Color.green;
                    PartyList.TestPartyUnits.Add(SelectedUnit);
                }
            }
        }

        if (SelectedUnit != null)
        {
            PlayerUI.SetActive(true);

            UnitSprite.sprite = SelectedUnit.GetComponent<SpriteRenderer>().sprite;

            EXP.GetComponent<TMP_Text>().text = "" + SelectedUnitStats.EXP + " / " + SelectedUnitStats.MaxEXP;

            LVL.GetComponent<TMP_Text>().text = "LVL: " + SelectedUnitStats.Level;

            HP.GetComponent<TMP_Text>().text = "" + SelectedUnitStats.Curr_HP + " / " + SelectedUnitStats.HP;

            AP.GetComponent<TMP_Text>().text = "" + SelectedUnitStats.AP + " / " + SelectedUnitStats.Max_AP;

            STR.GetComponent<TMP_Text>().text = "STR: " + SelectedUnitStats.STR;

            INT.GetComponent<TMP_Text>().text = "INT: " + SelectedUnitStats.INT;

            DEF.GetComponent<TMP_Text>().text = "DEF: " + SelectedUnitStats.DEF;

            CON.GetComponent<TMP_Text>().text = "CON: " + SelectedUnitStats.CON;

            AGI.GetComponent<TMP_Text>().text = "AGI: " + SelectedUnitStats.AGI;

            RangeValue.GetComponent<TMP_Text>().text = "Range: " + SelectedUnitStats.AttackRange;

            //Damage and Range type
            if (SelectedUnitStats.IsPhysical && SelectedUnitStats.IsMelee) //Phys - Melee
            {
                DamageType.GetComponent<TMP_Text>().text = "Physical";
                RangeType.GetComponent<TMP_Text>().text = "Melee";
                TypeIcon.GetComponent<Image>().sprite = PhysM;
            }
            if (SelectedUnitStats.IsPhysical && !SelectedUnitStats.IsMelee) //Phys - Ranged
            {
                DamageType.GetComponent<TMP_Text>().text = "Physical";
                RangeType.GetComponent<TMP_Text>().text = "Ranged";
                TypeIcon.GetComponent<Image>().sprite = PhysR;
            }
            if (!SelectedUnitStats.IsPhysical && SelectedUnitStats.IsMelee) //Magi - Melee
            {
                DamageType.GetComponent<TMP_Text>().text = "Magic";
                RangeType.GetComponent<TMP_Text>().text = "Melee";
                TypeIcon.GetComponent<Image>().sprite = MagiM;
            }
            if (!SelectedUnitStats.IsPhysical && !SelectedUnitStats.IsMelee) //Magi - Ranged
            {
                DamageType.GetComponent<TMP_Text>().text = "Magic";
                RangeType.GetComponent<TMP_Text>().text = "Ranged";
                TypeIcon.GetComponent<Image>().sprite = MagiR;
            }
        }
    }
}
