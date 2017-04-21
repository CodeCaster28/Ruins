using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feets : MonoBehaviour {

	private int numColliders = 0;

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Untagged") {
			numColliders++;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Untagged") {
			numColliders--;
		}
	}

	public bool OnGround() {
		if (numColliders == 0)
			return false;
		else return true;
	}

}
