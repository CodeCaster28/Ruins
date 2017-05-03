using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCtrl : MonoBehaviour {

	private List<Vector3> normals;
	private Rigidbody rBody;
	private bool blocked;
	
	public bool Blocked {
		get { return blocked;}
	}

	private void Start() {
		normals = new List<Vector3>();
		blocked = false;
		rBody = null;
	}

	private void Update() {
		foreach (Vector3 normal in normals) {
			// Debug.DrawLine(transform.position + normal, transform.position, Color.red);
			// Debug.Log(normal);
		}
	}

	private void FixedUpdate() {
		normals.Clear();
		blocked = false;
		rBody = null;
	}

	// Get collection of rigidbody colliders
	private void OnCollisionStay(Collision other) {
		rBody = other.gameObject.GetComponent<Rigidbody>();
		if (rBody != null) {
			if (rBody.isKinematic == true) {
				foreach (ContactPoint contact in other.contacts) {
					normals.Add(contact.normal);
					CheckForOpposites(contact.normal);
				}
			}
		}
	}

	// Check if there is any opposite normals from kinematic rigidbodies
	private void CheckForOpposites(Vector3 x) {
		foreach (Vector3 normal in normals) {
			if (x == normal * -1) {
				blocked = true;
			}
		}
	}
}
