using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperContext : MonoBehaviour
{
	[SerializeField] TextMesh timerMesh;

	float timeSpent = 0;
	bool running = false;

    void Start() {
		displayTime(timeSpent);
    }

	public void startTimer() {
		running = true;
	}

	void Update() {
		if (running) {
			timeSpent += Time.deltaTime;
			displayTime(timeSpent);
		}
    }

	void displayTime(float t) {
		int tInMs = (int)(t * 1000.0);
		int minutes = tInMs / 60000;
		int rem = tInMs % 60000;
		int seconds = rem / 1000;
		int ms = rem % 1000;
		timerMesh.text = string.Format(
			"{0}:{1}.{2}",
			minutes, seconds.ToString("00"), ms.ToString("000")
		);
	}

	public void setWon() {
		running = false;
		Debug.Log("Won");
		// save time
		// display menu with win option
	}

	public void setLost() {
		running = false;
		Debug.Log("Lost");
		// display menu with lost option
	}
}
