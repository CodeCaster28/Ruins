using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour {

	private bool canDamage;
	private MeshRenderer model;
	private Rigidbody rBody;

	private void Start() {
		canDamage = true;
		model = GetComponent<MeshRenderer>();
		rBody = GetComponent<Rigidbody>();
	}

	public void Damage(Vector3 source, float force) {
		if (canDamage) {
			StartCoroutine(TakeDamage(source, force));
		}
	}

	IEnumerator TakeDamage(Vector3 source, float force) {

		rBody.isKinematic = false;
		canDamage = false;
		model.material.color = new Color(1f, 0.35f, 0.35f, 1);
		Vector3 direction = source - transform.position;
		// Debug.Log("Source: " + source + ", enemy: " + transform.position);
		direction = -direction.normalized;
		rBody.AddForce(direction.x * force, 0, direction.z * force, ForceMode.Impulse);

		yield return new WaitForSeconds(0.4f);

		rBody.velocity = Vector3.zero;
		model.material.color = new Color(1, 1, 1, 1);
		rBody.isKinematic = true;
		canDamage = true;
		yield return null;
	}

}