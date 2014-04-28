using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
    public GameObject currentObject = null;

    public GameObject selectedPrefab = null;

    public GameObject[] prefabs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedPrefab = prefabs[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedPrefab = prefabs[1];
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentObject != null)
            {
                Destroy(currentObject);
            }
            selectedPrefab = null;
        }
	
	}
}
