using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtil : MonoBehaviour {
	public static int MaxColorAllowed = 3;
	public static Dictionary<int, ColorEnum> colorDict = new Dictionary<int, ColorEnum>(){
		{0, ColorEnum.BLUE},
		{1, ColorEnum.GREEN},
		{2, ColorEnum.YELLOW},
		{3, ColorEnum.RED}
	};


	public static List<ColorEnum> getListOfAvailableColor(){
		List<ColorEnum> result = new List<ColorEnum> ();
		for (int i = 0; i < MaxColorAllowed; i++) {
			result.Add (colorDict [i]);
		}
		return result;
	}

	public static ColorEnum getRandomAvailableColor(){
		List<ColorEnum> colorList = getListOfAvailableColor ();
		return colorList[Random.Range (0, colorList.Count)];
	}

	public static float getMaxVolumeRangeFromKey(int key){
		float VolumeStepSize = (1f - Projectile.shootVolumeTreshold) / MaxColorAllowed;
		return VolumeStepSize*(key+1)+Projectile.shootVolumeTreshold;
	}


	public static ColorEnum getColorFromKey(int colorKey){
        Debug.Log(colorKey + " colorkey");
		if (colorKey < MaxColorAllowed && colorKey>=0) {
			return colorDict [colorKey];
		} else {
            Debug.Log(colorKey + " colorkey err");
            return ColorEnum.ERROR;
		}
	}
	public static int getKeyFromVolume(float volume){
		if (volume < Projectile.shootVolumeTreshold || volume>1f) {
			return -1;
		} else {
			float VolumeStepSize = (1f - Projectile.shootVolumeTreshold) / MaxColorAllowed;
            int ret = (int)((volume - Projectile.shootVolumeTreshold) / VolumeStepSize);
            if (ret >= MaxColorAllowed)
                ret = MaxColorAllowed-1;
            return ret;
		}
	}

	public static ColorEnum getColorFromVolume(float volume){
		return getColorFromKey (getKeyFromVolume (volume));
	}


	public static Color getUIImageColorClassFromColorEnum(ColorEnum ce){
		switch (ce) {
		case ColorEnum.RED:
			return Color.red;
		case ColorEnum.GREEN:
			return Color.green;
		case ColorEnum.BLUE:
			return Color.blue;
		case ColorEnum.YELLOW:
			return Color.yellow;
		}
		return Color.black;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
