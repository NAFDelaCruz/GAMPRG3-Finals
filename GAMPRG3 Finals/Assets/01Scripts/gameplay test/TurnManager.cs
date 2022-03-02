using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (EntityList.Count == 0)
            ObtainEntities();
    }

    void ObtainEntities()
    {
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

    void StartTurn()
    {

    }
}
