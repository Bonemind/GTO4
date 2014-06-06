using UnityEngine;
using System.Collections;

public class Building : Photon.MonoBehaviour {
    /// <summary>
    /// The row this building lives in
    /// </summary>
    public int r;

    /// <summary>
    /// The column this building lives in
    /// </summary>
    public int c;

    public int CostRes1 = 10;
    public int CostRes2 = 5;
    public int CostRes3 = 3;

    public void OnMouseDown()
    {
        if (!this.photonView.isMine)
        {
            return;
        }
        HUD.currState = HUD.ActionState.SELECTED_BUILDING;
        HUD.currentObject = gameObject;
    }

    public void TurnEnd()
    {
        print("TurnEnd building");
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
}
