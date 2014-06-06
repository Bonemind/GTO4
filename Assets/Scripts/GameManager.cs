using UnityEngine;
using System.Collections;

public class GameManager : Photon.MonoBehaviour {
    private PhotonPlayer currentPlayer = null;
    private PhotonPlayer[] players;

	// Use this for initialization
	void Start () {
        currentPlayer = PhotonNetwork.masterClient;
       
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EndTurn();
        }
	}

    public void EndTurn()
    {
        BroadcastMessage("TurnEnd", SendMessageOptions.DontRequireReceiver);
    }

    public void StartTurn()
    {
        BroadcastMessage("TurnStart", SendMessageOptions.DontRequireReceiver);
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        players = PhotonNetwork.playerList;
        print("Player connected");
    }
}
