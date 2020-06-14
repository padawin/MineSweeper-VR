using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VibrationType { Gentle, Strong };

public class MineSweeperControl : MonoBehaviour
{
	[SerializeField] float gentleVibrationFrequency = 0.1f;
	[SerializeField] float gentleVibrationAmplitude = 0.1f;
	[SerializeField] float strongVibrationFrequency = 0.5f;
	[SerializeField] float strongVibrationAmplitude = 0.5f;
	[SerializeField] OVRInput.Controller controller;
	[SerializeField] OVRInput.RawButton button;

	bool controllerPressed = false;

	public void vibrate(VibrationType vibrationType) {
		float vibrationFrequency, vibrationAmplitude;
		if (vibrationType == VibrationType.Gentle) {
			vibrationFrequency = gentleVibrationFrequency;
			vibrationAmplitude = gentleVibrationAmplitude;
		}
		else {
			vibrationFrequency = strongVibrationFrequency;
			vibrationAmplitude = strongVibrationAmplitude;
		}
		OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, controller);
		StartCoroutine("stopVibrating", controller);
	}

	IEnumerator stopVibrating(OVRInput.Controller controller) {
		yield return new WaitForSeconds(0.1f);
		OVRInput.SetControllerVibration(0.0f, 0.0f, controller);
	}

	public bool isButtonPressed() {
		return controllerPressed;
	}

	private void Update() {
		controllerPressed = OVRInput.Get(button, controller);
	}
}
