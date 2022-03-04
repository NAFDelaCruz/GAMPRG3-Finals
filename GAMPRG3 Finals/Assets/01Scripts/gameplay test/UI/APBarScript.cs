using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APBarScript : MonoBehaviour
{
    public float fill_perc;

    public UnitSelector UnitSelector;
    public EntityStats entityStats;

    public GameObject Fill;

    // Update is called once per frame
    void Update()
    {
        fill_perc = (float)entityStats.AP / (float)entityStats.Max_AP;
        Fill.GetComponent<Image>().fillAmount = fill_perc;
    }

    public void UpdateSelectedTarget()
    {
        entityStats = UnitSelector.SelectedUnit.GetComponent<EntityStats>();
    }
}
