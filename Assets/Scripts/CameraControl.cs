using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    /// <summary>
    /// The speed the camera moves at
    /// </summary>
    public float cameraSpeed = 2f;

    public float mouseBorderZonePercentage = 0.1f;

	// Use this for initialization
	public void Start () {
	
	}
	
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
            normalizeAndMove(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            normalizeAndMove(Vector3.back);
        }
        if (Input.GetKey(KeyCode.A))
        {
            normalizeAndMove(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            normalizeAndMove(Vector3.right);
        }
    }

    private void mouseMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        int w = Screen.width;
        int h = Screen.height;
        if (Mathf.Abs(h - mousePos.y) <= mouseBorderZonePercentage * h)
        {
            normalizeAndMove(Vector3.forward);
        }
        if (mousePos.y <= mouseBorderZonePercentage * h)
        {
            normalizeAndMove(Vector3.back);
        }

        if (Mathf.Abs(w - mousePos.x) <= mouseBorderZonePercentage * w)
        {
            normalizeAndMove(Vector3.right);
        }
        if (mousePos.x <= mouseBorderZonePercentage * w)
        {
            normalizeAndMove(Vector3.left);
        }
        Debug.Log(h - mousePos.y);
        Debug.Log(mouseBorderZonePercentage * h);
    }

    private void normalizeAndMove(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        transform.Translate(direction * Time.deltaTime * cameraSpeed, Space.World);
    }
}
