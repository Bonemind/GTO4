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


	// Use this for initialization
	void Start () {
        currState = ActionState.NO_ACTION;
	}
	
	// Update is called once per frame
	void Update () {
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
	}

    /// <summary>
    /// Handles gui drawing
    /// </summary>
    void OnGUI()
    {
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

    public void HUDAction(GameObject prefab)
    {
        selectedPrefab = prefab;
        currState = ActionState.PLACING_BUILDING;
    }
}
