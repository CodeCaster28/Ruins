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

	public void Damage() {
		if (canDamage) {
			StartCoroutine(TakeDamage());
		}
	}

	IEnumerator TakeDamage() {

		rBody.isKinematic = false;
		canDamage = true;
		model.material.color = new Color(1f, 0.35f, 0.35f, 1);
		rBody.AddForce(1, 0, 0, ForceMode.Impulse);

		yield return new WaitForSeconds(0.4f);

		canDamage = true;
		model.material.color = new Color(1, 1, 1, 1);
		rBody.isKinematic = true;

		yield return null;
	}

}