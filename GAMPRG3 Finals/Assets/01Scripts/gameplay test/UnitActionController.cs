using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UnitActionController : MonoBehaviour
{
    public float AttackRange;
    public int ActionPoints;
    private int CurrentActionPoints;
    Color MoveColor = Color.green;
    Color AttackColor = Color.red;

    public Collider2D[] PossibleTiles;
    public List<GameObject> SelectedTiles;
    Dictionary<string, GameObject> TilePath;

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

            if (PossibleTiles.Contains(hit.collider))
            {
                SelectedTiles.Add(hit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GetMoveTiles();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GetAttackTiles();
        }
    }

    void ResetAP()
    {
        CurrentActionPoints = ActionPoints;
    }

    public void GetMoveTiles()
    {
        if (CurrentActionPoints > 0)
        {
            Debug.Log("Getting Move Tiles");
            GetTiles(gameObject.transform, 1);
        }
    }

    public void GetAttackTiles()
    {
        if (CurrentActionPoints > 0)
        {

            Debug.Log("Getting Move Tiles");
            GetTiles(gameObject.transform, AttackRange);
            MarkTiles(AttackColor);
        }
    }

    public void MarkTiles(Color TileColor)
    {
        foreach (Collider2D tile in PossibleTiles)
        {
            tile.gameObject.GetComponent<SpriteRenderer>().color = TileColor;
        }

    }

    public void UnmarkTiles()
    {
        foreach (Collider2D tile in PossibleTiles)
        {
            tile.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }

    public virtual void GetTiles(Transform Tile, float Range)
    {
        if (PossibleTiles.Length != 0)
            Array.Clear(PossibleTiles, 0, PossibleTiles.Length);

        PossibleTiles = Physics2D.OverlapBoxAll(new Vector2(Tile.position.x, Tile.position.y), new Vector2(Range, Range), 0);
    }
}
