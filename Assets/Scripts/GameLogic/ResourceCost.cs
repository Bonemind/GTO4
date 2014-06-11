using UnityEngine;
using System.Collections;

public class ResourceCost : Photon.MonoBehaviour {
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
}
