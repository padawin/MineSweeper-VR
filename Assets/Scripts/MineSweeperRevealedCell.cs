using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MineSweeperRevealedCell : MonoBehaviour
{
	Camera m_camera;
	[SerializeField] TextMeshPro text;
	[SerializeField] MeshRenderer textRenderer;
	[SerializeField] Material defaultMaterial;
	[SerializeField] Material hoveredMaterial;
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
			text.color = colors[val];
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

	public void setHovered(bool hovered) {
		if (hovered) {
			textRenderer.material = hoveredMaterial;
		}
		else {
			textRenderer.material = defaultMaterial;
		}
	}
}
