using UnityEngine;
using System.Collections;

public class Building : Photon.MonoBehaviour {
    /// <summary>
    /// Resource cost for the first resource
    /// </summary>
    public int CostRes1 = 10;

    /// <summary>
    /// Resource cost for the second resource
    /// </summary>
    public int CostRes2 = 5;

    /// <summary>
    /// Resource cost for the third resource
    /// </summary>
    public int CostRes3 = 3;

    /// <summary>
    /// The location this building lives in
    /// </summary>
    BoardLocation boardLocation;

    /// <summary>
    /// Initialization
    /// </summary>
    public void Start()
    {
        boardLocation = gameObject.GetComponent<BoardLocation>();
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
            Debug.Log("Building turn start");
        }
        else
        {
            Debug.Log("not mine");
        }
    }

    /// <summary>
    /// Checks if we have enough resources to build this building
    /// </summary>
    /// <returns>True if we do, false otherwise</returns>
    public bool CheckCost()
    {
        HUD h = Camera.main.GetComponent<HUD>();
        GameResources res = h.GetResources();
        return res.HasResource(GameResources.ResourceTypes.RES1, CostRes1) && res.HasResource(GameResources.ResourceTypes.RES2, CostRes2) && res.HasResource(GameResources.ResourceTypes.RES3, CostRes3);
    }

    /// <summary>
    /// Decreases the resources required to build this from the resource pool
    /// </summary>
    public void DecreaseResources()
    {
        HUD h = Camera.main.GetComponent<HUD>();
        GameResources res = h.GetResources();
        res.DecreaseResource(GameResources.ResourceTypes.RES1, CostRes1);
        res.DecreaseResource(GameResources.ResourceTypes.RES2, CostRes2);
        res.DecreaseResource(GameResources.ResourceTypes.RES3, CostRes3);
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

    public void HUDAction(GameObject go)
    {
        Debug.Log(go.ToString());
    }
}
