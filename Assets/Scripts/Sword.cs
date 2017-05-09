using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	private bool canDamage;
	private MeshRenderer target;
	private CharacterCtrl charCtrl;

	private void Start() {
		canDamage = true;
		charCtrl = GetComponentInParent<CharacterCtrl>();
	}

	// TODO: create detector script so each one of detector can be damaged
	// TODO: communicate with Player.cs whenever player is attacking or not (or disable/enable sword trigger during attack)
	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Damageable" && canDamage && charCtrl.IsAttacking) {
			target = other.GetComponent<MeshRenderer>();
			StartCoroutine(Highlight(target));
		}
	}

	IEnumerator Highlight(MeshRenderer other) {
		canDamage = false;
		other.material.color = new Color(1, 0, 0, 1);
		yield return new WaitForSeconds(0.4f);
		other.material.color = new Color(1, 1, 1, 1);
		canDamage = true;
		yield return null;
	}
}
