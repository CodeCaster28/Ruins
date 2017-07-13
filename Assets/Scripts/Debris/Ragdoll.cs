using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {

	public float lifeTime = 10.0f;
	private Rigidbody rBody;
	private SkinnedMeshRenderer mesh;

	void Start () {
		rBody = GetComponentInChildren<Rigidbody>();
		mesh = GetComponentInChildren<SkinnedMeshRenderer>();
		Vector3 dir = new Vector3(Random.Range(-2, 2), Random.Range(3, 4), Random.Range(-2, 2));
		rBody.AddForce(dir * 210);
		rBody.AddTorque(dir*60);
		StartCoroutine(Remove(lifeTime));
		
	}

	IEnumerator Remove(float lifeTime) {

		yield return new WaitForSeconds(lifeTime);
		float decreaseVal = 1;
		float colorVal = 0;
		rBody.isKinematic = true;
		mesh.material.SetColor("_EmissionColor", new Color(0,0,0));
		for (int i = 0; i < 30; i++) {
			yield return new WaitForSeconds(0.02f);
			gameObject.transform.localScale *= decreaseVal;
			mesh.material.SetColor("_EmissionColor", new Color(colorVal, colorVal, colorVal));
			colorVal += 0.03f;
			decreaseVal -= 0.005f;
		}
		Destroy(gameObject);
	}
}
