using UnityEngine;
using System.Collections.Generic;

public class UnitProductionBuilding : Building{
    /// <summary>
    /// List containing the current build queue
    /// </summary>
    private List<ProductionStruct> buildQueue = new List<ProductionStruct>();

    /// <summary>
    /// Contains the current location
    /// </summary>
    private LocationStruct location;

    /// <summary>
    /// Initializes this object
    /// </summary>
    public override void Initialize()
    {
        location = gameObject.GetComponent<BoardLocation>().location;
    }

    /// <summary>
    /// Responds to a hud action
    /// </summary>
    /// <param name="go">The gameobject that was clicked</param>
    public override void HUDAction(GameObject go)
    {
        ResourceCost resCost = go.GetComponent<ResourceCost>();
        if (resCost == null)
        {
            Debug.LogError(string.Format("Buildable object {0} does not have a resource cost script", go.name));
        }
        if (!resCost.CheckCost())
        {
            return;
        }
        resCost.DecreaseResources();
        ProductionStruct ps = new ProductionStruct();
        ps.productionObject = go;
        ps.turnsLeft = go.GetComponent<Unit>().BuildTurns;
        buildQueue.Add(ps);
    }

    /// <summary>
    /// Handles a turn start
    /// </summary>
    public override void HandleTurnStart()
    {
        for (int i = buildQueue.Count - 1; i >= 0; i-- )
        {
            if (buildQueue[i].turnsLeft <= 0)
            {
     
                LocationStruct? freeTileNullable = Board.GetClosestFreeTile(location);
                if (freeTileNullable == null)
                {
                    Debug.LogError("Couldn't find empty location, retrying next turn");
                    continue;
                }
                LocationStruct freeTile = (LocationStruct)freeTileNullable;
                GameObject newUnit = (GameObject)PhotonNetwork.Instantiate(Utils.RemoveClone(buildQueue[i].productionObject.name), Board.GetTileFromLocation(freeTile).transform.position, new Quaternion(), 0);
                TileControl tc = Board.GetTileControlFromLocation(freeTile);
                tc.SetOccupyingObject(newUnit);
                newUnit.GetComponent<BoardLocation>().SetLocation(freeTile.row, freeTile.column);
                buildQueue.Remove(buildQueue[i]);
            }
            else
            {
                buildQueue[i].turnsLeft--;
            }

        }
    }
}
