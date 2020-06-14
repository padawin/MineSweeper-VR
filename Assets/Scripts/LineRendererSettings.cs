using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererSettings : MonoBehaviour {
	[SerializeField] LineRenderer lineRenderer;

	//Settings for the LineRenderer are stored as a Vector3 array of points. Set up a V3 array to //initialize in Start. 
	Vector3[] points;

	bool hits = false;
	bool menuItemClicked = false;

	public LayerMask layerMask;

	GameObject selectedMenuItem;
	MineSweeperControl control;

	void Start() {
		points = new Vector3[2];

		//set the start point of the linerenderer to the position of the gameObject. 
		points[0] = Vector3.zero;

		//set the end point 20 units away from the GO on the Z axis (pointing forward)
		points[1] = transform.position + new Vector3(0, 0, 20);

		//finally set the positions array on the LineRenderer to our new values
		lineRenderer.SetPositions(points);
		lineRenderer.enabled = true;
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

		control = GetComponent<MineSweeperControl>();
	}

	// Update is called once per frame
	void Update() {
		detectButtonCollision();
		clickButton();
	}

	void detectButtonCollision() {
		Ray ray;
		ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, layerMask)) {
			if (!hits) {
				hits = true;
				lineRenderer.startColor = Color.green;
				lineRenderer.endColor = Color.green;
				selectedMenuItem = hit.collider.gameObject;
				control.vibrate(VibrationType.Gentle);
			}
		}
		else if (hits) {
			hits = false;
			lineRenderer.startColor = Color.white;
			lineRenderer.endColor = Color.white;
			selectedMenuItem = null;
			control.vibrate(VibrationType.Gentle);
		}
	}

	void clickButton() {
		if (selectedMenuItem != null && !menuItemClicked && control.isButtonPressed()) {
			Button button = selectedMenuItem.GetComponent<Button>();
			button.onClick.Invoke();
			menuItemClicked = true;
		}
		else if (menuItemClicked && selectedMenuItem != null && !control.isButtonPressed()) {
			menuItemClicked = false;
		}
	}
}
