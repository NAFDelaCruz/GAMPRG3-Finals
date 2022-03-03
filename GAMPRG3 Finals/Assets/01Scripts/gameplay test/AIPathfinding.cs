using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class AIPathfinding : ActionManager
{
    [Header("Set Components")]
    public TurnManager TurnManagerScript;
    EntityStats _stats;

    [Header("Collections")]
    [HideInInspector]
    public Collider[] _objects;
    [HideInInspector]
    public List<GameObject> _units;
    [HideInInspector]
    public Collider2D[] AvailableTiles;

    [Header("Entity Stats")]
    public int AttackRange;
    public string Target;

    [Header("Tracking Variables")]
    float _distanceFromTarget;
    int _currentTarget = 0;

    void Start()
    {
        TurnManagerScript = GameObject.Find("Game Manager").GetComponent<TurnManager>();
        TurnManagerScript.TurnEnds.AddListener(ResetTurn);
        _stats = gameObject.GetComponent<EntityStats>();
        StartPathfinding();
    }

    void ResetTurn()
    {
        _currentTarget = 0;
        SelectedTiles.Clear();
        SelectedTileActions.Clear();
        _units.Clear();
        StartPathfinding();
    }

    void StartPathfinding()
    {
        GetTargets();

        if (_units.Count > 0)
        {
            GetPath();
        }
    }

    void GetPath()
    {
        for (int Count = 0; Count < _stats.Max_AP; Count++)
        {
            LastTile = GetLastMoveTile();

            if (_units.Count != 0)
                _distanceFromTarget = Vector2.Distance(GetLastMoveTile().transform.position, _units[_currentTarget].transform.position);

            if (_distanceFromTarget < AttackRange + 0.5f)
            {
                SelectedTiles.Add(_units[_currentTarget].transform.parent.gameObject);
                _currentTarget++;
                SelectedTileActions.Add("Attack");
            }
            else if (_distanceFromTarget > AttackRange + 0.5f)
            {
                SelectedTiles.Add(GetTiles());
                SelectedTileActions.Add("Move");
            }
        }
    }

    GameObject GetTiles()
    {
        GameObject Tile = null;
        float X = GetDirection(LastTile.position.x, _units[_currentTarget].transform.parent.position.x);
        float Y = GetDirection(LastTile.position.y, _units[_currentTarget].transform.parent.position.y);
        Vector2 dir = _units[_currentTarget].transform.position - transform.position;
        float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (_angle < 0) _angle += 360f;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(LastTile.position.x + X, LastTile.position.y + Y), Vector2.zero);

        if (!hit.collider.gameObject.GetComponent<Tiles>().IsObstacle && hit.collider.gameObject.transform.childCount == 0)
            Tile = hit.collider.gameObject;
        else if (hit.collider.gameObject.GetComponent<Tiles>().IsObstacle || hit.collider.gameObject.transform.childCount > 0)
        {
            if (_angle < 44.5f || _angle > 314.5f)
                Tile = GetValidTiles(new Vector2(LastTile.position.x, LastTile.position.y + 1f), new Vector2(LastTile.position.x + 1f, LastTile.position.y - 1f));
            else if (_angle < 134.5f && _angle > 44.5f)
                Tile = GetValidTiles(new Vector2(LastTile.position.x - 1f, LastTile.position.y), new Vector2(LastTile.position.x + 1f, LastTile.position.y + 1f));
            else if (_angle < 224.5f && _angle > 134.5f)
                Tile = GetValidTiles(new Vector2(LastTile.position.x, LastTile.position.y + 1f), new Vector2(LastTile.position.x - 1f, LastTile.position.y - 1f));
            else if (_angle < 314.5f && _angle > 224.5f)
                Tile = GetValidTiles(new Vector2(LastTile.position.x - 1f, LastTile.position.y), new Vector2(LastTile.position.x + 1f, LastTile.position.y - 1f));
        }

        return Tile;
    }

    GameObject GetValidTiles(Vector2 EnemyTilePos, Vector2 EndArea)
    {
        Physics2D.SyncTransforms();
        GameObject NewTile = null;

        if (AvailableTiles.Length != 0)
            Array.Clear(AvailableTiles, 0, AvailableTiles.Length);
        AvailableTiles = Physics2D.OverlapAreaAll(EnemyTilePos, EndArea);

        foreach (Collider2D PossibleTile in AvailableTiles)
        {
            if (!PossibleTile.gameObject.GetComponent<Tiles>().IsObstacle && PossibleTile.gameObject.transform.childCount == 0 && PossibleTile.name != LastTile.name)
            {
                NewTile = PossibleTile.gameObject;
                break;
            }
        }

        return NewTile;
    }

    float GetDirection(float LocalCoord, float TargetCoord)
    {
        if (LocalCoord < TargetCoord) return 1;
        else if (LocalCoord == TargetCoord) return 0;
        else return -1;
    }

    void GetTargets()
    {
        _objects = Physics.OverlapSphere(transform.position, _stats.Max_AP + (AttackRange - 1));

        foreach (Collider Object in _objects)
        {
            if (Object.gameObject.CompareTag(Target))
            _units.Add(Object.gameObject);
        }
        
        _units.Sort(SortByHP);
    }

    static int SortByHP(GameObject x, GameObject y)
    {
        return x.GetComponent<EntityStats>().HP.CompareTo(y.GetComponent<EntityStats>().HP);
    }
}
