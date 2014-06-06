using UnityEngine;
using System.Collections;

public class GameResources {
    public enum ResourceTypes {
        RES1,
        RES2,
        RES3
    }
    private int[] resources = new int[3];
    public int RES1Initial = 100;
    public int RES2Initial = 200;
    public int RES3Initial = 50;

    public int RES1TurnStartIncrease = 10;
    public int RES2TurnStartIncrease = 30;
    public int RES3TurnStartIncrease = 5;

	// Use this for initialization
	public GameResources () {
        resources[(int)ResourceTypes.RES1] = RES1Initial;
        resources[(int)ResourceTypes.RES2] = RES2Initial;
        resources[(int)ResourceTypes.RES3] = RES3Initial;
	
	}

    public void TurnStartIncrease()
    {
        resources[(int)ResourceTypes.RES1] += RES1TurnStartIncrease;
        resources[(int)ResourceTypes.RES2] += RES2TurnStartIncrease;
        resources[(int)ResourceTypes.RES3] += RES3TurnStartIncrease;
    }

    public int GetResource(ResourceTypes res)
    {
        return resources[(int)res];
    }

    public bool HasResource(ResourceTypes res, int amount)
    {
        return this.resources[(int)res] >= amount;
    }

    public void DecreaseResource(ResourceTypes res, int amount)
    {
        this.resources[(int)res] -= amount;
    }
}
