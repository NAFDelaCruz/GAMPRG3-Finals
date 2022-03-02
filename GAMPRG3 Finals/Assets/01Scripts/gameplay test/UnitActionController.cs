using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionController : MonoBehaviour
{
    [Header("Entity Stats")]
    public float AttackRange;
    public int ActionPoints;

    [Header("Collections")]
    [HideInInspector]
    public Collider2D[] CollidedTiles;
    [HideInInspector]
    public List<GameObject> AvailableTiles;
    [HideInInspector]
    public List<GameObject> ValidTiles;
    public List<GameObject> SelectedTiles;
    public List<string> SelectedTileActions;

    [Header("Color Code")]
    Color MoveColor = Color.blue;
    Color SelectedMoveColor = Color.cyan;
    Color AttackColor = Color.magenta;
    Color SelectedAttackColor = new Color(221, 160, 221, 0.5f);

    [Header("Tracking Variables")]
    int CurrentActionPoints;
    string TileAction;

    void Start()
    {
        ResetTurn();
    }

    void ResetTurn()
    {
        CurrentActionPoints = ActionPoints;
        SelectedTiles.Clear();
        SelectedTileActions.Clear();
    }

    private void Update()
    {
        if (CurrentActionPoints == 0)
            if (TileAction == "Move")
                UnmarkTiles(AvailableTiles);
            else if (TileAction == "Attack")
                UnmarkTiles(ValidTiles);

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero);

            if (ValidTiles.Contains(hit.collider.gameObject))
            {
                SelectedTiles.Add(hit.collider.gameObject);
                SelectedTileActions.Add(TileAction);
                ValidTiles.Remove(hit.collider.gameObject);
                CurrentActionPoints--;
                if (TileAction == "Move")
                {
                    AvailableTiles.Remove(hit.collider.gameObject);
                    ValidTiles.Clear();
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = SelectedMoveColor;
                    GetMoveTiles();
                }
                else if (TileAction == "Attack")
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = SelectedAttackColor;
            }
        }
    }

    public void GetMoveTiles()
    {
        TileAction = "Move";
        UnmarkTiles(ValidTiles);
        ValidTiles.Clear();
        Transform CenterTile = GetLastMoveTile();
        GetTiles(CenterTile, CurrentActionPoints * 2, AvailableTiles);
        MarkTiles(MoveColor, AvailableTiles);

        if (CurrentActionPoints > 0)
        {
            CenterTile = GetLastMoveTile();
            GetTiles(CenterTile, 1, ValidTiles);
        }
    }

    public void GetAttackTiles()
    {
        TileAction = "Attack";
        UnmarkTiles(AvailableTiles);
        ValidTiles.Clear();
        AvailableTiles.Clear();

        if (CurrentActionPoints > 0)
        {
            Transform CenterTile = GetLastMoveTile();
            GetTiles(CenterTile, AttackRange*2, ValidTiles);
            MarkTiles(AttackColor, ValidTiles);
        }
    }

    public virtual Transform GetLastMoveTile()
    {
        Transform TileTransform = null;

        if (!SelectedTileActions.Contains("Move"))
        {
            TileTransform = gameObject.transform;
        }
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

    public void MarkTiles(Color TileColor, List<GameObject> ChosenList)
    {
        foreach (GameObject tile in ChosenList)
        {
            if (!tile.GetComponent<Tiles>().IsObstacle)
                tile.GetComponent<SpriteRenderer>().color = TileColor;
        }

    }

    public void UnmarkTiles(List<GameObject> ChosenList)
    {
        foreach (GameObject tile in ChosenList)
        {
            if (!tile.GetComponent<Tiles>().IsObstacle)
                tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public virtual void GetTiles(Transform Tile, float Range, List<GameObject> ChosenList)
    {
        if (CollidedTiles.Length != 0)
            Array.Clear(CollidedTiles, 0, CollidedTiles.Length);

        CollidedTiles = Physics2D.OverlapBoxAll(new Vector2(Tile.position.x, Tile.position.y), new Vector2(Range, Range), 0);
        
        foreach (Collider2D tile in CollidedTiles)
        {
            bool HasFriendly = new bool();

            if (tile.transform.childCount > 0)
                HasFriendly = tile.gameObject.transform.GetChild(0).CompareTag("FriendlyUnit");

            if (TileAction == "Move" && !tile.gameObject.GetComponent<Tiles>().IsObstacle && tile.transform.childCount == 0 && (tile.gameObject.GetComponent<SpriteRenderer>().color == Color.white || tile.gameObject.GetComponent<SpriteRenderer>().color == MoveColor))
                ChosenList.Add(tile.gameObject);
            else if (TileAction == "Attack" && !tile.gameObject.GetComponent<Tiles>().IsObstacle && !HasFriendly && (tile.gameObject.GetComponent<SpriteRenderer>().color == Color.white || tile.gameObject.GetComponent<SpriteRenderer>().color == AttackColor))
                ChosenList.Add(tile.gameObject);
        }
    }
}
