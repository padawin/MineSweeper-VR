using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealCell : MonoBehaviour
{
	[SerializeField] float vibrationFrequency = 0.5f;
	[SerializeField] float vibrationAmplitude = 0.5f;
	MineSweeperGrid grid;
	MineSweeperControl playerControl;

	private void Start() {
		playerControl = GetComponent<MineSweeperControl>();
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
	}

	private void Update() {
		playerControl.execute(reveal, OVRInput.Controller.RTouch, OVRInput.RawButton.RIndexTrigger);
	}

	public void reveal(MineSweeperCell cell) {
		if (cell.getState() == CellState.initial) {
			grid.revealCell(cell);
		}
	}
}
