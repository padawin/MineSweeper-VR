using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperRevealedCell : MonoBehaviour
{
	Camera m_camera;
	[SerializeField] TextMesh text;

	private void Start() {
		m_camera = FindObjectOfType<Camera>();
	}

	public void setValue(int val) {
		text.text = val.ToString();
	}

	void Update() {
		transform.LookAt(m_camera.transform);
    }
}
