using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealCell : MonoBehaviour
{
	MineSweeperGrid grid;
	MineSweeperCellInteractor cellInteractor;

	private void Start() {
		cellInteractor = GetComponent<MineSweeperCellInteractor>();
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
	}

	private void Update() {
		if (grid.isActive()) {
			cellInteractor.execute(reveal, grid.revealNeighbours);
		}
	}

	public void reveal(MineSweeperCell cell) {
		if (cell.getState() == CellState.initial) {
			grid.revealCell(cell);
		}
	}
}
