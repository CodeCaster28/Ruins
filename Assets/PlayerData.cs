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
	private int maxHealth;
	private int health;
	private bool isPlayerInvictible;

	//== Mono ===========================

	private void Start() {
		hearts = new List<GameObject>();
		maxHealth = 5;
		health = maxHealth;
		isPlayerInvictible = false;
		GuiRefreshHearts();
	}

	//== Public ========================

	public void Damage(int x) {
		if (isPlayerInvictible == false) {
			//if ((health - x) >= 0 && (health - x) <= maxHealth) {
			foreach (GameObject heart in hearts) {
				Destroy(heart);
			}
			health -= x;
			if (health > maxHealth) health = maxHealth;
			if (health < 0) health = 0;

			GuiRefreshHearts();

			if (health <= 0) {
				isPlayerInvictible = true;
				StartCoroutine(KillPlayer());
			}
		}
	}

	//== Private ========================

	private void GuiRefreshHearts() {
		int n = 0;
		GameObject heart;

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

	IEnumerator KillPlayer() {
		Destroy(player);
		yield return new WaitForSeconds(2.0f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield return null;
	}
}
