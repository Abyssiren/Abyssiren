using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class damageIndicatorScreen : MonoBehaviour {
	Image img;
	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		img.color = new Color (img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime*2));
	}
}
