using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceField : MonoBehaviour {
	public GameObject locationPoint;
	public Vector3 forceFieldDirection = new Vector3(1,0,0);
	public float forceMagnitude = 250000f;
	List<GameObject> listOfFishAffected;
	public float timeLeft;
	// Use this for initialization
	void Start () {
		//turnOnForceField (new Vector3(1,0,0));
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft = timeLeft - Time.deltaTime;
		if (timeLeft < 0f) {
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Fish" && other.gameObject.name.Equals ("fishhead")) {
			other.gameObject.GetComponent<forceFieldReceiver> ().enableForceField(forceFieldDirection * forceMagnitude);
		}
	}


	public void turnOnForceField(Vector3 newDirection){
		if (!gameObject.activeSelf) {
			transform.position = locationPoint.transform.position;
			//transform.position = locationPoint.transform.position;
			transform.rotation = Quaternion.LookRotation (newDirection);
			//		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90f);
			gameObject.SetActive (true);
			timeLeft = 20f;
		}
	}
}
