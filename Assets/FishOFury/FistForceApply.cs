using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistForceApply : MonoBehaviour {
	public static float forceAmount = 10000000f;
	public forceField forceField;
	void Update(){
		if(GetComponent<Rigidbody>().velocity.magnitude > 1f){
			forceField.turnOnForceField(GetComponent<Rigidbody>().velocity.normalized);
		}
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Fish") {
			Debug.Log ("COLLIDED - "+col.gameObject.name);
			col.rigidbody.AddForce (gameObject.GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime * forceAmount);
		}
	}
}
