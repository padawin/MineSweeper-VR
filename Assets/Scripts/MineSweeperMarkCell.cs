using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperMarkCell : MonoBehaviour
{
	void OnTriggerEnter(Collider other) {
		Debug.Log(other.name);
	}
}
