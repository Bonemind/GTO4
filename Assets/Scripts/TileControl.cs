using UnityEngine;
using System.Collections;

public class TileControl : Photon.MonoBehaviour
{
    /// <summary>
    /// The object currntly occupying this tile
    /// </summary>
    public GameObject occupyingObject = null;

    /// <summary>
    /// TODO: Remove
    /// </summary>
    private Color originalColor;
    private BoardLocation boardLocation;

    /// <summary>
    /// Used for intialization
    /// </summary>
    public void Start()
    {
        originalColor = gameObject.renderer.material.color;
        boardLocation = gameObject.GetComponent<BoardLocation>();
    }

    /// <summary>
    /// Handles the mouse cursor entering the bounds of this tile
    /// Also handles building placement preview
    /// </summary>
    public void OnMouseEnter()
    {
        transform.renderer.material.color = Color.cyan;
        /*
        if (occupyingObject != null)
        {
            return;
        }
        
        HUD hud = Camera.main.GetComponent<HUD>();
        if (hud.selectedPrefab == null)
        {
            return;
        }
        if (hud.selectedPrefab.GetComponent<Building>() == null)
        {
            return;
        }
        if (HUD.currentObject == null)
        {
            GameObject go = (GameObject)Instantiate(hud.selectedPrefab,
                getChildObjectPosition(hud.selectedPrefab),
                new Quaternion());

            HUD.currentObject = go;
        }
        setObjectProperties(HUD.currentObject);*/
    }

    /// <summary>
    /// Handles the exiting of the collision zone of this tile
    /// </summary>
    public void OnMouseExit()
    {
        transform.renderer.material.color = originalColor;
    }

    /// <summary>
    /// Tries to place the selected building
    /// </summary>
    public void OaaanMouseDown()
    {
        if (occupyingObject != null)
        {
            return;
        }
        HUD hud = Camera.main.GetComponent<HUD>();
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            hud.selectedPrefab = null;
        }
        if (HUD.currentObject == null)
        {
            return;
        }
        Building b = HUD.currentObject.GetComponent<Building>();
        if (b == null)
        {
            return;
        }
        if (!b.CheckCost())
        {
            Destroy(HUD.currentObject);
            return;
        }
        b.DecreaseResources();
        occupyingObject = (GameObject) PhotonNetwork.Instantiate(HUD.currentObject.name.Replace("(Clone)", ""), HUD.currentObject.transform.position, HUD.currentObject.transform.rotation, 0);
        occupyingObject.transform.parent = transform;
        setObjectProperties(occupyingObject);
        occupyingObject.GetComponent<BoardLocation>().SetLocation(boardLocation.location.row, boardLocation.location.column);
        Destroy(HUD.currentObject);
        HUD.currState = HUD.ActionState.NO_ACTION;
    }

    /// <summary>
    /// Sets the properties of the passed object to the correct location
    /// </summary>
    /// <param name="go">The gameobject to set the properties of</param>
    public void setObjectProperties(GameObject go)
    {
        go.transform.position = getChildObjectPosition(go);
    }

    /// <summary>
    /// The position the child object of this tile should have
    /// </summary>
    /// <param name="child">The child</param>
    /// <returns>The child's position</returns>
    private Vector3 getChildObjectPosition(GameObject child)
    {
        return new Vector3(transform.position.x, renderer.bounds.size.y + child.renderer.bounds.size.y / 2, transform.position.z);
    }

    /// <summary>
    /// Method that is called when a turn ends
    /// </summary>
    public void TurnEnd()
    {
        print("TurnEnd");
    }

    /// <summary>
    /// Method called when a turn starts
    /// </summary>
    public void TurnStart()
    {
        print("TurnStart");
    }

    public void SetOccupyingObject(GameObject go)
    {
        this.setObjectProperties(go);
        this.occupyingObject = go;
        go.transform.parent = gameObject.transform.parent;
    }
}
