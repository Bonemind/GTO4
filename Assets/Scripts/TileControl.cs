using UnityEngine;
using System.Collections;

public class TileControl : Photon.MonoBehaviour
{
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
        originalColor = gameObject.renderer.material.color;
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
            return;
        }
        b.DecreaseResources();
        occupyingObject = (GameObject) PhotonNetwork.Instantiate(HUD.currentObject.name.Replace("(Clone)", ""), HUD.currentObject.transform.position, HUD.currentObject.transform.rotation, 0);
        //occupyingObject = (GameObject) Instantiate(HUD.currentObject, HUD.currentObject.transform.position, HUD.currentObject.transform.rotation);
        
        print(HUD.currentObject.name.Replace("(Clone)", ""));
        print(occupyingObject);
        //occupyingObject = HUD.currentObject;
        occupyingObject.transform.parent = transform;
        Destroy(HUD.currentObject);
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

    public void TurnEnd()
    {
        print("TurnEnd");
    }
    public void TurnStart()
    {
        print("TurnStart");
    }
}
