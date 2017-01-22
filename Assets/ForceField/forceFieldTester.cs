using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceFieldTester : MonoBehaviour {
	public forceField ff;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {
			ff.turnOnForceField (new Vector3(0,-1,0));
		}
	}
}
