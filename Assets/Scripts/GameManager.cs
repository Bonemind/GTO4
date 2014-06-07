using UnityEngine;
using System.Collections;

public class GameManager : Photon.MonoBehaviour {
    /// <summary>
    /// The current player
    /// </summary>
    private PhotonPlayer currentPlayer = null;

    private int currPlayerIndex = 0;

    /// <summary>
    /// The connected player list
    /// </summary>
    private PhotonPlayer[] players;

    private HUD hud;

	// Use this for initialization
	void Start () {
        currentPlayer = PhotonNetwork.masterClient;
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
    public void EndTurn()
    {
        currPlayerIndex = (currPlayerIndex + 1) % players.Length;

        photonView.RPC("StartTurn", PhotonTargets.All, currPlayerIndex + 1);
    }

    /// <summary>
    /// Informs all tiles that a turn has just started
    /// </summary>
    [RPC]
    public void StartTurn(int playerId)
    {
        //A turn is starting, this means a turn has just ended, inform all tiles of that first
        BroadcastMessage("TurnEnd", SendMessageOptions.DontRequireReceiver);
        if (PhotonNetwork.player.ID == playerId)
        {
            HUD.MyTurn = true;
            BroadcastMessage("TurnStart", SendMessageOptions.DontRequireReceiver);
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
