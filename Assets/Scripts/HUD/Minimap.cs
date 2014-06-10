using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
    public float FollowHeight = 30f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currPos = transform.position;
        Vector3 cameraPos = Camera.main.transform.position;
        currPos.x = cameraPos.x;
        currPos.z = cameraPos.z;
        currPos.y = cameraPos.y + FollowHeight;
        transform.position = currPos;
	}
}
