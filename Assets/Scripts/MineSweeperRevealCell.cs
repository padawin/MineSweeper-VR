using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealCell : MonoBehaviour
{
	MineSweeperGrid grid;
	MineSweeperControl playerControl;

	private void Start() {
		playerControl = GetComponent<MineSweeperControl>();
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
	}

	private void Update() {
		if (grid.isActive()) {
			playerControl.execute(reveal);
		}
	}

	public void reveal(MineSweeperCell cell) {
		if (cell.getState() == CellState.initial) {
			grid.revealCell(cell);
		}
	}
}
