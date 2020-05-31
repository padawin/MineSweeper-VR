using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealCell : MonoBehaviour
{
	[SerializeField] float vibrationFrequency = 0.5f;
	[SerializeField] float vibrationAmplitude = 0.5f;
	MineSweeperGrid grid;
	List<MineSweeperCell> hoveredCells = new List<MineSweeperCell>();

	private void Start() {
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
	}

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

	private void Update() {
		if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch)) {
			foreach (var cell in hoveredCells) {
				grid.revealCell(cell);
			}
			if (hoveredCells.Count > 0) {
				OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, OVRInput.Controller.RTouch);
				StartCoroutine("stopVibrating");
			}
			hoveredCells.Clear();
		}
	}

	IEnumerator stopVibrating() {
		yield return new WaitForSeconds(0.1f);
		OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.RTouch);

	}
}
