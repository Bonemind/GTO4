using UnityEngine;
using System.Collections;

public class GameManager : Photon.MonoBehaviour {
    /// <summary>
    /// The current player index
    /// </summary>
    private int currPlayerIndex = 0;

    /// <summary>
    /// The connected player list
    /// </summary>
    private PhotonPlayer[] players;

    private HUD hud;

	// Use this for initialization
	void Start () {
        players = PhotonNetwork.playerList;
        hud = Camera.main.GetComponent<HUD>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C) && HUD.MyTurn)
        {
            EndTurn();
        }
	}

    /// <summary>
    /// Informs all tiles that a turn has just ended
    /// </summary>
    [RPC]
    public void EndTurn()
    {
        if (PhotonNetwork.isMasterClient)
        {
            currPlayerIndex = (currPlayerIndex + 1) % players.Length;
            photonView.RPC("StartTurn", PhotonTargets.All, players[currPlayerIndex].ID);
        }
        else
        {
            photonView.RPC("EndTurn", PhotonTargets.MasterClient);
        }
    }

    /// <summary>
    /// Informs all tiles that a turn has just started
    /// </summary>
    [RPC]
    public void StartTurn(int playerId)
    {
        //A turn is starting, this means a turn has just ended, inform all tiles of that first
        //BroadcastMessage("TurnEnd", SendMessageOptions.DontRequireReceiver);
        if (PhotonNetwork.player.ID == playerId)
        {
            HUD.MyTurn = true;
            //BroadcastMessage("TurnStart", SendMessageOptions.DontRequireReceiver);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("placeable"))
            {
                go.SendMessage("TurnStart", SendMessageOptions.DontRequireReceiver);
            }
            hud.TurnStart();
        }
        else
        {
            HUD.MyTurn = false;
        }
    }

    /// <summary>
    /// Player connected event
    /// </summary>
    /// <param name="player"></param>
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        players = PhotonNetwork.playerList;
    }
}
