﻿using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * 10);
	}
}
