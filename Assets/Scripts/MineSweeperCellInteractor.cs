using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperCellInteractor : MonoBehaviour
{
	MineSweeperControl control;
	[SerializeField] bool cleanAfterExecution = false;

	List<MineSweeperCell> hoveredCells = new List<MineSweeperCell>();
	List<MineSweeperRevealedCell> hoveredNeighbourIndicators = new List<MineSweeperRevealedCell>();

	bool executed = false;

    void Start() {
		control = GetComponent<MineSweeperControl>();
    }

	void OnTriggerEnter(Collider other) {
		if (1 << other.gameObject.layer == LayerMask.GetMask("cells")) {
			addCellToHovered(other.GetComponent<MineSweeperCell>());
		}
		else {
			addNeighbourIndicatorToHovered(other.GetComponent<MineSweeperRevealedCell>());
		}
	}

	void OnTriggerExit(Collider other) {
		if (1 << other.gameObject.layer == LayerMask.GetMask("cells")) {
			removeCellFromHovered(other.GetComponent<MineSweeperCell>());
		}
		else {
			removeNeighbourIndicatorFromHovered(other.GetComponent<MineSweeperRevealedCell>());
		}
	}

	void addCellToHovered(MineSweeperCell cell) {
		cell.setHovered(true);
		hoveredCells.Add(cell);
		control.vibrate(VibrationType.Gentle);
	}

	void removeCellFromHovered(MineSweeperCell cell) {
		cell.setHovered(false);
		hoveredCells.Remove(cell);
		control.vibrate(VibrationType.Gentle);
	}

	void addNeighbourIndicatorToHovered(MineSweeperRevealedCell obj) {
		obj.setHovered(true);
		hoveredNeighbourIndicators.Add(obj);
		control.vibrate(VibrationType.Gentle);
	}

	void removeNeighbourIndicatorFromHovered(MineSweeperRevealedCell obj) {
		obj.setHovered(false);
		hoveredNeighbourIndicators.Remove(obj);
		control.vibrate(VibrationType.Gentle);
	}

	public void execute(System.Action<MineSweeperCell> cellCallback, System.Action<MineSweeperRevealedCell> neighbourIndicatorCallback) {
		bool buttonPressed = control.isButtonPressed();
		if (!executed && buttonPressed) {
			bool hasHoveredCells = hoveredCells.Count > 0;
			executeOnCells(cellCallback);
			if (!hasHoveredCells && neighbourIndicatorCallback != null) {
				executeOnNeighbourIndicators(neighbourIndicatorCallback);
			}
			executed = true;
		}
		else if (!buttonPressed) {
			executed = false;
		}
	}

	void executeOnCells(System.Action<MineSweeperCell> callback) {
		foreach (var cell in hoveredCells) {
			callback(cell);
		}
		if (hoveredCells.Count > 0) {
			control.vibrate(VibrationType.Strong);
		}
		if (cleanAfterExecution) {
			hoveredCells.Clear();
		}
	}

	void executeOnNeighbourIndicators(System.Action<MineSweeperRevealedCell> callback) {
		foreach (var neighbourIndicator in hoveredNeighbourIndicators) {
			callback(neighbourIndicator);
		}
		if (hoveredNeighbourIndicators.Count > 0) {
			control.vibrate(VibrationType.Strong);
		}
	}
}
