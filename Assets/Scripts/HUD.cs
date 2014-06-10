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
    private static GameObject selectedObject = null;

    /// <summary>
    /// The currently selected prefab
    /// </summary>
    public GameObject selectedPrefab = null;

    /// <summary>
    /// The current action state
    /// </summary>
    public static ActionState currState;

    public static bool MyTurn = false;

    public GameObject SelectionPrefab;


	// Use this for initialization
	void Start () {
        currState = ActionState.NO_ACTION;
	}

    /// <summary>
    /// The object currently selected
    /// </summary>
    public static GameObject currentObject
    {
        get
        {
            return selectedObject;
        }
        set
        {
            selectedObject = value;
        }
    }

	// Update is called once per frame
	void Update () {
        if (!MyTurn)
        {
            return;
        }
        if (currentObject != null && currState == ActionState.NO_ACTION)
        {
            currState = ActionState.SELECTED_BUILDING;
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
            GameObject.Find("GameManager").SendMessage("EndTurn", SendMessageOptions.DontRequireReceiver);
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

    /// <summary>
    /// Handles mouseover messages
    /// Also handles Clicks
    /// </summary>
    /// <param name="location">The location where the event originated</param>
    public void MouseOver(LocationStruct location)
    {
        //We haven't selected any object, instead of informing the object of our actions, select whatever object we
        //clicked as long as it is not a tile
        if (selectedObject == null && currState != ActionState.PLACING_BUILDING)
        {
            TileControl tc = Board.board[location.row, location.column].GetComponent<TileControl>();
            if (tc != null && tc.occupyingObject != null)
            {
                
                if (Input.GetMouseButtonDown((int)MouseButtons.LEFT))
                {
                    HUD.selectedObject = tc.occupyingObject;
                    //BuildingMouseLeft(location);
                }
                if (Input.GetMouseButtonDown((int)MouseButtons.RIGHT))
                {
                 
                }
                if (Input.GetMouseButtonDown((int)MouseButtons.MIDDLE))
                {
                 
                }
            }
            return;
        }
        //We're placing a building, info should be handled by this object
        else if (currState == ActionState.PLACING_BUILDING)
        {
            BuildingMouseOver(location);
            if (Input.GetMouseButtonDown((int)MouseButtons.LEFT))
            {
                BuildingMouseLeft(location);
            }
            if (Input.GetMouseButtonDown((int)MouseButtons.RIGHT))
            {
                Destroy(currentObject);
                currentObject = null;
                currState = ActionState.NO_ACTION;
                return;
            }
        }
        //An object is selected, route our actions to said object
        else
        {
            selectedObject.SendMessage("MouseEnter", location, SendMessageOptions.DontRequireReceiver);
            if (Input.GetMouseButtonDown((int)MouseButtons.LEFT))
            {
                selectedObject.SendMessage("LeftClick", location, SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetMouseButtonDown((int)MouseButtons.RIGHT))
            {
                selectedObject.SendMessage("RightClick", location, SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetMouseButtonDown((int)MouseButtons.MIDDLE))
            {
                selectedObject.SendMessage("MiddleClick", location, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    /// <summary>
    /// Handles the mouseover state while a building is being placed
    /// </summary>
    /// <param name="location">The location the mouseoverstate occured from</param>
    private void BuildingMouseOver(LocationStruct location)
    {
        if (currState != ActionState.PLACING_BUILDING)
        {
            return;
        }
        if (selectedPrefab == null)
        {
            return;
        }
        if (currentObject == null)
        {
            currentObject = (GameObject)Instantiate(selectedPrefab, Vector3.zero, new Quaternion(0f, 0f, 0f, 0f));
            currentObject.collider.enabled = false;
        }
        else
        {
            Debug.Log("currentObject != null");
        }
        TileControl tc = Board.board[location.row, location.column].GetComponent<TileControl>();
        if (tc.occupyingObject != null)
        {
            return;
        }
        tc.setObjectProperties(currentObject);
    }

    /// <summary>
    /// Handles left clicking when placing a building
    /// </summary>
    /// <param name="location">The location the action occured from</param>
    private void BuildingMouseLeft(LocationStruct location)
    {
        Building building = selectedObject.GetComponent<Building>();
        if (building == null)
        {
            return;
        }
        if (!building.CheckCost())
        {
            return;
        }
        selectedPrefab = null;
        building.DecreaseResources();
        GameObject tile = Board.board[location.row, location.column];
        TileControl tc = tile.GetComponent<TileControl>();
        GameObject actualObject = (GameObject) PhotonNetwork.Instantiate(Utils.RemoveClone(currentObject.name), currentObject.transform.position, currentObject.transform.rotation, 0);

        Destroy(HUD.currentObject);
        currentObject = null;


        Debug.Log(actualObject);
        Debug.Log(currentObject);
        tc.SetOccupyingObject(actualObject);
        actualObject.GetComponent<BoardLocation>().SetLocation(location.row, location.column);

        currState = ActionState.NO_ACTION;
    }
}
