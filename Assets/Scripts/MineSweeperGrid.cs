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

	List<List<List<MineSweeperCell>>> cells = new List<List<List<MineSweeperCell>>>();
	MineSweeperContext context;
	int minesFound = 0;

	// Set to true when the player destroys or marks a mine for the first time
	bool acted = false;
	bool active = true;

	// Start is called before the first frame update
	void Start() {
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

	private void positionGrid() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		float cellDepth = cellPrefab.transform.localScale.z;
		float initialZ = player.transform.position.z
						 + initialDistanceFromPlayer
						 + (depth / 2) * (cellDepth + cellSpacing);
		transform.position = new Vector3(
			player.transform.position.x,
			player.transform.position.y,
			initialZ
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

	void instanciateCells() {
		float cellWidth = cellPrefab.transform.localScale.x;
		float cellHeight = cellPrefab.transform.localScale.y;
		float cellDepth = cellPrefab.transform.localScale.z;
		float startX = transform.position.x - (cellWidth * width / 2);
		float startY = transform.position.y - (cellHeight * height / 2);
		float startZ = transform.position.z - (cellDepth * depth / 2);
		for (int i = 0; i < width; i++) {
			cells.Add(new List<List<MineSweeperCell>>());
			for (int j = 0; j < height; j++) {
				cells[i].Add(new List<MineSweeperCell>());
				for (int k = 0; k < depth; k++) {
					GameObject cell = Instantiate(cellPrefab, transform.position, Quaternion.identity);
					cell.transform.position = new Vector3(
						startX + i * (cellWidth + cellSpacing),
						startY + j * (cellHeight + cellSpacing),
						startZ + k * (cellDepth + cellSpacing)
					);
					cell.transform.parent = gameObject.transform;
					MineSweeperCell cellComponent = cell.GetComponent<MineSweeperCell>();
					cellComponent.setCoordinates(i, j, k);
					cells[i][j].Add(cellComponent);
				}
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
			context.setLost();
		}
		else {
			replaceCellWithRevealed(x, y, z);
		}
	}

	public void addFoundMine() {
		minesFound++;
		if (minesFound == minesCount) {
			deactivate();
			// Reveal the whole grid
			// if not lost, set won
			Debug.Log("Won");
			context.setWon();
		}
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

	private IEnumerable<GridCoordinate> neighbourIndices(int x, int y, int z) {
		for (int k = z - 1; k <= z + 1; k++) {
			for (int j = y - 1; j <= y + 1; j++) {
				for (int i = x - 1; i <= x + 1; i++) {
					yield return new GridCoordinate(i, j, k);
				}
			}
		}
	}

	private int countNeighbourMines(int x, int y, int z) {
		int neighbours = 0;
		foreach (GridCoordinate coord in neighbourIndices(x, y, z)) {
			int i = coord.x;
			int j = coord.y;
			int k = coord.z;
			if (i < 0 || j < 0 || k < 0 ||
				i >= width || j >= height || k >= depth ||
				i == x && j == y && k == z
			) {
				continue;
			}
			if (cells[i][j][k].hasMine()) {
				neighbours++;
			}
		}
		return neighbours;
	}

	private void replaceCellWithRevealed(int x, int y, int z) {
		MineSweeperCell cell = cells[x][y][z];
		cell.setState(CellState.revealed);
		int nbNeighbourMines = countNeighbourMines(x, y, z);
		if (nbNeighbourMines > 0) {
			GameObject revealedCell = Instantiate(revealedCellPrefab, cell.gameObject.transform.position, Quaternion.identity);
			revealedCell.GetComponent<MineSweeperRevealedCell>().setValue(nbNeighbourMines);
			revealedCell.transform.parent = gameObject.transform;
		}
		else {
			List<MineSweeperCell> neighboursToReveal = new List<MineSweeperCell>();
			foreach (GridCoordinate coord in neighbourIndices(x, y, z)) {
				int i = coord.x;
				int j = coord.y;
				int k = coord.z;
				if (i < 0 || j < 0 || k < 0 ||
					i >= width || j >= height || k >= depth ||
					(i == x && j == y && k == z) ||
					cells[i][j][k].getState() != CellState.initial
				) {
					continue;
				}
				replaceCellWithRevealed(i, j, k);
			}
		}
		Destroy(cell.gameObject);
	}

	private void setMines(int xToExclude, int yToExclude, int zToExclude) {
		int size = getSize();
		var candidateIndices = Enumerable.Range(0, size).ToList();
		System.Random rnd = new System.Random();

		foreach (GridCoordinate coord in neighbourIndices(xToExclude, yToExclude, zToExclude)) {
			int i = coord.x;
			int j = coord.y;
			int k = coord.z;
			if (i < 0 || j < 0 || k < 0 ||
				i >= width || j >= height || k >= depth
			) {
				continue;
			}
			int cellToRemove = getCellIndex(i, j, k);
			candidateIndices.Remove(cellToRemove);
		}

		for (int m = 0; m < minesCount && candidateIndices.Count > 0; m++) {
			int randomIndex = rnd.Next(candidateIndices.Count);

			int cellIndex = candidateIndices[randomIndex];
			// index to coordinates
			int z = cellIndex / (width * height);
			int yx = cellIndex % (width * height);
			int y = yx / width;
			int x = yx % width;
			cells[x][y][z].setMine();
			candidateIndices.RemoveAt(randomIndex);
		}
	}
}

