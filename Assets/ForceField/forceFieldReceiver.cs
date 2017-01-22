using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceFieldReceiver : MonoBehaviour {

	public float TimeLeft;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TimeLeft = TimeLeft - Time.deltaTime;
		if (TimeLeft < 0f) {
			disableForceField ();
		}
	}

	public void enableForceField(Vector3 newForceVector) {
		GetComponent<FishEnemyController> ().enabled = false;
		GetComponent<ConstantForce> ().enabled = true;
		GetComponent<ConstantForce> ().force = newForceVector;
		TimeLeft = 4f;
	}

	public void disableForceField() {
		GetComponent<FishEnemyController> ().enabled = true;
		GetComponent<ConstantForce> ().enabled = false;
	}
}
