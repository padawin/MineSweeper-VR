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

	List<bool> mines = new List<bool>();
	int minesFound = 0;

	// Set to true when the player destroys or marks a mine for the first time
	bool acted = false;

	// Start is called before the first frame update
	void Start() {
		initMinesList();
		instanciateCells();
	}

	void initMinesList() {
		int gridSize = width * height;
        for (int i = 0; i < gridSize; i++) {
			mines.Add(false);
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
			for (int j = 0; j < height; j++) {
				for (int k = 0; k < depth; k++) {
					GameObject cell = Instantiate(cellPrefab, transform.position, Quaternion.identity);
					cell.transform.position = new Vector3(
						startX + i * (cellWidth + cellSpacing),
						startY + j * (cellHeight + cellSpacing),
						startZ + k * (cellDepth + cellSpacing)
					);
					cell.transform.parent = gameObject.transform;
					cell.GetComponent<MineSweeperCell>().setCoordinates(i, j);
				}
			}
		}
	}

	public int getWidth() {
		return width;
	}

	public int getHeight() {
		return height;
	}

	public void revealCell(MineSweeperCell cell) {
		int i = cell.getY() * width + cell.getX();
		if (!acted) {
			acted = true;
			setMines(i);
		}

		if (mines[i]) {
			// Lost
			Destroy(gameObject);
		}
		else {
			GameObject revealedCell = Instantiate(revealedCellPrefab, cell.gameObject.transform.position, Quaternion.identity);
			revealedCell.transform.parent = gameObject.transform;
			Destroy(cell.gameObject);
		}
		// TODO show number neighbours
		// If 0 mines, expand reveal
	}

	private void setMines(int cellToExclude) {
		int size = width * height;
		var candidateIndices = Enumerable.Range(0, size).ToList();
		System.Random rnd = new System.Random();

		candidateIndices.RemoveAt(cellToExclude);
		int numberCandidates = size - 1;
		for (int m = 0; m < minesCount; m++) {
			int randomIndex = rnd.Next(candidateIndices.Count);
			mines[candidateIndices[randomIndex]] = true;
			candidateIndices.RemoveAt(randomIndex);
		}
	}
}

