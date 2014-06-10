using UnityEngine;
using System.Collections.Generic;

public class UnitProductionBuilding : Building{
    private List<ProductionStruct> buildQueue = new List<ProductionStruct>();
    private LocationStruct location;

    public override void Initialize()
    {
        location = gameObject.GetComponent<BoardLocation>().location;
        //void
    }

    public override void HUDAction(GameObject go)
    {
        ProductionStruct ps = new ProductionStruct();
        ps.productionObject = go;
        ps.turnsLeft = go.GetComponent<Unit>().BuildTurns;
        Debug.Log("HUDACtion");
        buildQueue.Add(ps);
    }

    public override void HandleTurnStart()
    {
        Debug.Log("turnstart");
        for (int i = buildQueue.Count - 1; i >= 0; i-- )
        {
            if (buildQueue[i].turnsLeft <= 0)
            {
                Debug.Log("produced");
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
                Debug.Log("turnsleft-");
            }

        }
    }
}
