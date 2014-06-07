using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {

    /// <summary>
    /// General setup
    /// </summary>
	public void Start () {
        PhotonNetwork.ConnectUsingSettings("0.0.1");
	}

    /// <summary>
    /// Draw guilabel
    /// </summary>
    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "---" + PhotonNetwork.otherPlayers.Length);
    }

    /// <summary>
    /// Join lobby event
    /// </summary>
    public void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Join lobby failed event
    /// </summary>
    public void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            ConsoleLog.Instance.Log("Joined room master");
            HUD.MyTurn = true;
        } 
        else {
            HUD.MyTurn = false;
        }
    }
}
