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
    public GameObject Entities;
    public List<EntityPriority> EntityList;
    // Start is called before the first frame update
    void Start()
    {
        ObtainEntities();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Sorting");
            SortPriority();
        }
    }
    void ObtainEntities()
    {
        for (int i = 0; i < Entities.transform.childCount; i++)
        {
            EntityList.Add(new EntityPriority() { Entity = Entities.transform.GetChild(i).gameObject, Entity_AGI = Entities.transform.GetChild(i).GetComponent<EntityStats>().AGI });
        }
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
