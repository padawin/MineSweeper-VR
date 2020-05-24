using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperGrid2D : MonoBehaviour
{
	[SerializeField] GameObject cellPrefab;
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] float cellSpacing;

	private List<GameObject> cells = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
		float cellWidth = cellPrefab.transform.localScale.x;
		float cellHeight = cellPrefab.transform.localScale.y;
		float startX = transform.position.x - (cellWidth * width / 2);
		float startY = transform.position.y - (cellHeight * height / 2);
        for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				GameObject cell = Instantiate(cellPrefab, transform.position, Quaternion.identity);
				cell.transform.position = new Vector3(
					startX + i * (cellWidth + cellSpacing),
					startY + j * (cellHeight + cellSpacing),
					transform.position.z
				);
				cells.Add(cell);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

