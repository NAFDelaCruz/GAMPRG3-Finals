using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfinding : MonoBehaviour
{
    public int ActionPoints;
    public int AttackRange;
    public Collider[] Objects;
    public List<GameObject> Units; 

    void Start()
    {
        LayerMask mask = LayerMask.GetMask("Friendly Units");
        Objects = Physics.OverlapSphere(gameObject.transform.position, ActionPoints + AttackRange, mask);
        foreach (var Object in Objects)
        {
            Units.Add(Object.gameObject);
        }

        Sort();
    }

    void Sort()
    {
        Units.Sort(SortByHP);
    }

    static int SortByHP(GameObject x, GameObject y)
    {
        return x.GetComponent<EntityStats>().HP.CompareTo(y.GetComponent<EntityStats>().HP);
    }
}
