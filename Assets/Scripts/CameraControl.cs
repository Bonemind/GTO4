using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    /// <summary>
    /// The speed the camera moves at
    /// </summary>
    public float cameraSpeed = 5f;

    /// <summary>
    /// The speed the camera rotates at
    /// </summary>
    public float cameraRotationSpeed = 2f;

    /// <summary>
    /// The percentage of the screen border that will trigger mouse panning
    /// </summary>
    public float mouseBorderZonePercentage = 0.05f;

    /// <summary>
    /// The minimum distance the mouse needs to move before it is seen as a rotation command
    /// </summary>
    public float minRotationDistance = 5f;

    /// <summary>
    /// The speed the camera zooms in at
    /// </summary>
    public float zoomSpeed = 1.5f;

    /// <summary>
    /// The last place the mouse was when middle mouse button was pressed
    /// </summary>
    private Vector3 lastMidMousePos = Vector3.zero;	
	// Update is called once per frame
	public void Update () {
        keyboardMovement();
        mouseMovement();
	}

    /// <summary>
    /// Handles movement using keyboard
    /// </summary>
    private void keyboardMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            normalizeAndMove(transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            normalizeAndMove(-transform.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            normalizeAndMove(-transform.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            normalizeAndMove(transform.right);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 currAngle = transform.eulerAngles;
            currAngle.y += cameraRotationSpeed;
            transform.rotation = Quaternion.Euler(currAngle);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 currAngle = transform.eulerAngles;
            currAngle.y -= cameraRotationSpeed;
            transform.rotation = Quaternion.Euler(currAngle);
        }
    }

    /// <summary>
    /// Handles mouse controlling of the camera
    /// </summary>
    private void mouseMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        int w = Screen.width;
        int h = Screen.height;
        if (h - mousePos.y <= mouseBorderZonePercentage * h)
        {
            normalizeAndMove(transform.forward);
        }
        if (mousePos.y <= mouseBorderZonePercentage * h)
        {
            normalizeAndMove(-transform.forward);
        }
        if (mousePos.x <= mouseBorderZonePercentage * w)
        {
            normalizeAndMove(-transform.right);
        }
        if (w - mousePos.x <= mouseBorderZonePercentage * w)
        {
            normalizeAndMove(transform.right);
        }

        //Handle middle mouse button rotation
        if (Input.GetMouseButton(2))
        {
            handleMiddleClick();
        }
        else
        {
            lastMidMousePos = Vector3.zero;
        }

        //Handle zooming
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAxis != 0)
        {
            Vector3 currPos = transform.position;
            //Negate scrollaxis so zooming doesn't feel inverted
            currPos.y += zoomSpeed * -scrollAxis;
            transform.position = currPos;
            Debug.Log(transform.position);
        }
    }

    /// <summary>
    /// Normalizes movement vector and ignores y displacement
    /// </summary>
    /// <param name="direction">The direction we want to move into</param>
    private void normalizeAndMove(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        transform.Translate(direction * Time.deltaTime * cameraSpeed, Space.World);
    }

    /// <summary>
    /// Handles the clicking and moving of the middlemousebutton
    /// User to control camera rotation
    /// </summary>
    private void handleMiddleClick()
    {
        if (lastMidMousePos == Vector3.zero)
        {
            lastMidMousePos = Input.mousePosition;
            return;
        }
        Vector3 currPos = Input.mousePosition;
        float xdiff = currPos.x - lastMidMousePos.x;
        if (Mathf.Abs(xdiff) < minRotationDistance)
        {
            return;
        }

        if (xdiff < 0)
        {
            Vector3 currAngle = transform.eulerAngles;
            currAngle.y -= cameraRotationSpeed;
            transform.rotation = Quaternion.Euler(currAngle);
        }
        else
        {
            Vector3 currAngle = transform.eulerAngles;
            currAngle.y += cameraRotationSpeed;
            transform.rotation = Quaternion.Euler(currAngle);
        }
        lastMidMousePos = currPos;   
    }
}
