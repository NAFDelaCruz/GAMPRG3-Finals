using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarScript : MonoBehaviour
{
    public float fill_perc;

    public UnitSelector UnitSelector;
    public EntityStats entityStats;

    public GameObject Fill;

    // Update is called once per frame
    void Update()
    {
        if (entityStats != null)
        {
            fill_perc = (float)entityStats.Curr_HP / (float)entityStats.HP;
            Fill.GetComponent<Image>().fillAmount = fill_perc;
        }
    }

    public void UpdateSelectedTarget()
    {
        entityStats = UnitSelector.SelectedUnit.GetComponent<EntityStats>();
    }
}
