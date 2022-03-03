using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public GameObject SelectedUnit;
    public ActionManager SelectedUnitActionManager;
    public EntityStats SelectedUnitStats;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            { 
                if (SelectedUnit != null && (hit.collider.gameObject == SelectedUnit || hit.collider.gameObject != SelectedUnit))
                {
                    SelectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
                    SelectedUnit = null;
                }

                if (hit.collider.CompareTag("FriendlyUnit") && hit.collider != null)
                {
                    SelectedUnit = hit.collider.gameObject;
                    SelectedUnit.GetComponent<SpriteRenderer>().color = Color.green;
                    SelectedUnitActionManager = SelectedUnit.GetComponent<ActionManager>();
                    SelectedUnitStats = SelectedUnit.GetComponent<EntityStats>();
                }

                if (!SelectedUnitActionManager.HasAP)
                {
                    SelectedUnitStats.AP = SelectedUnitStats.Max_AP;
                    SelectedUnitActionManager.HasAP = true;
                }
            }
        }
    }
}   
