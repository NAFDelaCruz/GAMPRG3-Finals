using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitActionController : MonoBehaviour
{
    public float AttackRange;
    public int ActionPoints;

    int CurrentActionPoints;
    int MoveIndex = -1;
    string TileAction;
    Color MoveColor = Color.blue;
    Color AttackColor = Color.yellow;


    public Collider2D[] CollidedTiles;
    public List<GameObject> ValidTiles;
    public List<GameObject> SelectedTiles;
    public List<string> SelectedTileActions;

    private void Start()
    {
        ResetAP();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero);

            if (ValidTiles.Contains(hit.collider.gameObject))
            {
                SelectedTiles.Add(hit.collider.gameObject);
                SelectedTileActions.Add(TileAction);
                if (TileAction == "Move")
                {
                    MoveIndex++;
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = MoveColor;
                }
                else if (TileAction == "Attack")
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = AttackColor;
                ValidTiles.Remove(hit.collider.gameObject);
                UnmarkTiles();
                ValidTiles.Clear();
                CurrentActionPoints--;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GetMoveTiles();
            TileAction = "Move";
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GetAttackTiles();
            TileAction = "Attack";
        }
    }

    void ResetAP()
    {
        CurrentActionPoints = ActionPoints;
        MoveIndex = -1;
    }

    public void GetMoveTiles()
    {
        if (CurrentActionPoints > 0)
        {
            if (MoveIndex >= 0)
            {
                GetTiles(SelectedTiles[MoveIndex].transform, 1);
                MarkTiles(MoveColor);
            }
            else if (MoveIndex < 0)
            {
                GetTiles(gameObject.transform, 1);
                MarkTiles(MoveColor);
            }
        }
    }

    public void GetAttackTiles()
    {
        if (CurrentActionPoints > 0)
        {
            if (MoveIndex >= 0)
            {
                GetTiles(SelectedTiles[MoveIndex].transform, AttackRange);
                MarkTiles(AttackColor);
            }
            else if (MoveIndex < 0)
            {
                GetTiles(gameObject.transform, AttackRange);
                MarkTiles(AttackColor);
            }  
        }
    }

    public void MarkTiles(Color TileColor)
    {
        foreach (GameObject tile in ValidTiles)
        {
            if (!tile.GetComponent<Tiles>().IsObstacle)
                tile.GetComponent<SpriteRenderer>().color = TileColor;
        }

    }

    public void UnmarkTiles()
    {
        foreach (GameObject tile in ValidTiles)
        {
            if (!tile.GetComponent<Tiles>().IsObstacle)
                tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public virtual void GetTiles(Transform Tile, float Range)
    {
        if (CollidedTiles.Length != 0)
            Array.Clear(CollidedTiles, 0, CollidedTiles.Length);

        CollidedTiles = Physics2D.OverlapBoxAll(new Vector2(Tile.position.x, Tile.position.y), new Vector2(Range, Range), 0);

        foreach (Collider2D tile in CollidedTiles)
        {
            if (!tile.gameObject.GetComponent<Tiles>().IsObstacle && tile.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                ValidTiles.Add(tile.gameObject);
        }
    }
}
