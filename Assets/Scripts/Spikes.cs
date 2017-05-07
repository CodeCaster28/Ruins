using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

	public int damage = 1;

	private void OnTriggerStay(Collider other) {
		if(other.tag == "Player") {
			other.transform.parent.GetComponent<Player>().Damage(damage);
		}
	}
}