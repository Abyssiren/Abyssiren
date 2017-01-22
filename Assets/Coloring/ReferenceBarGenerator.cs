using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceBarGenerator : MonoBehaviour {
	public GameObject Prefab_BarBase;
	// Use this for initialization
	void Start () {

		GameObject barBaseNew;
		int j = 0;
		for (int i = ColorUtil.MaxColorAllowed-1; i >=0; i--) {
			barBaseNew = this.transform.GetChild (j).gameObject;
			barBaseNew.GetComponent<Image> ().color = ColorUtil.getUIImageColorClassFromColorEnum (ColorUtil.getColorFromKey(i));
			barBaseNew.GetComponent<Image> ().fillAmount = ColorUtil.getMaxVolumeRangeFromKey (i);
			barBaseNew.SetActive (true);
			j++;
		}
		//base
		barBaseNew = this.transform.GetChild (ColorUtil.MaxColorAllowed).gameObject;
		barBaseNew.GetComponent<Image> ().color = Color.white;
		barBaseNew.GetComponent<Image> ().fillAmount = Projectile.shootVolumeTreshold;
		barBaseNew.SetActive (true);

	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
