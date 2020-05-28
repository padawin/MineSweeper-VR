using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealedCell : MonoBehaviour
{
	Camera m_camera;

	private void Start() {
		m_camera = FindObjectOfType<Camera>();
	}

	void Update() {
		transform.LookAt(m_camera.transform);
    }
}
