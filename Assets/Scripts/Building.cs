using UnityEngine;
using System.Collections;

public class Building : Photon.MonoBehaviour {
    public int CostRes1 = 10;
    public int CostRes2 = 5;
    public int CostRes3 = 3;
    BoardLocation boardLocation;

    public void Start()
    {
        ConsoleLog.Instance.Log("BuildingStart");
        boardLocation = gameObject.GetComponent<BoardLocation>();
    }

    public void OnMouseDown()
    {
        ConsoleLog.Instance.Log(boardLocation.row + " " + boardLocation.column);  
        if (!this.photonView.isMine)
        {
            return;
        }
        HUD.currState = HUD.ActionState.SELECTED_BUILDING;
        HUD.currentObject = gameObject;
    }

    public void TurnStart()
    {
        ConsoleLog.Instance.Log("Building turn end");
    }

    public bool CheckCost()
    {
        HUD h = Camera.main.GetComponent<HUD>();
        GameResources res = h.GetResources();
        return res.HasResource(GameResources.ResourceTypes.RES1, CostRes1) && res.HasResource(GameResources.ResourceTypes.RES2, CostRes2) && res.HasResource(GameResources.ResourceTypes.RES3, CostRes3);
    }

    public void DecreaseResources()
    {
        HUD h = Camera.main.GetComponent<HUD>();
        GameResources res = h.GetResources();
        res.DecreaseResource(GameResources.ResourceTypes.RES1, CostRes1);
        res.DecreaseResource(GameResources.ResourceTypes.RES2, CostRes2);
        res.DecreaseResource(GameResources.ResourceTypes.RES3, CostRes3);
    }

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
}
