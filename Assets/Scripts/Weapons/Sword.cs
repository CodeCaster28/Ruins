using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	public Transform start;
	public Transform end;

	private CharacterCtrl charCtrl;
	private void Start() {
		charCtrl = GetComponentInParent<CharacterCtrl>();
	}

	private void Update() {
		RaycastHit hit;
		if (Physics.Raycast(start.position, end.position - start.position, out hit, Vector3.Distance(start.position, end.position))) {
			if (hit.collider.gameObject.tag == "Damageable" && charCtrl.IsAttacking) {
				hit.collider.gameObject.GetComponent<AttackDetector>().Damage();
			}
		}
	}
}