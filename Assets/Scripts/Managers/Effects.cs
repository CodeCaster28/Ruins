using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : GenericSingletonClass<Effects> {

	ParticleSystem[] particles;

	void Start () {
		particles = GetComponentsInChildren<ParticleSystem>();
	}

	public void PlayEffect(string systemName, Vector3 pos) {

		for (int i=0; i< particles.Length; i++) {
			if (particles[i].name == systemName) {
				GameObject k;
				k = Instantiate(particles[i].gameObject, pos + Vector3.up, particles[i].transform.rotation);
				k.GetComponent<ParticleSystem>().Play();
				Destroy(k, k.GetComponent<ParticleSystem>().main.duration);
				return;
			}
		}
		Debug.Log("Particle system: \"" + name + "\" not found!");
	}
}
