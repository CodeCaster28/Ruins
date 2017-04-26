using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feets : MonoBehaviour {

	//== Fields =========================

	private int numColliders = 0;
	private bool forceFlight;
	private List<Collider> colliders;

	//== Mono ===========================

	private void Start() {
		forceFlight = false;
		colliders = new List<Collider>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Untagged") {
			numColliders++;
			if (other.GetComponent<Rigidbody>())
				colliders.Add(other);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Untagged") {
			numColliders--;
			colliders.Remove(other);
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

	public Collider GetLastCollider() {

		if (colliders.Count > 0) {
			return colliders[colliders.Count - 1];
		}
		else return null;
	}
}
