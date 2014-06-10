using UnityEngine;
using System.Collections;

public class ResourceProductionBuilding : Building {
    /// <summary>
    /// The gameresources script
    /// </summary>
    private GameResources resources;

    /// <summary>
    /// The production of resources in 1 turn
    /// </summary>
    public int TurnProduction = 10;

    public GameResources.ResourceTypes Resource = GameResources.ResourceTypes.RES2;

    /// <summary>
    /// Initializes this script
    /// </summary>
    public override void Initialize()
    {
        resources = HUD.Instance.GetResources();
    }

    /// <summary>
    /// Handles a turn start for this type of building
    /// </summary>
    public override void HandleTurnStart()
    {
        resources.IncreaseResource(GameResources.ResourceTypes.RES2, TurnProduction);
    }

    public override void HUDAction(GameObject go)
    {
        //void
    }
}
