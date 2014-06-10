using UnityEngine;
using System.Collections;

public class BoardLocation : Photon.MonoBehaviour {
    /// <summary>
    /// The location this gameobject lives in
    /// </summary>
    public LocationStruct location;

    /// <summary>
    /// The column this gameobject lives in
    /// </summary>
    private GameObject mainCamera;

    /// <summary>
    /// Initialization
    /// </summary>
    public void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    /// <summary>
    /// Registers this object with it's parent
    /// </summary>
    public void RegisterParent()
    {
        string tag = gameObject.tag;
        switch (tag)
        {
            case "tile":
                gameObject.transform.parent = GameObject.Find("GameManager").transform;
                Board.board[location.row, location.column] = gameObject;
                break;
            case "placeable":
                gameObject.transform.parent = Board.board[location.row, location.column].transform;
                Board.board[location.row, location.column].GetComponent<TileControl>().occupyingObject = gameObject;
                break;
            default:
                ConsoleLog.Instance.Log("RegisterParent reached default case for object" + gameObject.name);
                break;
        }
    }

    /// <summary>
    /// Broadcasts a new location for this object to all clients
    /// </summary>
    /// <param name="row">The new row</param>
    /// <param name="column">The new column</param>
    public void SetLocation(int row, int column)
    {
        photonView.RPC("SyncLocation", PhotonTargets.AllBuffered, row, column);
    }

    /// <summary>
    /// Actually sets the row and column for this object
    /// </summary>
    /// <param name="row">The row to set</param>
    /// <param name="column">The column to set</param>
    [RPC]
    public void SyncLocation(int row, int column)
    {
        if (gameObject.tag != "tile")
        {
            Board.GetTileControlFromLocation(location).occupyingObject = null;
        }
        location.row = row;
        location.column = column;
        RegisterParent();
    }

    /// <summary>
    /// Informs our hud that this object was mouseovered
    /// </summary>
    public void OnMouseOver()
    {
        mainCamera.SendMessage("MouseOver", location, SendMessageOptions.DontRequireReceiver);
    }
}
