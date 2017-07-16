using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

	public Transform start;
	public Transform end;
	public Transform zeroZone;

	private BaseLiving target;
	private float knockoutForce = 10;
	private int damage = 1;
	private CharacterCtrl charCtrl;

	private void Start() {
		charCtrl = GetComponentInParent<CharacterCtrl>();
	}

	private void Update() {
		RaycastHit hit;

		if (charCtrl.IsAttacking) {
			if (Physics.Raycast(start.position, end.position - start.position, out hit, Vector3.Distance(start.position, end.position))) {
				target = hit.collider.GetComponent<BaseLiving>();
				if (target != null) {
					target.Damage(charCtrl.rBody.transform.position, knockoutForce, damage);
				}
			}
			else {
				foreach (Collider col in Physics.OverlapCapsule(zeroZone.position - new Vector3(0, -0.4f, 0), zeroZone.position - new Vector3(0, 0.4f, 0), 0.25f)) {
					target = col.GetComponent<BaseLiving>();
					if (target != null) {
						target.Damage(charCtrl.rBody.transform.position, knockoutForce, damage);
					}
				}
			}
		}
	}

	/*private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(zeroZone.position - new Vector3(0, -0.4f, 0), 0.25f);
		Gizmos.DrawWireSphere(zeroZone.position - new Vector3(0, 0.4f, 0), 0.25f);
	}*/
}