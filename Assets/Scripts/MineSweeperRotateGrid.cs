using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRotateGrid : MonoBehaviour
{
	[SerializeField] float rotationSpeed = 80.0f;

	MineSweeperGrid grid;

	private void Start() {
		grid = GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>();
	}

    void Update() {
		Vector2 stickState = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
		grid.rotate(
			stickState.y * rotationSpeed * Time.deltaTime,
			stickState.x * rotationSpeed * Time.deltaTime
		);
    }
}
