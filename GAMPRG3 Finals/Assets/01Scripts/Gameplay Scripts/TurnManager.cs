using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EntityPriority
{
    public GameObject Entity;
    public int Entity_AGI;
}

public class TurnManager : MonoBehaviour
{
    public GameObject Map;
    public List<EntityPriority> EntityList;
    public ActionManager Actions;
    public EntityStats Stats;
    public CombatScript Combat;
    public UnityEvent TurnStarts;
    public UnityEvent TurnEnds;
    public bool _isTurnNotRunning = true;
    Vector2 LastPos;
    float Distance;
    float X, Y;
    int EntityIndex;
    bool _isMoving = false;
    
    public void StartTurn()
    {
        Combat = GetComponent<CombatScript>();
        ObtainEntities();
        StartCoroutine(SlowLoop());
    }

    IEnumerator SlowLoop()
    {
        TurnStarts.Invoke();
        _isTurnNotRunning = false;

        yield return new WaitForSeconds(2f);

        for (int index1 = 0; index1 < EntityList.Count; index1++)
        {
            EntityIndex = index1;
            Actions = EntityList[index1].Entity.GetComponent<ActionManager>();
            Stats = EntityList[index1].Entity.GetComponent<EntityStats>();

            yield return StartCoroutine(DoUnitAction());
        }

        _isTurnNotRunning = true;
        TurnEnds.Invoke();
    }

    IEnumerator DoUnitAction()
    {
        for (int index2 = 0; index2 < Actions.SelectedTiles.Count; index2++)
        {
            if (Actions.SelectedTileActions[index2] == "Rest")
            {
                int UnitMaxHP = Stats.HP;
                int AmountToHeal = Mathf.FloorToInt(UnitMaxHP * 0.1f);
                if (AmountToHeal == 0) AmountToHeal = 1;
                Stats.Curr_HP += AmountToHeal;
                if (Stats.Curr_HP > UnitMaxHP) Stats.Curr_HP = UnitMaxHP;
                yield return new WaitForSeconds(1f);
            }
            else if (Actions.SelectedTileActions[index2] == "Move")
            {
                X = GetDirection(EntityList[EntityIndex].Entity.transform.parent.position.x, Actions.SelectedTiles[index2].transform.position.x);
                Y = GetDirection(EntityList[EntityIndex].Entity.transform.parent.position.y, Actions.SelectedTiles[index2].transform.position.y);

                LastPos = EntityList[EntityIndex].Entity.transform.position;
                _isMoving = true;
                yield return new WaitForSeconds(0.2f);
                _isMoving = false;
                EntityList[EntityIndex].Entity.transform.parent = Actions.SelectedTiles[index2].transform;
                EntityList[EntityIndex].Entity.transform.localPosition = new Vector3(0, 0, 0);
                yield return new WaitForSeconds(0.2f);
            }
            else if (Actions.SelectedTileActions[index2] == "Attack")
            {
                EntityStats Target = new EntityStats();
                Combat.SetModifiers(Stats.IsPhysical);
                if (Actions.SelectedTiles[index2].transform.childCount > 0)
                    Target = Actions.SelectedTiles[index2].GetComponentInChildren<EntityStats>();
                else
                    break;

                if (Stats.IsPhysical)
                    Target.Curr_HP -= Combat.DamageCalc(Stats.STR, Target.DEF);
                else if (!Stats.IsPhysical)
                    Target.Curr_HP -= Combat.DamageCalc(Stats.INT, Target.CON);
            }
        }

       
    }

    public void Update()
    {
        if (_isMoving)
            EntityList[EntityIndex].Entity.transform.Translate(new Vector2(X, Y) * 5f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
            StartTurn();
    }

    float GetDirection(float LocalCoord, float TargetCoord)
    {
        if (LocalCoord < TargetCoord) return 1;
        else if (LocalCoord == TargetCoord) return 0;
        else return -1;
    }


    void ObtainEntities()
    {
        EntityList.Clear();

        for (int i = 0; i < Map.transform.childCount; i++)
        {
            GameObject currentTile = Map.transform.GetChild(i).gameObject;

            if (currentTile.transform.childCount > 0) 
                EntityList.Add(new EntityPriority() { Entity = currentTile.transform.GetChild(0).gameObject, Entity_AGI = currentTile.transform.GetChild(0).GetComponent<EntityStats>().AGI });
        }

        SortPriority();
    }

    void SortPriority()
    {
        EntityList.Sort(SortByAGI);
    }

    static int SortByAGI(EntityPriority x, EntityPriority y)
    {
        return x.Entity_AGI.CompareTo(y.Entity_AGI);
    }
}