using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtilTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<ColorEnum> colorList = ColorUtil.getListOfAvailableColor ();
		for (int i = 0; i < colorList.Count; i++) {
			Debug.Log ("ColorList - " + i+ " - "+colorList[i]);
		}
		Debug.Log ("Random Color - "+ColorUtil.getRandomAvailableColor ());
		Debug.Log ("Random Color - "+ColorUtil.getRandomAvailableColor ());
		Debug.Log ("Random Color - "+ColorUtil.getRandomAvailableColor ());
		Debug.Log ("Random Color - "+ColorUtil.getRandomAvailableColor ());
		for (int i = 0; i < ColorUtil.MaxColorAllowed; i++) {
			Debug.Log ("rangeFromKey - " + i+ " - "+ColorUtil.getMaxVolumeRangeFromKey(i));
		}
		for (float i = 0.01f; i < 1f; i = i+0.1f) {
			Debug.Log ("getColorFromVolume - " + i+ " - "+ColorUtil.getColorFromVolume(i));
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
