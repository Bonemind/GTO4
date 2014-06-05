using UnityEngine;
using System.Collections;

public class Building : Photon.MonoBehaviour {
    /// <summary>
    /// The row this building lives in
    /// </summary>
    public int r;

    /// <summary>
    /// The column this building lives in
    /// </summary>
    public int c;

    public void OnMouseDown()
    {
        HUD.currState = HUD.ActionState.SELECTED_BUILDING;
        HUD.currentObject = gameObject;
    }

    public void TurnEnd()
    {
        print("TurnEnd building");
    }
}
