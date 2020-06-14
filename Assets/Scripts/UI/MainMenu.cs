using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	[SerializeField] GameObject gridPrefab;
	[SerializeField] GameObject gameRig;

	public void clickMenuButton() {
		Destroy(gameObject);
		MineSweeperPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<MineSweeperPlayer>();
		player.enableGameHands();
		GameObject.FindGameObjectWithTag("grid").GetComponent<MineSweeperGrid>().init();
	}
}
