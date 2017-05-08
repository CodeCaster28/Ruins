using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

	//== Fields =========================

	public int damage = 1;

	//== Private =========================

	private void OnTriggerStay(Collider other) {
		if(other.tag == "Player") {
			other.transform.parent.GetComponent<Player>().Damage(damage);
		}
	}
}