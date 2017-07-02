using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	public Transform start;
	public Transform end;

	private float knockoutForce = 4;
	private CharacterCtrl charCtrl;
	private void Start() {
		charCtrl = GetComponentInParent<CharacterCtrl>();
	}

	private void Update() {
		RaycastHit hit;
		if (Physics.Raycast(start.position, end.position - start.position, out hit, Vector3.Distance(start.position, end.position))) {
			if (hit.collider.gameObject.tag == "Damageable" && charCtrl.IsAttacking) {
				//Debug.Log("|" + charCtrl.rBody.gameObject.name + "|" + charCtrl.rBody.transform.position + "|");
				hit.collider.gameObject.GetComponent<AttackDetector>().Damage(charCtrl.rBody.transform.position, knockoutForce);
			}
		}
	}
}