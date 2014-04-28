using UnityEngine;
using System.Collections;

public class TileControl : MonoBehaviour {
    /// <summary>
    /// The row this tile is on
    /// </summary>
    public int r;

    /// <summary>
    /// The column this tile is on
    /// </summary>
    public int c;

    /// <summary>
    /// The object currntly occupying this tile
    /// </summary>
    public GameObject occupyingObject = null;

    /// <summary>
    /// TODO: Remove
    /// </summary>
    private Color originalColor;

    /// <summary>
    /// Used for intialization
    /// </summary>
    public void start()
    {
        originalColor = transform.renderer.material.color;
    }

    /// <summary>
    /// Handles the mouse cursor entering the bounds of this tile
    /// Also handles building placement preview
    /// </summary>
    public void OnMouseEnter()
    {
        if (occupyingObject != null)
        {
            return;
        }
        transform.renderer.material.color = Color.cyan;
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
        setObjectProperties(ref HUD.currentObject);
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
    public void OnMouseDown()
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
        occupyingObject = HUD.currentObject;
        HUD.currentObject = null;
        HUD.currState = HUD.ActionState.NO_ACTION;
    }

    /// <summary>
    /// Sets the properties of the passed object to the correct location
    /// </summary>
    /// <param name="go">The gameobject to set the properties of</param>
    private void setObjectProperties(ref GameObject go)
    {
        go.transform.position = getChildObjectPosition(go);
        go.GetComponent<Building>().c = c;
        go.GetComponent<Building>().r = r;
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
}
