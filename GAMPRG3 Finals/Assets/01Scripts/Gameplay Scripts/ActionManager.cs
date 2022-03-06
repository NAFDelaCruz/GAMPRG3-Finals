using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Set Components")]
    [HideInInspector]
    public EntityStats ThisUnitStats;

    [Header("Collections")]
    [HideInInspector]
    public List<GameObject> SelectedTiles;
    [HideInInspector]
    public List<string> SelectedTileActions;

    private void Start()
    {
        ThisUnitStats = GetComponent<EntityStats>();
        ResetAP(); 
    }

    public virtual Transform GetLastMoveTile(Transform GameObjectTransform)
    {
        Transform TileTransform = null;

        if (!SelectedTileActions.Contains("Move"))
            TileTransform = GameObjectTransform.parent;
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

    public void ResetAP()
    {
        ThisUnitStats.AP = ThisUnitStats.Max_AP;
        SelectedTiles.Clear();
        SelectedTileActions.Clear();
    }
}
