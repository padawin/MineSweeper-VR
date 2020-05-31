using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRotateGrid : MonoBehaviour
{
	[SerializeField] MineSweeperGrid grid;
	[SerializeField] float rotationSpeed = 80.0f;

    void Update() {
		Vector2 stickState = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
		grid.gameObject.transform.Rotate(new Vector3(
			stickState.y * rotationSpeed * Time.deltaTime,
			stickState.x * rotationSpeed * Time.deltaTime,
			0.0f
		));
    }
}
