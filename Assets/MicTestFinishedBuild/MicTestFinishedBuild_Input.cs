﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicTestFinishedBuild_Input : MonoBehaviour {

	//input,microphone var
	private static int MicSampleTime_s = 1;
	private static int MicSampleFrequency_perS = 44100;
	private bool _isInitialized;

	//sampling, audio clip var
	private static AudioClip _clipRecord;
	private static int _VolumeSampleWindow = 1000; //1 centi-secnond of sample window
	private bool beginningBuffer;

	//boundingVar
	public static float maxVolLevel = float.PositiveInfinity;
	public static float minVolLevel = float.NegativeInfinity;

	//volume var
	public static float volumeVal;

	public Slider sensitivity_slider;
	public static float Volume_Sensitivity = 1f;

	void InitMic()
	{
		_clipRecord = Microphone.Start (null, true, MicSampleTime_s, MicSampleFrequency_perS);
		beginningBuffer = true;
		Debug.Log ("MIC INIT FINISHED");
	}

	void StopMicrophone()
	{
		Microphone.End (null);
		Debug.Log ("MIC STOPPED");
	}
		
	public void setSensitivity(){
		Volume_Sensitivity = sensitivity_slider.value;
	}


	public float getRawVolume(){
		float[] sampleWinodw = new float[_VolumeSampleWindow];

		int samplePosition = (int)(Microphone.GetPosition (null) - _VolumeSampleWindow - 1);
		if (samplePosition < 0) {
			if (beginningBuffer) {
				return 0f;
			} else {
				samplePosition = samplePosition + (MicSampleTime_s * MicSampleFrequency_perS);
			}
		}

		_clipRecord.GetData (sampleWinodw, (int)(Microphone.GetPosition(null) - _VolumeSampleWindow -1));
		float rawVolume = 0;
		for (int i = 0; i < _VolumeSampleWindow; i++) {
			rawVolume = rawVolume + Mathf.Abs( sampleWinodw [i]);
		}
		rawVolume = rawVolume / _VolumeSampleWindow;
		Debug.Log ("rawVolume - "+rawVolume);
		return rawVolume;
	}

	public void setVolumeVal(){
		
		volumeVal = getRawVolume () * Volume_Sensitivity;
	}

	void Update()
	{
		setSensitivity ();
		setVolumeVal ();

	}

	void OnEnable()
	{
		InitMic ();
		_isInitialized = true;
	}

	void OnDisable()
	{
		StopMicrophone ();
	}

	void OnDestory()
	{
		StopMicrophone ();
	}

	void OnApplicationFocus(bool focus)
	{
		if (focus) {
			if (!_isInitialized) {
				InitMic ();
				_isInitialized = true;
			}
		}

		if (!focus) {
			StopMicrophone ();
			_isInitialized = false;
		}
	}

}
