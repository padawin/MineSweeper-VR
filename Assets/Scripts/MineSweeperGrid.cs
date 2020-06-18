using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct GridCoordinate {
	public int x;
	public int y;
	public int z;
	public GridCoordinate(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
}

public class MineSweeperGrid : MonoBehaviour {
	[SerializeField] GameObject cellPrefab;
	[SerializeField] GameObject revealedCellPrefab;
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] int depth;
	[SerializeField] float cellSpacing;
	[SerializeField] int minesCount = 0;
	[SerializeField] float initialDistanceFromPlayer = 2.0f;

	MineSweeperContext context;

	List<MineSweeperCell> cells = new List<MineSweeperCell>();
	List<int> mines = new List<int>();
	int minesFound = 0;

	// Set to true when the player destroys or marks a mine for the first time
	bool acted = false;
	bool active = true;

	public void init() {
		context = GameObject.FindGameObjectWithTag("gameContext").GetComponent<MineSweeperContext>();
		positionGrid();
		instanciateCells();
	}

	public bool isActive() {
		return active;
	}

	void deactivate() {
		active = false;
	}

	public void translate(Vector3 translateVector) {
		transform.position = transform.position + translateVector;
	}

	public void rotate(float left, float up) {
		transform.RotateAround(transform.position, Vector3.left, left);
		transform.RotateAround(transform.position, Vector3.up, up);
	}

	private void positionGrid() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		float cellDepth = cellPrefab.transform.localScale.z;
		float initialZ = initialDistanceFromPlayer + (depth / 2) * (cellDepth + cellSpacing);
		transform.position = new Vector3(
			player.transform.position.x,
			player.transform.position.y,
			player.transform.position.z + initialZ
		);
	}

	int getSize() {
		return width * height * depth;
	}

	public int getWidth() {
		return width;
	}

	public int getHeight() {
		return height;
	}

	private int getCellIndex(int x, int y, int z) {
		return z * (width * height) + y * width + x;
	}

	private GridCoordinate getCoords(int index) {
		int z = index / (width * height);
		int yx = index % (width * height);
		int y = yx / width;
		int x = yx % width;
		GridCoordinate coords = new GridCoordinate(x, y, z);
		return coords;
	}

	void instanciateCells() {
		float cellWidth = cellPrefab.transform.localScale.x;
		float cellHeight = cellPrefab.transform.localScale.y;
		float cellDepth = cellPrefab.transform.localScale.z;
		float startX = -(cellWidth * width / 2);
		float startY = -(cellHeight * height / 2);
		float startZ = -(cellDepth * depth / 2);
		int gridSize = getSize();
		for (int i = 0; i < gridSize; i++) {
			GridCoordinate coords = getCoords(i);
			GameObject cell = Instantiate(cellPrefab, transform.position, transform.rotation);
			cell.transform.parent = gameObject.transform;
			cell.transform.localPosition = new Vector3(
				startX + coords.x * (cellWidth + cellSpacing),
				startY + coords.y * (cellHeight + cellSpacing),
				startZ + coords.z * (cellDepth + cellSpacing)
			);
			MineSweeperCell cellComponent = cell.GetComponent<MineSweeperCell>();
			cellComponent.setCoordinates(coords.x, coords.y, coords.z);
			cells.Add(cellComponent);
		}
	}

	public void revealNeighbours(MineSweeperRevealedCell cell) {
		int x = cell.getOriginalCell().getX();
		int y = cell.getOriginalCell().getY();
		int z = cell.getOriginalCell().getZ();
		int countMarkedNeighbours = 0;
		List<MineSweeperCell> unmarkedNeighbours = new List<MineSweeperCell>();
		foreach (GridCoordinate coords in neighbourCoordinates(x, y, z)) {
			MineSweeperCell neighbour = cells[getCellIndex(coords.x, coords.y, coords.z)];
			if (neighbour.getState() == CellState.initial) {
				unmarkedNeighbours.Add(neighbour);
			}
			else if (neighbour.getState() == CellState.flagged) {
				countMarkedNeighbours++;
			}
		}
		if (countMarkedNeighbours == cell.getCountNeighbours()) {
			foreach (var unmarkedCell in unmarkedNeighbours) {
				revealCell(unmarkedCell);
			}
		}
	}

	public void revealCell(MineSweeperCell cell) {
		int x = cell.getX();
		int y = cell.getY();
		int z = cell.getZ();
		if (!acted) {
			acted = true;
			setMines(x, y, z);
			context.startTimer();
		}

		if (cell.hasMine()) {
			deactivate();
			showMines(cell);
			context.setLost();
		}
		else {
			StartCoroutine("replaceCellWithRevealed", cells[getCellIndex(x, y, z)]);
		}
	}

	void showMines(MineSweeperCell clickedMine) {
		foreach (var mineIndex in mines) {
			MineSweeperCell cell = cells[mineIndex];
			if (cell.getState() != CellState.flagged) {
				cell.showMine(cell == clickedMine);
			}
		}
	}

	void setWin() {
		if (minesFound != minesCount) {
			return;
		}
		foreach (var cell in cells) {
			if (cell.getState() == CellState.initial || cell.getState() == CellState.potentialFlag) {
				return;
			}
		}

		deactivate();
		// Reveal the whole grid
		// if not lost, set won
		context.setWon();
	}

	IEnumerator replaceCellWithRevealed(MineSweeperCell cell) {
		List<MineSweeperCell> cellsToReveal = new List<MineSweeperCell>();
		cellsToReveal.Add(cell);
		while (cellsToReveal.Count > 0) {
			MineSweeperCell currentCell = cellsToReveal[0];
			cellsToReveal.RemoveAt(0);
			if (currentCell.getState() != CellState.initial) {
				continue;
			}
			currentCell.setState(CellState.revealed);
			int currX = currentCell.getX();
			int currY = currentCell.getY();
			int currZ = currentCell.getZ();
			int nbNeighbourMines = countNeighbourMines(currX, currY, currZ);
			if (nbNeighbourMines > 0) {
				showNeighboursCount(currentCell, nbNeighbourMines);
			}
			else {
				foreach (GridCoordinate coords in neighbourCoordinates(currX, currY, currZ)) {
					MineSweeperCell neighbourCell = cells[getCellIndex(coords.x, coords.y, coords.z)];
					if (coords.x == currX && coords.y == currY && coords.z == currZ ||
						neighbourCell.getState() != CellState.initial
					) {
						continue;
					}
					cellsToReveal.Add(neighbourCell);
				}
			}
			currentCell.gameObject.SetActive(false);
		}
		setWin();
		yield return null;
	}

	private void showNeighboursCount(MineSweeperCell cell, int nbNeighbourMines) {
		GameObject obj = Instantiate(revealedCellPrefab, cell.gameObject.transform.position, Quaternion.identity);
		MineSweeperRevealedCell revealedCell = obj.GetComponent<MineSweeperRevealedCell>();
		revealedCell.setOriginalCell(cell);
		revealedCell.setValue(nbNeighbourMines);
		obj.transform.parent = gameObject.transform;
	}

	public void addFoundMine() {
		minesFound++;
		setWin();
	}

	public void removeFoundMine() {
		minesFound--;
	}

	public int getCountUnfoundMines() {
		return minesCount - minesFound;
	}

	public bool hasUnfoundMines() {
		return minesCount > minesFound;
	}

	private IEnumerable<GridCoordinate> neighbourCoordinates(int x, int y, int z) {
		for (int k = z - 1; k <= z + 1; k++) {
			for (int j = y - 1; j <= y + 1; j++) {
				for (int i = x - 1; i <= x + 1; i++) {
					if (i >= 0 && j >= 0 && k >= 0 && i < width && j < height && k < depth) {
						yield return new GridCoordinate(i, j, k);
					}
				}
			}
		}
	}

	private int countNeighbourMines(int x, int y, int z) {
		int neighbours = 0;
		foreach (GridCoordinate coords in neighbourCoordinates(x, y, z)) {
			if (coords.x == x && coords.y == y && coords.z == z) {
				continue;
			}
			else if (cells[getCellIndex(coords.x, coords.y, coords.z)].hasMine()) {
				neighbours++;
			}
		}
		return neighbours;
	}

	private void setMines(int xToExclude, int yToExclude, int zToExclude) {
		int size = getSize();
		var candidateIndices = Enumerable.Range(0, size).ToList();
		System.Random rnd = new System.Random();

		foreach (GridCoordinate coords in neighbourCoordinates(xToExclude, yToExclude, zToExclude)) {
			candidateIndices.Remove(getCellIndex(coords.x, coords.y, coords.z));
		}

		for (int m = 0; m < minesCount && candidateIndices.Count > 0; m++) {
			int randomIndex = rnd.Next(candidateIndices.Count);

			int cellIndex = candidateIndices[randomIndex];
			cells[cellIndex].setMine();
			mines.Add(cellIndex);
			candidateIndices.RemoveAt(randomIndex);
		}
	}
}

