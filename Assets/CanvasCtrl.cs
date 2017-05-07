using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtrl : MonoBehaviour {

	public GameObject heartFull;
	public GameObject heartEmpty;
	private List<GameObject> hearts;

	private void Start() {
		hearts = new List<GameObject>();
		Player.RefreshHearts += RefreshHearts;
	}

	private void RefreshHearts() {
		GameObject heart;
		int n = 0;

		if (hearts.Count > 0) {
			foreach (GameObject x in hearts) {
				Destroy(x);
			}
		}

		for (int i = 0; i < PlayerData.maxHealth; i++) {
			heart = Instantiate(heartEmpty, heartEmpty.transform.position, heartEmpty.transform.rotation) as GameObject;
			hearts.Add(heart);
			heart.transform.SetParent(transform);
			heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 + n, -30, 0);
			n += 32;
		}
		n = 0;

		for (int i = 0; i < PlayerData.health; i++) {
			heart = Instantiate(heartFull, heartFull.transform.position, heartFull.transform.rotation) as GameObject;
			hearts.Add(heart);
			heart.transform.SetParent(transform);
			heart.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 + n, -30, 0);
			n += 32;
		}
	}

	private void OnDestroy() {
		Player.RefreshHearts -= RefreshHearts;
	}
}
