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

    void Update()
    {
        float Distance = Vector3.Distance(gameObject.transform.localPosition, Units[0].transform.localPosition);

        if (Input.GetKeyDown(KeyCode.Q) && Distance > AttackRange + 1.5f)
        {
            gameObject.transform.localPosition = GetTiles();
            Debug.Log(gameObject.transform.localPosition);
        }
    }

    void Sort()
    {
        Units.Sort(SortByHP);
    }

    static int SortByHP(GameObject x, GameObject y)
    {
        return x.GetComponent<EntityStats>().HP.CompareTo(y.GetComponent<EntityStats>().HP);
    }

    public virtual Vector3 GetTiles()
    {
        float X = gameObject.transform.localPosition.x; //Prevents the method from return X as 0 when the units are already on the same X coord
        float Y = gameObject.transform.localPosition.y; //Prevents the method from return Y as 0 when the units are already on the same Y coord

        if (gameObject.transform.localPosition.x < Units[0].transform.localPosition.x)
        {
            X = gameObject.transform.localPosition.x + 1;
            Debug.Log("added X");
        }
        else if (gameObject.transform.localPosition.x > Units[0].transform.localPosition.x)
        {
            X = gameObject.transform.localPosition.x - 1;
            Debug.Log("deducted X");
        }

        if (gameObject.transform.localPosition.y < Units[0].transform.localPosition.y)
        {
            Y = gameObject.transform.localPosition.y + 1;
        }
        else if (gameObject.transform.localPosition.y > Units[0].transform.localPosition.y)
        {
            Y = gameObject.transform.localPosition.y - 1;
        }

        return new Vector3(X, Y, 0);
    }
}
