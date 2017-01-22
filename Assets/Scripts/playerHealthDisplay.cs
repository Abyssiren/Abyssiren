using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthDisplay : MonoBehaviour {

	public PlayerHealth playerHealth;
	private Image img;
	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		img.fillAmount = playerHealth.currHealth / playerHealth.maxHealth;
	}
}
