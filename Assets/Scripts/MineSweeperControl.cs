using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperControl : MonoBehaviour
{
	[SerializeField] float vibrationFrequency = 0.5f;
	[SerializeField] float vibrationAmplitude = 0.5f;
	List<MineSweeperCell> hoveredCells = new List<MineSweeperCell>();

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

	public void reveal(MineSweeperGrid grid) {
		void callback(MineSweeperCell cell) {
			grid.revealCell(cell);
		}
		execute(callback);
	}

	public void mark() {
		void callback(MineSweeperCell cell) {
			cell.mark();
		}
		execute(callback);
	}

	public void execute(System.Action<MineSweeperCell> callback) {
		foreach (var cell in hoveredCells) {
			callback(cell);
		}
		if (hoveredCells.Count > 0) {
			vibrate();
		}
		hoveredCells.Clear();
	}

	public List<MineSweeperCell> getHoveredCells() {
		return hoveredCells;
	}

	public void vibrate() {
		OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, OVRInput.Controller.RTouch);
		StartCoroutine("stopVibrating");
	}

	IEnumerator stopVibrating() {
		yield return new WaitForSeconds(0.1f);
		OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.RTouch);

	}
}
