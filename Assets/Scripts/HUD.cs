using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
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

    /// <summary>
    /// The available prefabs (buildings) to place
    /// </summary>
    public GameObject[] prefabs;

	// Use this for initialization
	void Start () {
        currState = ActionState.NO_ACTION;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (GameObject go in prefabs)
            {
                Debug.Log(go.name);
            }
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
	}

    /// <summary>
    /// Handles gui drawing
    /// </summary>
    void OnGUI()
    {
        switch (currState)
        {
            case ActionState.NO_ACTION:
                drawBuildingsGui();
                break;
            case ActionState.SELECTED_BUILDING:
                currentObject.SendMessage("DrawGUI", SendMessageOptions.DontRequireReceiver);
                break;
            default:
                break;
        }
    }

    private void drawBuildingsGui()
    {
        float xStart = Screen.width / 2;
        float yStart = Screen.height - (0.25f * Screen.height);
        GUILayout.BeginArea(new Rect(0, Screen.height - 0.1f * Screen.height, Screen.width, 0.1f * Screen.height));
        GUILayout.FlexibleSpace();
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (GUILayout.Button(prefabs[i].name))
                {
                    selectedPrefab = prefabs[i];
                    currState = ActionState.PLACING_BUILDING;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }

    /// <summary>
    /// Handles the neutral (no action) state
    /// </summary>
    private void noState()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedPrefab = prefabs[0];
            currState = ActionState.PLACING_BUILDING;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedPrefab = prefabs[1];
            currState = ActionState.PLACING_BUILDING;
        }
        
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
}
