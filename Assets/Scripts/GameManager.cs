using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private PhotonPlayer currentPlayer = null;

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
}
