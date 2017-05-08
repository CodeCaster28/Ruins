using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	//== Delegates ======================

	public delegate void DRefreshHearts();
	public static event DRefreshHearts RefreshHearts;

	//== Fields =========================

	public int maxHealth;

	private MeshRenderer playerModel;
	private ParticleSystem[] healParticles;
	private GameObject player;
	private Coroutine healEffect;
	private Color defaultColor;

	//== Mono ===========================

	private void Start() {
		PlayerData.health = maxHealth;
		PlayerData.maxHealth = maxHealth;
		PlayerData.isPlayerInvincible = false;
		player = GameObject.Find("Player");
		playerModel = player.transform.Find("Base/Model").gameObject.GetComponent<MeshRenderer>();
		healParticles = player.transform.Find("Base/HealParticle").GetComponentsInChildren<ParticleSystem>();
		defaultColor = playerModel.material.color;
		if (RefreshHearts != null) {
			RefreshHearts();
		}
	}

	//== Public ========================

	public void ModifyHealth(int x) {
			if (x < 0 && !PlayerData.isPlayerInvincible)
				Damage(x);
			else if (x > 0 && PlayerData.health < PlayerData.maxHealth)
				Heal(x);
	}

	//== Private ========================

	private void Damage(int x) {
		PlayerData.health = PlayerData.health.CalculateLimited(x, maxHealth);
		if (PlayerData.health == 0) {
			StartCoroutine(KillPlayer());
		}
		else {
			StartCoroutine(DamageCooldown());
		}
		if (RefreshHearts != null) {
			RefreshHearts();
		}
	}

	private void Heal(int x) {
		PlayerData.health = PlayerData.health.CalculateLimited(x, maxHealth);
		if (RefreshHearts != null) {
			RefreshHearts();
		}
		if (healEffect != null) StopCoroutine(healEffect);
		healEffect = StartCoroutine(HealEffect());
	}

	private void Death() {
		player.GetComponent<CharacterCtrl>().DisableInputs = true;
		player.GetComponent<CharacterCtrl>().enabled = false;
	}

	//== Coroutines =====================

	IEnumerator DamageCooldown() {
		PlayerData.isPlayerInvincible = true;
		for (int i = 0; i < PlayerData.damageCooldown * 10; i++) {
			yield return new WaitForSeconds(PlayerData.damageCooldown / 20);
			var c = playerModel.material.color;
			playerModel.material.color = new Color(c.r, c.g, c.b, 0.3f);
			yield return new WaitForSeconds(PlayerData.damageCooldown / 20);
			playerModel.material.color = new Color(c.r, c.g, c.b, 1f);
		}
		PlayerData.isPlayerInvincible = false;
		yield return null;
	}

	IEnumerator HealEffect() {
		Color targetColor = new Color(0, 0.7f, 0, 1f);
		float n = 0;
		foreach (ParticleSystem particle in healParticles) {
			particle.Play();
		}
		for (int i = 0; i < 10; i++) {
			playerModel.material.color = Color.Lerp(targetColor, defaultColor, n);
			playerModel.material.SetColor("_EmissionColor", Color.Lerp(targetColor, Color.black, n));
			n += 0.1f;
			yield return new WaitForSeconds(0.04f);
		}
		playerModel.material.color = defaultColor;
		playerModel.material.SetColor("_EmissionColor", Color.black);
		yield return null;
	}

	IEnumerator KillPlayer() {
		Death();
		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield return null;
	}

}
