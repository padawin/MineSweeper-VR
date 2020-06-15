using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealedCell : MonoBehaviour
{
	Camera m_camera;
	[SerializeField] TextMesh text;
	[SerializeField] MeshRenderer textRenderer;
	[SerializeField] Color[] colors;
	MineSweeperCell originalCell;
	int neighboursCount = 0;

	private void Start() {
		m_camera = FindObjectOfType<Camera>();
	}

	public void setValue(int val) {
		neighboursCount = val;
		text.text = val.ToString();
		if (val < colors.Length) {
			textRenderer.materials[0].SetColor("_Color", colors[val]);
		}
	}

	public int getCountNeighbours() {
		return neighboursCount;
	}

	public void setOriginalCell(MineSweeperCell cell) {
		originalCell = cell;
	}

	public MineSweeperCell getOriginalCell() {
		return originalCell;
	}

	void Update() {
		transform.LookAt(m_camera.transform);
    }
}
