using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperDestroyCell : MonoBehaviour
{
	void OnTriggerEnter(Collider other) {
		Debug.Log(other.name);
		Destroy(other.gameObject);
	}
}
