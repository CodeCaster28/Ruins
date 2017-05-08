using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : GenericSingletonClass<PlayerData> {

	//== Fields =========================

	public static int maxHealth;
	public static int health;
	public static bool isPlayerInvincible;
	public static float damageCooldown = 1.0f;


}
