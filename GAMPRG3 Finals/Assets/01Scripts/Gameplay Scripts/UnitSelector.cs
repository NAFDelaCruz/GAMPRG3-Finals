using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSelector : MonoBehaviour
{
    public GameObject SelectedUnit;
    public ActionManager SelectedUnitActionManager;
    public EntityStats SelectedUnitStats;
    public UnitSelectedController UnitSelectedControllerScript;
    public TurnManager TurnManagerScript;
    public RaycastHit hit;

    private void Start()
    {
        TurnManagerScript = GetComponent<TurnManager>();
        TurnManagerScript.TurnStarts.AddListener(StartTurn);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && TurnManagerScript._isTurnNotRunning)
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
                    UnitSelectedControllerScript.TurnManagerScript.TurnEnds.AddListener(SelectedUnitActionManager.ResetAP);
                }
            }
        }
    }
    
    public void StartTurn()
    {
        if (SelectedUnit)
        {
            SelectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
            SelectedUnit = null;
            UnitSelectedControllerScript.DeselectUnit();
        }
    }
}   
