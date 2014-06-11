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

    private GameObject selectionObject;

    public RenderTexture minimap;

    public GUISkin MinimapSkin;


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
            if (selectedObject != null)
            {
                selectedObject.SendMessage("Selection", false, SendMessageOptions.DontRequireReceiver);
            }
            selectedObject = value;
            if (value != null)
            {
                value.SendMessage("Selection", true, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                currState = ActionState.NO_ACTION;
            }
            Instance.HandleSelectionMarker();
        }
    }

    public static HUD Instance
    {
        get
        {
            return Camera.main.GetComponent<HUD>();
        }
    }

    private void HandleSelectionMarker()
    {
        if (currentObject == null && selectionObject != null)
        {
            Destroy(selectionObject);
            return;
        }
        if (currentObject == null)
        {
            return;
        }
        if (selectionObject == null)
        {
            selectionObject = (GameObject) Instantiate(SelectionPrefab, Vector3.zero, new Quaternion());
        }
        selectionObject.transform.parent = currentObject.transform;
        selectionObject.transform.position = currentObject.transform.position;
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
        GUILayout.BeginArea(new Rect(0f, 0f, 400f, 30f));
        {
            //GUI.Label(new Rect(Screen.width * 0.7f, 0f, 400f, 30f), string.Format("RES1: {0} RES2: {1} RES3: {2}", resources.GetResource(GameResources.ResourceTypes.RES1), resources.GetResource(GameResources.ResourceTypes.RES2), resources.GetResource(GameResources.ResourceTypes.RES3)));
            GUILayout.Box(string.Format("Credits: {0} Energy: {1} Uranium: {2}", resources.GetResource(GameResources.ResourceTypes.RES1), resources.GetResource(GameResources.ResourceTypes.RES2), resources.GetResource(GameResources.ResourceTypes.RES3)));
        }
        GUILayout.EndArea();
        GUISkin originalSkin = GUI.skin;
        GUI.skin = MinimapSkin;
        GUILayout.BeginArea(new Rect(Screen.width - minimap.width, 0f, minimap.width, minimap.height));
        {
            GUILayout.Box(minimap);
        }
        GUILayout.EndArea();
        GUI.skin = originalSkin;


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
        if (GUIUtility.hotControl > 0)
        {
            return;
        }
        //It's not our turn, return
        if (!MyTurn)
        {
            return;
        }
        //We haven't selected any object, instead of informing the object of our actions, select whatever object we
        //clicked, if it was a tile select the object it contained
        if (currState != ActionState.PLACING_BUILDING && Input.GetMouseButtonDown((int)MouseButtons.LEFT))
        {
            TileControl tc = Board.board[location.row, location.column].GetComponent<TileControl>();
            if (tc != null)
            {
                HUD.currentObject = tc.occupyingObject;
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
        else if (currentObject != null)
        {
            currentObject.SendMessage("MouseEnter", location, SendMessageOptions.DontRequireReceiver);
            if (Input.GetMouseButtonDown((int)MouseButtons.RIGHT))
            {
                currentObject.SendMessage("RightClick", location, SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetMouseButtonDown((int)MouseButtons.MIDDLE))
            {
                currentObject.SendMessage("MiddleClick", location, SendMessageOptions.DontRequireReceiver);
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
        if (!selectedPrefab.GetComponent<Building>().IsValidTile(location))
        {
            return;
        }
        if (currentObject == null)
        {
            currentObject = (GameObject)Instantiate(selectedPrefab, Vector3.zero, new Quaternion(0f, 0f, 0f, 0f));
            currentObject.collider.enabled = false;
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
        Building building = currentObject.GetComponent<Building>();
        if (building == null)
        {
            return;
        }
        ResourceCost resCost = currentObject.GetComponent<ResourceCost>();
        if (resCost == null)
        {
            Debug.Log(string.Format("Buildable object {0} does not have a resource cost script", currentObject.name));
        }
        if (!resCost.CheckCost())
        {
            return;
        }
        resCost.DecreaseResources();
        selectedPrefab = null;
        GameObject tile = Board.board[location.row, location.column];
        TileControl tc = tile.GetComponent<TileControl>();
        GameObject actualObject = (GameObject) PhotonNetwork.Instantiate(Utils.RemoveClone(currentObject.name), currentObject.transform.position, currentObject.transform.rotation, 0);

        Destroy(HUD.currentObject);
        currentObject = null;

        tc.SetOccupyingObject(actualObject);
        actualObject.GetComponent<BoardLocation>().SetLocation(location.row, location.column);

        currState = ActionState.NO_ACTION;
    }
}
