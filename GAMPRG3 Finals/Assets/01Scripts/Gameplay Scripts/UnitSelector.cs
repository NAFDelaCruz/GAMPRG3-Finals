using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSelector : MonoBehaviour
{
    public GameObject SelectedUnit;
    public ActionManager SelectedUnitActionManager;
    public EntityStats SelectedUnitStats;
    public SceneChanger SceneChangerScript;
    public UnitSelectedController UnitSelectedControllerScript;
    public TurnManager TurnManagerScript;
    public RaycastHit hit;
    [HideInInspector]
    public bool SwitchedUnits;

    private void Start()
    {
        TurnManagerScript = GetComponent<TurnManager>();
        SceneChangerScript = GetComponent<SceneChanger>();
        TurnManagerScript.TurnStarts.AddListener(StartTurn);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && TurnManagerScript._isTurnNotRunning && SceneChangerScript.IsInGame)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider && SelectedUnit && SelectedUnit == hit.collider.gameObject)
                {
                    StartCoroutine(SwitchedUnit());
                    SelectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
                    SelectedUnit = null;
                    UnitSelectedControllerScript.DeselectUnit();
                }
                else if (hit.collider && hit.collider.CompareTag("FriendlyUnit"))
                {
                    StartCoroutine(SwitchedUnit());
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

        if (Input.GetKey(KeyCode.F))
            Time.timeScale = 2f;
        else
            Time.timeScale = 1f;
    }

    IEnumerator SwitchedUnit()
    {
        SwitchedUnits = true;
        yield return new WaitForSeconds(0.2f);
        SwitchedUnits = false;
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
