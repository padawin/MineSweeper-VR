using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperCell : MonoBehaviour
{
	private int x;
	private int y;

	public void setCoordinates(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int getX() {
		return x;
	}

	public int getY() {
		return y;
	}
}
