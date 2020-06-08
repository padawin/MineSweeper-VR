using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperMarkCell : MonoBehaviour
{
	MineSweeperGrid grid;
	MineSweeperControl playerControl;
	[SerializeField] TextMesh mineCounter;

	private void Start() {
		playerControl = GetComponent<MineSweeperControl>();
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
		mineCounter.text = grid.getCountUnfoundMines().ToString();
	}

	private void Update() {
		if (grid.isActive()) {
			playerControl.execute(mark);
		}
	}

	private void mark(MineSweeperCell cell) {
		if (cell.getState() == CellState.flagged) {
			grid.removeFoundMine();
		}
		cell.mark();
		if (cell.getState() == CellState.flagged) {
			grid.addFoundMine();
		}
		mineCounter.text = grid.getCountUnfoundMines().ToString();
	}
}
