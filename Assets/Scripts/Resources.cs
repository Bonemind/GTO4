using UnityEngine;
using System.Collections;

public class GameResources {
    /// <summary>
    /// The available resource types
    /// </summary>
    public enum ResourceTypes {
        RES1,
        RES2,
        RES3
    }
    /// <summary>
    /// Array containing the resources
    /// </summary>
    private int[] resources = new int[3];
    
    /// <summary>
    /// Starting resources for res 1
    /// </summary>
    public int RES1Initial = 100;

    /// <summary>
    /// Starting resources for res 2 
    /// </summary>
    public int RES2Initial = 200;

    /// <summary>
    /// Starting resources for res 3
    /// </summary>
    public int RES3Initial = 50;

    /// <summary>
    /// Amount of res1 gained per turn
    /// </summary>
    public int RES1TurnStartIncrease = 10;

    /// <summary>
    /// Amount of res2 gained per turn
    /// </summary>
    public int RES2TurnStartIncrease = 30;

    /// <summary>
    /// Amount of res3 gained per turn
    /// </summary>
    public int RES3TurnStartIncrease = 5;

	/// <summary>
	/// Default constructor
    /// Sets the resources to the initial values
	/// </summary>
	public GameResources () {
        resources[(int)ResourceTypes.RES1] = RES1Initial;
        resources[(int)ResourceTypes.RES2] = RES2Initial;
        resources[(int)ResourceTypes.RES3] = RES3Initial;
	
	}

    /// <summary>
    /// Increase the resources on the start of a turn by the set amount
    /// </summary>
    public void TurnStartIncrease()
    {
        resources[(int)ResourceTypes.RES1] += RES1TurnStartIncrease;
        resources[(int)ResourceTypes.RES2] += RES2TurnStartIncrease;
        resources[(int)ResourceTypes.RES3] += RES3TurnStartIncrease;
    }

    /// <summary>
    /// Returns how much of a given resource we have
    /// </summary>
    /// <param name="res">The resource</param>
    /// <returns>The amount we have</returns>
    public int GetResource(ResourceTypes res)
    {
        return resources[(int)res];
    }

    /// <summary>
    /// Vhecks whether we have enough of the passed resource
    /// </summary>
    /// <param name="res">The resource to check</param>
    /// <param name="amount">The amount we should have</param>
    /// <returns></returns>
    public bool HasResource(ResourceTypes res, int amount)
    {
        return this.GetResource(res) >= amount;
    }

    /// <summary>
    /// Decreases passed resource by the passed amount
    /// </summary>
    /// <param name="res">The resource to decrease</param>
    /// <param name="amount">The amount to decrease it with</param>
    public void DecreaseResource(ResourceTypes res, int amount)
    {
        if (!this.HasResource(res, amount))
        {
            return;
        }
        this.resources[(int)res] -= amount;
    }

    /// <summary>
    /// Increases passed resource by the passed amount
    /// </summary>
    /// <param name="res">The resource to increase</param>
    /// <param name="amount">The amount to increase it with</param>
    public void IncreaseResource(ResourceTypes res, int amount)
    {
        this.resources[(int)res] += amount;
    }

    /// <summary>
    /// Sets a resource to a certain amount
    /// </summary>
    /// <param name="res">The resource to set</param>
    /// <param name="amount">The amount to set it to</param>
    public void SetResource(ResourceTypes res, int amount)
    {
        this.resources[(int)res] = amount;
    }
}
