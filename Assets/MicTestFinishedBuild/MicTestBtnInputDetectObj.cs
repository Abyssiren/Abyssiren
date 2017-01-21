using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MicTestBtnInputDetectObj : MonoBehaviour {
	public bool upButtonPressedDown = false;
	public bool downButtonPressedDown = false;


	public Slider sensitivitySlider;
	SteamVR_Controller.Device device;
	private SteamVR_Controller.Device controller {

		get { return SteamVR_Controller.Input((int)trackedObj.index);

		}

	}

	private SteamVR_TrackedObject trackedObj;

	void Start() {

		trackedObj = GetComponent<SteamVR_TrackedObject>();
		device = SteamVR_Controller.Input ((int)trackedObj.index);
	}

	void Update() {

		if (controller == null) {

			Debug.Log("Controller not initialized");

			return;

		}

		upButtonPressedDown = (( device.GetPress (SteamVR_Controller.ButtonMask.Touchpad) && device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f ) || Input.GetKey(KeyCode.UpArrow));
		downButtonPressedDown = (( device.GetPress (SteamVR_Controller.ButtonMask.Touchpad) && device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < 0.5f ) || Input.GetKey(KeyCode.DownArrow));

		if (upButtonPressedDown) {
			Debug.Log("UP");
			deltaSensitivity (1);
		}

		if (downButtonPressedDown) {
			Debug.Log("DOWN");
			deltaSensitivity (-1);
		}

	}

	void deltaSensitivity(int d){
		sensitivitySlider.value = sensitivitySlider.value + d;
	}

}
