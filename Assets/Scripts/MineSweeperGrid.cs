using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperGrid : MonoBehaviour
{
	[SerializeField] GameObject cellPrefab;
	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] int depth;
	[SerializeField] float cellSpacing;

	private List<GameObject> cells = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
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
					cells.Add(cell);
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

