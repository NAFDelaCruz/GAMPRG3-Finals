using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Set Components")]
    public GameObject ThisUnit;

    [Header("Collections")]
    [HideInInspector]
    public List<GameObject> SelectedTiles;
    [HideInInspector]
    public List<string> SelectedTileActions;

    [Header("Tracking Variables")]
    [HideInInspector]
    public bool HasAP;

    public virtual Transform GetLastMoveTile(Transform GameObjectTransform)
    {
        Transform TileTransform = null;

        if (!SelectedTileActions.Contains("Move"))
            TileTransform = GameObjectTransform.transform.parent;
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
}
