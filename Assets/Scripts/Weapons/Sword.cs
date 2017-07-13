using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	public Transform start;
	public Transform end;

	private BaseLiving target;
	private float knockoutForce = 10;
	private int damage = 1;
	private CharacterCtrl charCtrl;

	private void Start() {
		charCtrl = GetComponentInParent<CharacterCtrl>();
	}

	private void Update() {
		RaycastHit hit;
		if (Physics.Raycast(start.position, end.position - start.position, out hit, Vector3.Distance(start.position, end.position))) {
			target = hit.collider.GetComponent<BaseLiving>();
			if (target != null && charCtrl.IsAttacking) {
				target.Damage(charCtrl.rBody.transform.position, knockoutForce, damage);
			}
		}
	}
}