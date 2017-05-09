using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour {

	private bool canDamage;
	private MeshRenderer model;

	private void Start() {
		canDamage = true;
		model = GetComponent<MeshRenderer>();
	}

	public void Damage() {
		if (canDamage)
			StartCoroutine(Highlight());
	}

	IEnumerator Highlight() {
		canDamage = true;
		model.material.color = new Color(1, 0, 0, 1);
		yield return new WaitForSeconds(0.4f);
		model.material.color = new Color(1, 1, 1, 1);
		canDamage = true;
		yield return null;
	}

}