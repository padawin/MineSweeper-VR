using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperTranslateGrid : MonoBehaviour
{
	[SerializeField] MineSweeperGrid grid;
	// Negative speed to reverse the rotation
	[SerializeField] float translationSpeed = 5.0f;

    void Update() {
		Vector2 sideTranslate = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
		float depthTranslate = 0.0f;
		if (OVRInput.Get(OVRInput.RawButton.Y)) {
			depthTranslate = translationSpeed;
		}
		else if (OVRInput.Get(OVRInput.RawButton.X)) {
			depthTranslate = -translationSpeed;
		}
		grid.gameObject.transform.Translate(new Vector3(
			sideTranslate.x * translationSpeed * Time.deltaTime,
			sideTranslate.y * translationSpeed * Time.deltaTime,
			depthTranslate * Time.deltaTime
		));
    }
}
