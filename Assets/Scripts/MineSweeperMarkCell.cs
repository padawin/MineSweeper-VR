using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperMarkCell : MonoBehaviour
{
	MineSweeperGrid grid;
	MineSweeperControl playerControl;

	private void Start() {
		playerControl = GetComponent<MineSweeperControl>();
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
	}

	private void Update() {
		playerControl.mark(OVRInput.Controller.LTouch, OVRInput.RawButton.LIndexTrigger);
	}
}
