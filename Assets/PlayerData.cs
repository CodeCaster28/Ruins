using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerData : GenericSingletonClass<PlayerData> {

	//== Fields =========================

	public Canvas canvas;
	public GameObject heartFull;
	public GameObject heartEmpty;
	public GameObject player;

	private List<GameObject> hearts;
	private MeshRenderer playerModel;
	private int maxHealth;
	private int health;
	private bool isPlayerInvictible;
	// private bool isPlayerDead;
	private float damageCooldown = 1.5f;

	//== Mono ===========================

	private void Start() {
		playerModel = player.transform.Find("Base/Model").gameObject.GetComponent<MeshRenderer>();
		hearts = new List<GameObject>();
		maxHealth = 3;
		health = maxHealth;
		Debug.Log("Setting health to:" + health);
		isPlayerInvictible = false;
	//	isPlayerDead = false;
		GuiRefreshHearts();
	}

	//== Public ========================

	public void Damage(int x) {
		Debug.Log("DAMAGE " + isPlayerInvictible + " " + health);
		Debug.Log(isPlayerInvictible);
		if (isPlayerInvictible == false) {

			foreach (GameObject heart in hearts) {
				Destroy(heart);
			}
			health -= x;
			if (health > maxHealth) health = maxHealth;
			if (health < 0) health = 0;

			GuiRefreshHearts();

			if (health <= 0) {
			//	isPlayerDead = true;
				isPlayerInvictible = true;
				StartCoroutine(KillPlayer());
			}
			else StartCoroutine(DamageCooldown());
		}
	}

	//== Private ========================

	private void GuiRefreshHearts() {

		GameObject heart;
		int n = 0;

		for (int i = 0; i < maxHealth; i++) {
			heart = Instantiate(heartEmpty, heartEmpty.transform.position, heartEmpty.transform.rotation) as GameObject;
			hearts.Add(heart);
			heart.transform.SetParent(canvas.transform);
			heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 + n, -30, 0);
			n += 32;
		}
		n = 0;

		for (int i = 0; i < health; i++) {
			heart = Instantiate(heartFull, heartFull.transform.position, heartFull.transform.rotation) as GameObject;
			hearts.Add(heart);
			heart.transform.SetParent(canvas.transform);
			heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 + n, -30, 0);
			n += 32;
		}
	}

	IEnumerator DamageCooldown() {
		isPlayerInvictible = true;
		for(int i=0; i < damageCooldown * 10; i++) {
			yield return new WaitForSeconds(damageCooldown / 20);
			var c = playerModel.material.color;
			playerModel.material.color = new Color(c.r, c.g, c.b, 0.3f);
			yield return new WaitForSeconds(damageCooldown / 20);
			playerModel.material.color = new Color(c.r, c.g, c.b, 1f);
		}
		isPlayerInvictible = false;
		yield return null;
	}

	IEnumerator KillPlayer() {
		Destroy(player);
		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		// yield return null;
	}
}
