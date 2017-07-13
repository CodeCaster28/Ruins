using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octto : BaseLiving {

	private Animator anim;
	private GameObject ragdoll;
	private void Awake() {
		anim = GetComponent<Animator>();
		ragdoll = Resources.Load("Rags/OcctoRag", typeof(GameObject)) as GameObject;
	}

	protected override void Start() {
		base.Start();
	}

	public override void Damage(Vector3 source, float force, int damage) {
		if (!invictible) {
			anim.SetTriggerRequired("KnockOut", "Idle");
			Debug.Log("POS: " + transform.position);
			Effects.Master.PlayEffect("SwordHit", transform.position);
		}
		base.Damage(source, force, damage);

	}

	protected override void Kill() {
		GameObject rag;
		rag = Instantiate(ragdoll, transform.position, transform.rotation);
		rag.SetActive(true);
		base.Kill();
	}
}