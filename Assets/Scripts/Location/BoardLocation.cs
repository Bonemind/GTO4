using UnityEngine;
using System.Collections;

public class BoardLocation : Photon.MonoBehaviour {
    public int row = 0;
    public int column = 0;

    public void RegisterParent()
    {
        string tag = gameObject.tag;
        switch (tag)
        {
            case "tile":
                gameObject.transform.parent = GameObject.Find("GameManager").transform;
                Board.board[row, column] = gameObject;
                break;
            case "placeable":
                gameObject.transform.parent = Board.board[this.row, this.column].transform;
                Board.board[this.row, this.column].GetComponent<TileControl>().occupyingObject = gameObject;
                break;
            default:
                ConsoleLog.Instance.Log("RegisterParent reached default case for object" + gameObject.name);
                break;
        }
    }
    public void SetLocation(int row, int column)
    {
        photonView.RPC("SyncLocation", PhotonTargets.AllBuffered, row, column);
    }

    [RPC]
    public void SyncLocation(int row, int column)
    {
        if (gameObject.tag != "tile")
        {
            ConsoleLog.Instance.Log("Synclocation");
        }
        this.row = row;
        this.column = column;
        RegisterParent();
    }

    public void OnMouseDown()
    {
        ConsoleLog.Instance.Log(string.Format("Tile {0} - {1}", row, column));
    }
}
