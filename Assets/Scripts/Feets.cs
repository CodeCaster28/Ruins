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

	//== Public ========================

	public bool ForceFlight {
		get { return forceFlight; }
		set { forceFlight = value; }
	}

	public bool OnGround() {
		return (numColliders == 0 || forceFlight) ? false : true;
	}
	
	public Collider GetLastCollider() {
		return (colliders.Count > 0) ? colliders[colliders.Count - 1] : null;
	}
}
