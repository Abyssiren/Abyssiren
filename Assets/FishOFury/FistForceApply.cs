using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistForceApply : MonoBehaviour {
	public static float forceAmount = 10000000f;
	public forceField forceField;
	public GameObject movingParentObj;
	public Vector3 oldPosition;

	void Start(){
		oldPosition = (movingParentObj.transform.position);
	}
	void FixedUpdate(){
		//Debug.Log ("VELO - "+GetComponent<Rigidbody>().velocity.magnitude);
		if((movingParentObj.transform.position - oldPosition).magnitude/Time.fixedDeltaTime >1f){
			forceField.turnOnForceField((movingParentObj.transform.position - oldPosition).normalized);
		}
		oldPosition = (movingParentObj.transform.position);
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Fish") {
			Debug.Log ("COLLIDED - "+col.gameObject.name);
			col.rigidbody.AddForce (gameObject.GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime * forceAmount);
		}
	}
}
