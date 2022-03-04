using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIIconGrabber : MonoBehaviour
{
    public UnitSelector UnitSelector;

    // Update is called once per frame
    void Update()
    {
        if (UnitSelector.SelectedUnit != null)
        {
            Debug.Log("Sprite Selected");
            this.GetComponent<Image>().sprite = UnitSelector.SelectedUnit.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = null;
            Debug.Log("No Sprite");
        }
    }
}
