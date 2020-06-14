using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperPlayer : MonoBehaviour
{
	[SerializeField] GameObject menuHands;
	[SerializeField] GameObject gameHands;

    void Start()
    {
		enableMenuHands();        
    }

	public void enableMenuHands() {
		menuHands.SetActive(true);
		gameHands.SetActive(false);
	}

	public void enableGameHands() {
		menuHands.SetActive(false);
		gameHands.SetActive(true);
	}
}
