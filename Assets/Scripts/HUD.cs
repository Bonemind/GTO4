using UnityEngine;
using System.Collections;

public class HUD : Photon.MonoBehaviour
{
    /// <summary>
    /// The actionstates we have within the game
    /// </summary>
    public enum ActionState{
        NO_ACTION,
        PLACING_BUILDING,
        SELECTED_BUILDING,
        SELECTED_UNIT
    }

    GameResources resources = new GameResources();

    /// <summary>
    /// The currently selected object
    /// </summary>
    public static GameObject currentObject = null;

    /// <summary>
    /// The currently selected prefab
    /// </summary>
    public GameObject selectedPrefab = null;

    /// <summary>
    /// The current action state
    /// </summary>
    public static ActionState currState;

    public static bool MyTurn = false;


	// Use this for initialization
	void Start () {
        currState = ActionState.NO_ACTION;
	}
	
	// Update is called once per frame
	void Update () {
        if (!MyTurn)
        {
            return;
        }
        switch (currState)
        {
            case ActionState.NO_ACTION:
                noState();
                break;
            case ActionState.PLACING_BUILDING:
                placingBuildingState();
                break;
            case ActionState.SELECTED_BUILDING:
                selectedBuildingState();
                break;
            default:
                break;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConsoleLog.Instance.Log("Ended turn");
            GameObject.Find("GameManager").SendMessage("StartTurn", SendMessageOptions.DontRequireReceiver);
            IncreaseResources();
        }
	}

    /// <summary>
    /// Handles gui drawing
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width * 0.7f, 0f, 400f, 30f), string.Format("RES1: {0} RES2: {1} RES3: {2}", resources.GetResource(GameResources.ResourceTypes.RES1), resources.GetResource(GameResources.ResourceTypes.RES2), resources.GetResource(GameResources.ResourceTypes.RES3)));
        if (!MyTurn)
        {
            return;
        }
        switch (currState)
        {
            case ActionState.NO_ACTION:
                SendMessage("DrawGUI", SendMessageOptions.DontRequireReceiver);
                break;
            case ActionState.SELECTED_BUILDING:
                currentObject.SendMessage("DrawGUI", SendMessageOptions.DontRequireReceiver);
                break;
            default:
                break;
        }
        
    }



    /// <summary>
    /// Handles the neutral (no action) state
    /// </summary>
    private void noState()
    {
        
    }

    /// <summary>
    /// Handles the building placement state
    /// </summary>
    private void placingBuildingState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentObject != null)
            {
                Destroy(currentObject);
            }
            selectedPrefab = null;
            currState = ActionState.NO_ACTION;
        }
    }

    /// <summary>
    /// Handles the building selected state
    /// </summary>
    private void selectedBuildingState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentObject = null;
            currState = ActionState.NO_ACTION;
        }
    }

    /// <summary>
    /// Handles clicking a build button
    /// </summary>
    /// <param name="prefab"></param>
    public void HUDAction(GameObject prefab)
    {
        selectedPrefab = prefab;
        currState = ActionState.PLACING_BUILDING;
    }

    /// <summary>
    /// Wraps resources.turnstartincrease
    /// </summary>
    public void IncreaseResources()
    {
        resources.TurnStartIncrease();
    }

    /// <summary>
    /// Returns this player's resources object
    /// </summary>
    /// <returns></returns>
    public GameResources GetResources()
    {
        return this.resources;
    }

    public void TurnStart()
    {
        this.IncreaseResources();
    }
}
