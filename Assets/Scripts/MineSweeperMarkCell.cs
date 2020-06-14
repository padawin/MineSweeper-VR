using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperMarkCell : MonoBehaviour
{
	MineSweeperGrid grid;
	MineSweeperCellInteractor cellInteractor;
	[SerializeField] TextMesh mineCounter;

	private void Start() {
		cellInteractor = GetComponent<MineSweeperCellInteractor>();
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
		mineCounter.text = grid.getCountUnfoundMines().ToString();
	}

	private void Update() {
		if (grid.isActive()) {
			cellInteractor.execute(mark);
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
