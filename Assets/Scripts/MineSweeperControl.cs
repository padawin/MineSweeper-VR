using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperControl : MonoBehaviour
{
	[SerializeField] float vibrationFrequency = 0.5f;
	[SerializeField] float vibrationAmplitude = 0.5f;
	[SerializeField] bool cleanAfterExecution = false;
	[SerializeField] OVRInput.Controller controller;
	[SerializeField] OVRInput.RawButton button;

	List<MineSweeperCell> hoveredCells = new List<MineSweeperCell>();
	bool executed = false;

	void OnTriggerEnter(Collider other) {
		MineSweeperCell cell = other.GetComponent<MineSweeperCell>();
		cell.setHovered(true);
		hoveredCells.Add(cell);
	}

	void OnTriggerExit(Collider other) {
		MineSweeperCell cell = other.GetComponent<MineSweeperCell>();
		cell.setHovered(false);
		hoveredCells.Remove(cell);
	}

	public void execute(System.Action<MineSweeperCell> callback) {
		bool controllerPressed = OVRInput.Get(button, controller);
		if (!executed && controllerPressed) {
			if (hoveredCells.Count == 0) {
				return;
			}
			foreach (var cell in hoveredCells) {
				callback(cell);
			}
			if (hoveredCells.Count > 0) {
				OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, controller);
				StartCoroutine("stopVibrating", controller);
			}
			if (cleanAfterExecution) {
				hoveredCells.Clear();
			}
			executed = true;
		}
		else if (!controllerPressed) {
			executed = false;
		}
	}

	IEnumerator stopVibrating(OVRInput.Controller controller) {
		yield return new WaitForSeconds(0.1f);
		OVRInput.SetControllerVibration(0.0f, 0.0f, controller);

	}
}
