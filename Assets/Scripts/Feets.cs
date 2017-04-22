using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feets : MonoBehaviour {

	//== Fields =========================

	private int numColliders = 0;
	private bool forceFlight;

	//== Mono ===========================

	private void Start() {
		forceFlight = false;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Untagged") {
			numColliders++;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Untagged") {
			numColliders--;
		}
	}

	//== Publics ========================

	public bool ForceFlight {
		get { return forceFlight; }
		set { forceFlight = value; }
	}

	public bool OnGround() {
		if (numColliders == 0 || forceFlight == true)
			return false;
		else return true;
	}
}
