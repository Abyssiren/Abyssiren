using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MicTestBtnInputDetectObj : MonoBehaviour {
	public bool upButtonPressedDown = false;
	public bool downButtonPressedDown = false;

	private Valve.VR.EVRButtonId upButton = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
	private Valve.VR.EVRButtonId downButton = Valve.VR.EVRButtonId.k_EButton_DPad_Down;

	public Slider sensitivitySlider;
	private SteamVR_Controller.Device controller {

		get { return SteamVR_Controller.Input((int)trackedObj.index);

		}

	}

	private SteamVR_TrackedObject trackedObj;

	void Start() {

		trackedObj = GetComponent<SteamVR_TrackedObject>();

	}

	void Update() {

		if (controller == null) {

			Debug.Log("Controller not initialized");

			return;

		}

		upButtonPressedDown = (controller.GetPress(upButton) || Input.GetKey(KeyCode.UpArrow));
		downButtonPressedDown = (controller.GetPress(downButton)|| Input.GetKey(KeyCode.DownArrow));


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
