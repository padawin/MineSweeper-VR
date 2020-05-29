using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState { initial, flagged, potentialFlag, revealed };

public class MineSweeperCell : MonoBehaviour
{
	private int x;
	private int y;
	private int z;

	private bool mine = false;
	private CellState state = CellState.initial;

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

	public CellState getState() {
		return state;
	}

	public void setState(CellState newState) {
		state = newState;
	}
}
