using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineSweeperGrid : MonoBehaviour {
	[SerializeField] GameObject cellPrefab;
	[SerializeField] GameObject revealedCellPrefab;
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] int depth;
	[SerializeField] float cellSpacing;
	[SerializeField] int minesCount = 0;

	List<List<List<bool>>> mines = new List<List<List<bool>>>();
	List<List<List<GameObject>>> cells = new List<List<List<GameObject>>>();
	int minesFound = 0;

	// Set to true when the player destroys or marks a mine for the first time
	bool acted = false;

	// Start is called before the first frame update
	void Start() {
		initMinesList();
		instanciateCells();
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

	void initMinesList() {
		for (int i = 0; i < width; i++) {
			mines.Add(new List<List<bool>>());
			for (int j = 0; j < height; j++) {
				mines[i].Add(new List<bool>());
				for (int k = 0; k < depth; k++) {
					mines[i][j].Add(false);
				}
			}
		}
	}

	void instanciateCells() {
		float cellWidth = cellPrefab.transform.localScale.x;
		float cellHeight = cellPrefab.transform.localScale.y;
		float cellDepth = cellPrefab.transform.localScale.z;
		float startX = transform.position.x - (cellWidth * width / 2);
		float startY = transform.position.y - (cellHeight * height / 2);
		float startZ = transform.position.z - (cellDepth * depth);
		for (int i = 0; i < width; i++) {
			cells.Add(new List<List<GameObject>>());
			for (int j = 0; j < height; j++) {
				cells[i].Add(new List<GameObject>());
				for (int k = 0; k < depth; k++) {
					GameObject cell = Instantiate(cellPrefab, transform.position, Quaternion.identity);
					cell.transform.position = new Vector3(
						startX + i * (cellWidth + cellSpacing),
						startY + j * (cellHeight + cellSpacing),
						startZ + k * (cellDepth + cellSpacing)
					);
					cell.transform.parent = gameObject.transform;
					cell.GetComponent<MineSweeperCell>().setCoordinates(i, j, k);
					cells[i][j].Add(cell);
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
		}

		Debug.Log(mines[x][y][z]);
		if (mines[x][y][z]) {
			// Lost
			//Destroy(gameObject);
		}
		else {
			replaceCellWithRevealed(cell);
		}
		// TODO show number neighbours
		// If 0 mines, expand reveal
	}

	private int countNeighbourMines(int x, int y, int z) {
		int neighbours = 0;
		for (int k = z - 1; k <= z + 1; k++) {
			for (int j = y - 1; j <= y + 1; j++) {
				for (int i = x - 1; i <= x + 1; i++) {
					if (i < 0 || j < 0 || k < 0 ||
						i >= width || j >= height || k >= depth ||
						i == x && j == y && k == z
					) {
						continue;
					}
					int index = getCellIndex(i, j, k);
					if (mines[i][j][k]) {
						neighbours++;
					}
				}
			}
		}
		return neighbours;
	}

	private void replaceCellWithRevealed(MineSweeperCell cell, int limit = 20) {
		Destroy(cell.gameObject);
		int nbNeighbourMines = countNeighbourMines(cell.getX(), cell.getY(), cell.getZ());
		if (nbNeighbourMines > 0) {
			GameObject revealedCell = Instantiate(revealedCellPrefab, cell.gameObject.transform.position, Quaternion.identity);
			revealedCell.GetComponent<MineSweeperRevealedCell>().setValue(nbNeighbourMines);
			revealedCell.transform.parent = gameObject.transform;
		}
		else {
			// TODO handle recursive revealing
		}
	}

	private void setMines(int xToExclude, int yToExclude, int zToExclude) {
		int size = getSize();
		var candidateIndices = Enumerable.Range(0, size).ToList();
		System.Random rnd = new System.Random();

		for (int k = zToExclude - 1; k <= zToExclude + 1; k++) {
			for (int j = yToExclude - 1; j <= yToExclude + 1; j++) {
				for (int i = xToExclude - 1; i <= xToExclude + 1; i++) {
					if (i < 0 || j < 0 || k < 0 ||
						i >= width || j >= height || k >= depth
					) {
						continue;
					}
					int cellToRemove = getCellIndex(i, j, k);
					candidateIndices.Remove(cellToRemove);
				}
			}
		}

		for (int m = 0; m < minesCount && candidateIndices.Count > 0; m++) {
			int randomIndex = rnd.Next(candidateIndices.Count);

			int cellIndex = candidateIndices[randomIndex];
			// index to coordinates
			int z = cellIndex / (width * height);
			int yx = cellIndex % (width * height);
			int y = yx / width;
			int x = yx % width;
			mines[x][y][z] = true;
			candidateIndices.RemoveAt(randomIndex);
		}
	}
}

