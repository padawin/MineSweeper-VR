using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperCellInteractor : MonoBehaviour
{
	MineSweeperControl control;
	[SerializeField] bool cleanAfterExecution = false;

	List<MineSweeperCell> hoveredCells = new List<MineSweeperCell>();
	bool executed = false;

    void Start() {
		control = GetComponent<MineSweeperControl>();
    }

	void OnTriggerEnter(Collider other) {
		MineSweeperCell cell = other.GetComponent<MineSweeperCell>();
		cell.setHovered(true);
		hoveredCells.Add(cell);
		control.vibrate(VibrationType.Gentle);
	}

	void OnTriggerExit(Collider other) {
		MineSweeperCell cell = other.GetComponent<MineSweeperCell>();
		cell.setHovered(false);
		hoveredCells.Remove(cell);
		control.vibrate(VibrationType.Gentle);
	}

	public void execute(System.Action<MineSweeperCell> callback) {
		bool buttonPressed = control.isButtonPressed();
		if (!executed && buttonPressed) {
			if (hoveredCells.Count == 0) {
				return;
			}
			foreach (var cell in hoveredCells) {
				callback(cell);
			}
			if (hoveredCells.Count > 0) {
				control.vibrate(VibrationType.Strong);
			}
			if (cleanAfterExecution) {
				hoveredCells.Clear();
			}
			executed = true;
		}
		else if (!buttonPressed) {
			executed = false;
		}
	}
}
