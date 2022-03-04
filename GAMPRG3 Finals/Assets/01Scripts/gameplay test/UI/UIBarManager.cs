using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarManager : MonoBehaviour
{
    public HPBarScript HPBar;
    public APBarScript APBAR;
    public XPBarScript XPBar;

    public void UpdateAllBars()
    {
        HPBar.UpdateSelectedTarget();
        APBAR.UpdateSelectedTarget();
        XPBar.UpdateSelectedTarget();
    }
}
