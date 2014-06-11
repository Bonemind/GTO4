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
    #pragma warning disable 0414
    private BoardLocation boardLocation;
    #pragma warning restore 0414
    private float originalHeight;
    private bool highlighted = false;

    /// <summary>
    /// Used for intialization
    /// </summary>
    public void Start()
    {
        originalColor = gameObject.renderer.material.color;
        boardLocation = gameObject.GetComponent<BoardLocation>();
        originalHeight = transform.position.y;
    }

    /// <summary>
    /// Handles the mouse cursor entering the bounds of this tile
    /// Also handles building placement preview
    /// </summary>
    public void OnMouseEnter()
    {
        transform.renderer.material.color = Color.cyan;
    }
    /// <summary>
    /// Handles the exiting of the collision zone of this tile
    /// </summary>
    public void OnMouseExit()
    {
        transform.renderer.material.color = originalColor;
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

    public void Highlight()
    {
        if (highlighted)
        {
            return;
        }
        Vector3 pos = transform.position;
        pos.y += 0.5f;
        this.transform.position = pos;
        highlighted = true;
    }

    public void UnHighlight()
    {
        Vector3 pos = transform.position;
        pos.y = originalHeight;
        this.transform.position = pos;
        highlighted = false;
    }
}
