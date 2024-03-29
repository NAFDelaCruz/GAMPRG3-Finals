﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelectedController : MonoBehaviour
{
    [Header("Set Components")]
    [HideInInspector]
    public TurnManager TurnManagerScript;
    UnitSelector UnitSelectorScript;

    [Header("Collections")]
    [HideInInspector]
    public Collider2D[] CollidedTiles;
    [HideInInspector]
    public List<GameObject> AvailableTiles;
    [HideInInspector]
    public List<GameObject> ValidTiles;
    [HideInInspector]
    public List<GameObject> AllSelectedTiles;

    [Header("Color Code")]
    Color MoveColor = Color.blue;
    Color SelectedMoveColor = Color.cyan;
    Color AttackColor = Color.magenta;
    Color SelectedAttackColor = new Color(221, 160, 221, 0.5f);

    [Header("Tracking Variables")]
    string TileAction;
    Transform LastTile;

    void Start()
    {
        UnitSelectorScript = GetComponent<UnitSelector>();
        TurnManagerScript = GameObject.Find("Game Manager").GetComponent<TurnManager>();
        TurnManagerScript.TurnEnds.AddListener(ResetTurn);
    }

    void Update()
    {
        if (UnitSelectorScript.SelectedUnit && UnitSelectorScript.SelectedUnitStats.AP == 0)
            DeselectUnit();

        if (Input.GetMouseButtonDown(0) && UnitSelectorScript.SceneChangerScript.IsInGame)
        {
            Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero);

            if (hit.collider && ValidTiles.Contains(hit.collider.gameObject))
            {
                AllSelectedTiles.Add(hit.collider.gameObject);
                UnitSelectorScript.SelectedUnitActionManager.SelectedTiles.Add(hit.collider.gameObject);
                UnitSelectorScript.SelectedUnitActionManager.SelectedTileActions.Add(TileAction);
                ValidTiles.Remove(hit.collider.gameObject);
                UnitSelectorScript.SelectedUnitStats.AP--;
                if (TileAction == "Move")
                {
                    AvailableTiles.Remove(hit.collider.gameObject);
                    ValidTiles.Clear();
                    GetMoveTiles();
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = SelectedMoveColor;
                }
                else if (TileAction == "Attack")
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = SelectedAttackColor;
            }
        }
    }

    void ResetTurn()
    {
        UnmarkTiles(AllSelectedTiles);
        ValidTiles.Clear();
        AvailableTiles.Clear();
    }

    public void UndoAction()
    {
        if(UnitSelectorScript.SelectedUnitActionManager.SelectedTileActions.Count > 0)
        {
            string LastAction = UnitSelectorScript.SelectedUnitActionManager.SelectedTileActions[UnitSelectorScript.SelectedUnitActionManager.SelectedTileActions.Count - 1];
            GameObject LastTile = UnitSelectorScript.SelectedUnitActionManager.SelectedTiles[UnitSelectorScript.SelectedUnitActionManager.SelectedTiles.Count - 1];

            if (LastAction == "Rest")
                UnitSelectorScript.SelectedUnitStats.AP += 2;
            else
            {
                UnitSelectorScript.SelectedUnitStats.AP++;
                LastTile.GetComponent<SpriteRenderer>().color = Color.white;
                UnitSelectorScript.SelectedUnitActionManager.SelectedTiles.Remove(LastTile);
                UnitSelectorScript.SelectedUnitActionManager.SelectedTileActions.Remove(LastAction);
            }
        }
    }

    public void Rest()
    {
        if (UnitSelectorScript.SelectedUnitStats.AP >= 2)
        {
            UnitSelectorScript.SelectedUnitStats.AP -= 2;
            UnitSelectorScript.SelectedUnitActionManager.SelectedTileActions.Add("Rest");
            UnitSelectorScript.SelectedUnitActionManager.SelectedTiles.Add(UnitSelectorScript.SelectedUnit.transform.parent.gameObject);
        }
    }

    public void MarkMoveTiles()
    {
        if (UnitSelectorScript.SelectedUnitStats.AP > 0)
        {
            UnmarkTiles(ValidTiles);
            ValidTiles.Clear();
            UnmarkTiles(AvailableTiles);
            AvailableTiles.Clear();
            LastTile = UnitSelectorScript.SelectedUnitActionManager.GetLastMoveTile(UnitSelectorScript.SelectedUnit.transform);
            GetTiles(LastTile, UnitSelectorScript.SelectedUnitStats.AP * 2, AvailableTiles);
            MarkTiles(MoveColor, AvailableTiles);
            GetMoveTiles();
        }
    }

    public void GetMoveTiles()
    {
        if (UnitSelectorScript.SelectedUnitStats.AP > 0)
        {
            TileAction = "Move";
            UnmarkTiles(ValidTiles);
            ValidTiles.Clear();
            LastTile = UnitSelectorScript.SelectedUnitActionManager.GetLastMoveTile(UnitSelectorScript.SelectedUnit.transform);
            GetTiles(LastTile, 1, ValidTiles);
        }
    }

    public void GetAttackTiles()
    {
        TileAction = "Attack";
        UnmarkTiles(AvailableTiles);
        ValidTiles.Clear();
        AvailableTiles.Clear();

        if (UnitSelectorScript.SelectedUnitStats.AP > 0)
        {
            LastTile = UnitSelectorScript.SelectedUnitActionManager.GetLastMoveTile(UnitSelectorScript.SelectedUnit.transform);
            GetTiles(LastTile, UnitSelectorScript.SelectedUnitStats.AttackRange * 2, ValidTiles);
            MarkTiles(AttackColor, ValidTiles);
        }
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

    public void DeselectUnit()
    {
        if (TileAction == "Move")
            UnmarkTiles(AvailableTiles);
        else if (TileAction == "Attack")
            UnmarkTiles(ValidTiles);

        ValidTiles.Clear();
        AvailableTiles.Clear();
    }
}
