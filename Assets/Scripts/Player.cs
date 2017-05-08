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
	private GameObject player;
	
	//== Mono ===========================

	private void Start() {
		PlayerData.health = maxHealth;
		PlayerData.maxHealth = maxHealth;
		PlayerData.isPlayerInvictible = false;
		player = GameObject.Find("Player");
		playerModel = player.transform.Find("Base/Model").gameObject.GetComponent<MeshRenderer>();
		if (RefreshHearts != null) {
			RefreshHearts();
		}
	}

	//== Public ========================

	public void Damage(int x) {
		if (!PlayerData.isPlayerInvictible) {
			PlayerData.health -= x;
			if (PlayerData.health > PlayerData.maxHealth) {
				PlayerData.health = PlayerData.maxHealth;
			}
			if (PlayerData.health < 0) {
				PlayerData.health = 0;
			}
			if (RefreshHearts != null) {
				RefreshHearts();
			}
			if (PlayerData.health <= 0) {

				PlayerData.isPlayerInvictible = true;
				StartCoroutine(KillPlayer());
			}
			else StartCoroutine(DamageCooldown());
		}
	}

	//== Private ========================

	private void Death() {
		player.GetComponent<CharacterCtrl>().DisableInputs = true;
		player.GetComponent<CharacterCtrl>().enabled = false;
	}

	//== Coroutines =====================

	IEnumerator DamageCooldown() {
		PlayerData.isPlayerInvictible = true;
		for (int i = 0; i < PlayerData.damageCooldown * 10; i++) {
			yield return new WaitForSeconds(PlayerData.damageCooldown / 20);
			var c = playerModel.material.color;
			playerModel.material.color = new Color(c.r, c.g, c.b, 0.3f);
			yield return new WaitForSeconds(PlayerData.damageCooldown / 20);
			playerModel.material.color = new Color(c.r, c.g, c.b, 1f);
		}
		PlayerData.isPlayerInvictible = false;
		yield return null;
	}

	IEnumerator KillPlayer() {
		Death();
		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield return null;
	}

}
