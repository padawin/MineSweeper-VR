using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperTranslateGrid : MonoBehaviour
{
	[SerializeField] float translationSpeed = 1.0f;
	[SerializeField] float minDistanceFromPlayer = 0.5f;

	GameObject grid;
	GameObject player;

	private void Start() {
		grid = GameObject.FindGameObjectWithTag("grid");
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		Vector2 sideTranslate = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
		Vector3 towardsPlayer = player.transform.position - grid.transform.position;
		Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
		Vector3 side = Vector3.Cross(towardsPlayer, up);
		float speedTowardsPlayer;
		if (OVRInput.Get(OVRInput.RawButton.Y)) {
			speedTowardsPlayer = translationSpeed; // we push the grid away
		}
		else if (OVRInput.Get(OVRInput.RawButton.X) && towardsPlayer.magnitude > minDistanceFromPlayer) {
			speedTowardsPlayer = -translationSpeed; // we pull the grid closer
		}
		else {
			speedTowardsPlayer = 0; // we don't move the grid
		}
		towardsPlayer.Normalize();
		towardsPlayer = towardsPlayer * -speedTowardsPlayer;
		grid.transform.position = grid.transform.position
			+ towardsPlayer * Time.deltaTime
			+ up * sideTranslate.y * translationSpeed * Time.deltaTime
			+ side * sideTranslate.x * translationSpeed * Time.deltaTime;
	}
}
