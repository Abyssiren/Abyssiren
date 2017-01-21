using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedBuildVolumeDisplay : MonoBehaviour {
	Image img;
	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		img.fillAmount = MicTestFinishedBuild_Input.volumeVal;
	}
}
