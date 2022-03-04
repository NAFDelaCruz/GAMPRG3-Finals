using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public GameObject SelectedUnit;
    public ActionManager SelectedUnitActionManager;
    public EntityStats SelectedUnitStats;
    public UnitSelectedController UnitSelectedControllerScript;
    public RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider && SelectedUnit && SelectedUnit == hit.collider.gameObject)
                {
                    SelectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
                    SelectedUnit = null;
                    UnitSelectedControllerScript.DeselectUnit();
                }
                else if (hit.collider && hit.collider.CompareTag("FriendlyUnit"))
                {
                    UnitSelectedControllerScript.DeselectUnit();
                    if (SelectedUnit) SelectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
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
