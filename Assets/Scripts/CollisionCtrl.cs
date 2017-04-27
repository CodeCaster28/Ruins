using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCtrl : MonoBehaviour {

	private List<Vector3> normals;
	private Rigidbody rBody;

	private void Start() {
		normals = new List<Vector3>();
		rBody = null;
	}

	private void Update() {
		foreach (Vector3 normal in normals) {
			Debug.DrawLine(transform.position + normal, transform.position, Color.red);
		}
	}

	void FixedUpdate() {
		normals.Clear();
	}

	// Get collection of rigidbody colliders
	private void OnCollisionStay(Collision other) {
		rBody = other.gameObject.GetComponent<Rigidbody>();
		if (rBody != null) {
			if (rBody.isKinematic == true) {
				foreach (ContactPoint contact in other.contacts) {
					normals.Add(contact.normal);
				}
			}
		}
	}
}
