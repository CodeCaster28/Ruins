using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	private Canvas canvas;
	private MeshRenderer playerModel;
	private GameObject player;
	private List<GameObject> hearts;

	public int maxHealth;
	public GameObject heartFull;
	public GameObject heartEmpty;

	private void Start() {
		PlayerData.health = maxHealth;
		PlayerData.maxHealth = maxHealth;
		PlayerData.isPlayerInvictible = false;
		Debug.Log("Starting, hp: " + PlayerData.health);
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		player = GameObject.Find("Player");
		playerModel = player.transform.Find("Base/Model").gameObject.GetComponent<MeshRenderer>();
		hearts = new List<GameObject>();

		GuiRefreshHearts();
	}

	//== Public ========================

	public void Damage(int x) {
		Debug.Log("Damaging: " + PlayerData.health + "-" + x);
		if (PlayerData.isPlayerInvictible == false) {

			foreach (GameObject heart in hearts) {
				Destroy(heart);
			}
			PlayerData.health -= x;
			if (PlayerData.health > PlayerData.maxHealth) {
				Debug.Log("First var");
				PlayerData.health = PlayerData.maxHealth;
			}
			if (PlayerData.health < 0) {
				Debug.Log("Second var");
				PlayerData.health = 0;
			}

			GuiRefreshHearts();

			if (PlayerData.health <= 0) {

				PlayerData.isPlayerInvictible = true;
				StartCoroutine(KillPlayer());
			}
			else StartCoroutine(DamageCooldown());
		}
	}

	//== Private ========================

	// TODO: move to gui manager, create delegates
	private void GuiRefreshHearts() {

		GameObject heart;
		int n = 0;

		for (int i = 0; i < PlayerData.maxHealth; i++) {
			heart = Instantiate(heartEmpty, heartEmpty.transform.position, heartEmpty.transform.rotation) as GameObject;
			hearts.Add(heart);
			heart.transform.SetParent(canvas.transform);
			heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 + n, -30, 0);
			n += 32;
		}
		n = 0;

		for (int i = 0; i < PlayerData.health; i++) {
			heart = Instantiate(heartFull, heartFull.transform.position, heartFull.transform.rotation) as GameObject;
			hearts.Add(heart);
			heart.transform.SetParent(canvas.transform);
			heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 + n, -30, 0);
			n += 32;
		}
	}

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

	// TODO: move to scene manager
	// TODO: create kill player method
	IEnumerator KillPlayer() {
		Death();

		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield return null;
	}

}
