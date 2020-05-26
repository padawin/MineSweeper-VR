using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperCell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other) {
		Debug.Log(other.name);
    }
}
