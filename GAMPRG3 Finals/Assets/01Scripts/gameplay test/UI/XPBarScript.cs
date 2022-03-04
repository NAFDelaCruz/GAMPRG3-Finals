using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBarScript : MonoBehaviour
{
    public float fill_perc;

    public UnitSelector UnitSelector;
    public EntityStats entityStats;

    public GameObject Fill;

    // Update is called once per frame
    void Update()
    {
        fill_perc = (float)entityStats.EXP / (float)entityStats.MaxEXP;
        Fill.GetComponent<Image>().fillAmount = fill_perc;
    }

    public void UpdateSelectedTarget()
    {
        entityStats = UnitSelector.SelectedUnit.GetComponent<EntityStats>();
    }
}
