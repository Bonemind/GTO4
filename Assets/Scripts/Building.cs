using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
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
        Debug.Log(r + "" + c);
        Debug.Log(GameManager.board[r,c]);
        HUD.currState = HUD.ActionState.SELECTED_BUILDING;
        HUD.currentObject = gameObject;
    }

    public void DrawGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 300), "Hello, i was selected");
    }
}
