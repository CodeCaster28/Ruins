using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPad : MonoBehaviour {
	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			other.transform.parent.GetComponent<Player>().ModifyHealth(100);
		}
	}
}
