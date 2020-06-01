using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState { initial, flagged, potentialFlag, revealed };

public class MineSweeperCell : MonoBehaviour {
	private int x;
	private int y;
	private int z;

	private bool mine = false;
	private CellState state = CellState.initial;
	private MeshRenderer meshRenderer;

	[SerializeField] Material defaultMaterial;
	[SerializeField] Material flaggedMaterial;
	[SerializeField] Material potentiallyFlaggedMaterial;

	private void Start() {
		meshRenderer = GetComponent<MeshRenderer>();
	}

	public void setCoordinates(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public int getX() {
		return x;
	}

	public int getY() {
		return y;
	}

	public int getZ() {
		return z;
	}

	public bool hasMine() {
		return mine;
	}

	public void setMine() {
		mine = true;
	}

	public void mark() {
		if (state == CellState.initial) {
			state = CellState.flagged;
			meshRenderer.material = flaggedMaterial;
		}
		else if (state == CellState.flagged) {
			state = CellState.potentialFlag;
			meshRenderer.material = potentiallyFlaggedMaterial;
		}
		else if (state == CellState.potentialFlag) {
			state = CellState.initial;
			meshRenderer.material = defaultMaterial;
		}
	}

	public CellState getState() {
		return state;
	}

	public void setState(CellState newState) {
		state = newState;
	}

	public void setHovered(bool hovered) {
		if (hovered) {
			meshRenderer.material.EnableKeyword("_EMISSION");
			meshRenderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
		}
		else {
			meshRenderer.material.DisableKeyword("_EMISSION");
			meshRenderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
		}
	}
}
