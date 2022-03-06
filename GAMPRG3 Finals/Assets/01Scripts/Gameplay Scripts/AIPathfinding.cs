using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIPathfinding : MonoBehaviour
{
    [Header("Set Components")]
    public TurnManager TurnManagerScript;
    public ActionManager ActionManagerScript;
    EntityStats _stats;

    [Header("Collections")]
    [HideInInspector]
    public Collider[] _objects;
    [HideInInspector]
    public List<GameObject> _units;
    [HideInInspector]
    public Collider2D[] AvailableTiles;
    [HideInInspector]
    public Collider2D[] WanderAvailableTiles;

    [Header("Entity Stats")]
    public string Target;

    [Header("Tracking Variables")]
    float _distanceFromTarget;
    int _currentTarget = 0;
    Transform LastTile;

    void Start()
    {
        TurnManagerScript = GameObject.Find("Game Manager").GetComponent<TurnManager>();
        TurnManagerScript.TurnStarts.AddListener(StartPathfinding);
        TurnManagerScript.TurnEnds.AddListener(ResetTurn);
        ActionManagerScript = GetComponent<ActionManager>();
        _stats = gameObject.GetComponent<EntityStats>();
    }

    void ResetTurn()
    {
        _currentTarget = 0;
        ActionManagerScript.SelectedTiles.Clear();
        ActionManagerScript.SelectedTileActions.Clear();
        _units.Clear();
    }

    void StartPathfinding()
    {
        GetTargets();

        if (_units.Count > 0)
        {
            GetPath();
        }
        else if (_units.Count == 0)
        {
            Wander();
        }
    }

    void Wander()
    {
        if (WanderAvailableTiles.Length != 0)
            Array.Clear(WanderAvailableTiles, 0, WanderAvailableTiles.Length);
        WanderAvailableTiles = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(_stats.Max_AP, _stats.Max_AP), 0);

        int RandomTile = UnityEngine.Random.Range(0, WanderAvailableTiles.Length - 1);
        int MaxCount = Mathf.RoundToInt(Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.y), WanderAvailableTiles[RandomTile].transform.position));

        for (int Count = 0; Count < MaxCount; Count++)
        {
            LastTile = ActionManagerScript.GetLastMoveTile(gameObject.transform);
            ActionManagerScript.SelectedTiles.Add(GetTiles(WanderAvailableTiles[RandomTile].gameObject));
            ActionManagerScript.SelectedTileActions.Add("Move");
        }
    }

    void GetPath()
    {
        for (int Count = 0; Count < _stats.Max_AP; Count++)
        {
            LastTile = ActionManagerScript.GetLastMoveTile(gameObject.transform);

            if (_units.Count != 0)
                _distanceFromTarget = Vector2.Distance(ActionManagerScript.GetLastMoveTile(gameObject.transform).position, _units[_currentTarget].transform.position);

            if (_distanceFromTarget < ActionManagerScript.ThisUnitStats.AttackRange + 0.5f)
            {
                ActionManagerScript.SelectedTiles.Add(_units[_currentTarget].transform.parent.gameObject);
                _currentTarget++;
                ActionManagerScript.SelectedTileActions.Add("Attack");
            }
            else if (_distanceFromTarget > ActionManagerScript.ThisUnitStats.AttackRange + 0.5f)
            {
                ActionManagerScript.SelectedTiles.Add(GetTiles(_units[_currentTarget].transform.parent.gameObject));
                ActionManagerScript.SelectedTileActions.Add("Move");
            }
        }
    }

    GameObject GetTiles(GameObject Target)
    {
        GameObject Tile = null;
        float X = GetDirection(LastTile.position.x, Target.transform.position.x);
        float Y = GetDirection(LastTile.position.y, Target.transform.position.y);
        Vector2 dir = Target.transform.position - transform.position;
        float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (_angle < 0) _angle += 360f;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(LastTile.position.x + X, LastTile.position.y + Y), Vector2.zero);

        if (!hit.collider.gameObject.GetComponent<Tiles>().IsObstacle && hit.collider.gameObject.transform.childCount == 0 && hit.collider.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
            Tile = hit.collider.gameObject;
        else if (hit.collider.gameObject.GetComponent<Tiles>().IsObstacle || hit.collider.gameObject.transform.childCount > 0 || hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
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
        _objects = Physics.OverlapSphere(transform.position, _stats.Max_AP + (ActionManagerScript.ThisUnitStats.AttackRange - 1));

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
