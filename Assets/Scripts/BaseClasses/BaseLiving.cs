using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLiving : MonoBehaviour {

	public float recoveryTime = 0.6f;
	public int health = 5;

	private bool usesNormalMesh;
	private SkinnedMeshRenderer model;
	private MeshRenderer modelAlt;

	protected Rigidbody rBody;
	protected bool invictible;

	[SerializeField]
	private bool damageKnockOut = true;

	protected virtual void Start() {
		invictible = false;
		usesNormalMesh = false;
		model = GetComponentInChildren<SkinnedMeshRenderer>();

		if (model == null) {
			usesNormalMesh = true;
			modelAlt = GetComponentInChildren<MeshRenderer>();
		}
		rBody = GetComponent<Rigidbody>();
	}

	public virtual void Damage(Vector3 source, float force, int damage) {
		if (!invictible) {
			health -= damage;
			if (health <= 0) {
				Kill();
			}
			else {
				StartCoroutine(Flicker());
				if (damageKnockOut)
					StartCoroutine(KnockOut(source, force));
			}
		}
	}

	protected virtual void Kill() {
		Destroy(gameObject);
	}

	IEnumerator KnockOut(Vector3 source, float force) {

		rBody.isKinematic = false;
		Vector3 direction = source - transform.position;
		direction = -direction.normalized;
		rBody.AddForce(direction.x * force, 0, direction.z * force, ForceMode.Impulse);
		yield return new WaitForSeconds(0.4f);

		rBody.velocity = Vector3.zero;
		rBody.isKinematic = true;
		yield return null;
	}

	IEnumerator Flicker() {

		invictible = true;
		if (usesNormalMesh) modelAlt.material.color = new Color(1f, 0.35f, 0.35f, 1);
		else model.material.color = new Color(1f, 0.35f, 0.35f, 1);
		yield return new WaitForSeconds(recoveryTime);

		if (usesNormalMesh) modelAlt.material.color = new Color(1, 1, 1, 1);
		else model.material.color = new Color(1, 1, 1, 1);
		invictible = false;
		yield return null;
	}

}
