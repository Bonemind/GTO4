using UnityEngine;
using System.Collections;

public class test : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.0.1");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Application.LoadLevel("testscene");
        }
	}

    void OnGUI()
    {
        GUI.Label(new Rect(0f, 0f, 300f, 20f), PhotonNetwork.connectionStateDetailed.ToString());
    }

    public void OnJoinedLobby()
    {
        Debug.Log("connectedtomaster");
        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }
}
