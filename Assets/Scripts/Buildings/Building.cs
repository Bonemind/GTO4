using UnityEngine;
using System.Collections.Generic;

public abstract class Building : Photon.MonoBehaviour {
    /// <summary>
    /// The location this building lives in
    /// </summary>
    #pragma warning disable 0414
    BoardLocation boardLocation;
    #pragma warning restore 0414

    /// <summary>
    /// Initialization
    /// </summary>
    public void Start()
    {
        boardLocation = gameObject.GetComponent<BoardLocation>();
        Initialize();
    }

    /// <summary>
    /// Handles a click of the left mouse button
    /// </summary>
    public void OnMouseDown()
    {
        if (!this.photonView.isMine)
        {
            return;
        }
        HUD.currState = HUD.ActionState.SELECTED_BUILDING;
        HUD.currentObject = gameObject;
    }

    /// <summary>
    /// Handles the start of a turn
    /// </summary>
    public void TurnStart()
    {
        if (photonView.isMine)
        {
            HandleTurnStart();
        }
    }

    /// <summary>
    /// Handles a right click
    /// </summary>
    /// <param name="location">The location the right click was initiated</param>
    public void RightClick(LocationStruct location)
    {
        HUD.currentObject = null;
    }

    /// <summary>
    /// Handles a photon instantiation event
    /// </summary>
    /// <param name="info">PhotonInfo</param>
    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        if (info.sender == PhotonNetwork.player)
        {
            ConsoleLog.Instance.Log("MyOwner");
        }
        else
        {
            ConsoleLog.Instance.Log("OtherOwner");
        }
        
    }

    public abstract void HUDAction(GameObject go);
    public abstract void HandleTurnStart();
    public abstract void Initialize();
}
