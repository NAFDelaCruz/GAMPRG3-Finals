using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfinding : MonoBehaviour
{
    [Header("Collections")]
    public List<GameObject> SelectedTiles;
    public List<string> SelectedTileActions;

    [Header("Entity Stats")]
    public int ActionPoints;
    public int AttackRange;
    public string Target;

    [Header("Target Variables")]
    [HideInInspector]
    public Collider2D[] _objects;
    //[HideInInspector]
    public List<GameObject> _units;
    float _distanceFromTarget;
    float _angle;

    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GetTargetsAndVariables();
            GetPath();
        }
    }

    void GetPath()
    {
        for (int Count = 0; Count < ActionPoints; Count++)
        {
            if (_units.Count != 0)
                _distanceFromTarget = Vector2.Distance(GetLastMoveTile().transform.position, _units[0].transform.position);

            if (_distanceFromTarget < AttackRange + 0.5f)
            {
                SelectedTiles.Add(_units[0].transform.parent.gameObject);
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
        GameObject Tile = new GameObject();
        Transform LastTile = GetLastMoveTile();
        float X = GetXDirection(LastTile.position.x, _units[0].transform.parent.position.x);
        float Y = GetYDirection(LastTile.position.y, _units[0].transform.parent.position.y);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(LastTile.position.x + X, LastTile.position.y + Y), Vector2.zero);

        if (!hit.collider.gameObject.GetComponent<Tiles>().IsObstacle && hit.collider.gameObject.transform.childCount == 0)
            Tile = hit.collider.gameObject;
        else if (hit.collider.gameObject.GetComponent<Tiles>().IsObstacle || hit.collider.gameObject.transform.childCount > 0)
        {
            if (_angle >= 0 && _angle <= 90 && Y == 1)
                Tile = GetValidTiles(gameObject.transform.position, new Vector2(LastTile.position.x + 1, LastTile.position.y + 1));
            else if (_angle >= 90 && _angle <= 180 && Y == 1)
                Tile = GetValidTiles(gameObject.transform.position, new Vector2(LastTile.position.x - 1, LastTile.position.y + 1));
            else if (_angle <= 0 && _angle >= -90 && Y == -1)
                Tile = GetValidTiles(gameObject.transform.position, new Vector2(LastTile.position.x + 1, LastTile.position.y - 1));
            else if (_angle <= -90 && _angle >= -180 && Y == -1)
                Tile = GetValidTiles(gameObject.transform.position, new Vector2(LastTile.position.x - 1, LastTile.position.y - 1));
        }

        return Tile;
    }

    GameObject GetValidTiles(Vector2 EnemyTilePos, Vector2 EndArea)
    {
        GameObject NewTile = new GameObject();

        Collider2D[] AvailableTiles = Physics2D.OverlapAreaAll(EnemyTilePos, EndArea);

        foreach (Collider2D PossibleTile in AvailableTiles)
        {
            if (PossibleTile.gameObject.GetComponent<Tiles>().IsObstacle && PossibleTile.gameObject.transform.childCount == 0)
            {
                NewTile = PossibleTile.gameObject;
                break;
            }
        }

        return NewTile;
    }

    public virtual Transform GetLastMoveTile()
    {
        Transform TileTransform = null;

        if (!SelectedTileActions.Contains("Move"))
            TileTransform = gameObject.transform.parent;
        else if (SelectedTileActions.Contains("Move"))
        {
            for (int LastIndex = SelectedTileActions.Count - 1; LastIndex >= 0; LastIndex--)
            {
                if (SelectedTileActions[LastIndex] == "Move")
                {
                    TileTransform = SelectedTiles[LastIndex].transform;
                    break;
                }
            }
        }

        return TileTransform;
    }

    float GetXDirection(float LocalXCoord, float TargetXCoord)
    {
        if (LocalXCoord < TargetXCoord)
            return 1;
        else if (LocalXCoord == TargetXCoord)
            return 0;
        else
            return -1;
    }

    float GetYDirection(float LocalYCoord, float TargetYCoord)
    {
        if (LocalYCoord < TargetYCoord)
            return 1;
        else if (LocalYCoord == TargetYCoord)
            return 0;
        else
            return -1;
    }

    void GetTargetsAndVariables()
    {
        _objects = Physics2D.OverlapCircleAll(transform.position, ActionPoints + (AttackRange - 1));

        foreach (Collider2D Object in _objects)
        {
            if (Object.gameObject.CompareTag("FriendlyUnit"))
            _units.Add(Object.gameObject);
        }

        _units.Sort(SortByHP);

        Vector2 dir = _units[0].transform.position - transform.position;
        _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    static int SortByHP(GameObject x, GameObject y)
    {
        return x.GetComponent<EntityStats>().HP.CompareTo(y.GetComponent<EntityStats>().HP);
    }
}
