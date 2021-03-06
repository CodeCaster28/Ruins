﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformSimple : MonoBehaviour {

	//== Fields =========================

	public enum Direction { Up, Down, Left, Right, Forward, Back }
	[SerializeField]
	public Direction dir;
	public enum BlockHandle { None, Stop, Break, Pong}
	[SerializeField]
	public BlockHandle blockHandle;
	public float distance;
	public bool pingPong;
	public float speed;

	private bool ignoreBlocked;
	private Vector3 direction;
	private Rigidbody rBody;
	private Vector3 startPos;
	private bool stop;
	private bool touchingPlayer;
	private GameObject player;
	private AudioSource audioSrc;

	//== Mono ===========================

	void Start() {
		rBody = GetComponent<Rigidbody>();
		stop = false;
		ignoreBlocked = false;
		touchingPlayer = false;
		startPos = transform.position;
		audioSrc = GetComponent<AudioSource>();
		switch (dir) {
			case Direction.Up:
				direction = Vector3.up;
				break;
			case Direction.Down:
				direction = Vector3.down;
				break;
			case Direction.Left:
				direction = Vector3.left;
				break;
			case Direction.Right:
				direction = Vector3.right;
				break;
			case Direction.Forward:
				direction = Vector3.forward;
				break;
			case Direction.Back:
				direction = Vector3.back;
				break;
		}
	}

	private void Update() {
		if (touchingPlayer)
			CheckPlayerBlocked(player);
	}

	private void FixedUpdate() {
		MoveToPosition();
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color(0.15f, 0.85f, 1, 1);
		Gizmos.DrawSphere(transform.position, 0.3f);
		if (distance != 0) {
			Gizmos.DrawRay(transform.position, GetDirEditor() * distance);
			DrawArrow(false);
			if (pingPong) DrawArrow(true);
		}
	}

	private void DrawArrow(bool pong) {
		float headSize = 0.5f;
		float bank = 20.0f;
		float offset = 0;
		Vector3 vector = new Vector3(0, 0, 1);
		Quaternion rotation = Quaternion.Euler(0, 0, 0);
		if (pong) {
			headSize *= 0.8f;
			offset = -180;
		}
		for (int i = 0; i < 4; i++) {
			if (i % 2 == 1)
				bank *= -1;
			if (i > 1) {
				vector = new Vector3(0, 1, 0);
				rotation = Quaternion.Euler(-90 + offset + bank, 0, 0);
			}
			else
				rotation = Quaternion.Euler(0, 180 + offset + bank, 0);
			Gizmos.DrawRay(transform.position + GetDirEditor() * ( !pong? distance : distance / 3), (Quaternion.LookRotation(GetDirEditor() * distance) * rotation * vector) * headSize);
		}

	}

	private void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "Player") {
			player = other.gameObject;
			touchingPlayer = true;
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == "Player") {
			touchingPlayer = false;
		}
	}

	//== Public ========================

	public Vector3 GetDirection {
		get { return direction; }
	}

	//== Private ========================

	private void CheckPlayerBlocked(GameObject player) {
		if (player.GetComponent<CollisionCtrl>().Blocked) {
			PlatformBlocked();
		}
		else
			stop = false;
	}

	private void PlatformBlocked() {
		switch (blockHandle) {
			case BlockHandle.None:
				break;
			case BlockHandle.Stop:
				stop = true;
				break;
			case BlockHandle.Break:
					audioSrc.PlayOneShot(audioSrc.clip);
					GetComponent<Collider>().enabled = false;
					GetComponent<Renderer>().enabled = false;
					Destroy(gameObject, audioSrc.clip.length);
				break;
			case BlockHandle.Pong:
				if (!ignoreBlocked) {
					ignoreBlocked = true;
					startPos = startPos + (distance * direction);
					direction *= -1;
					StartCoroutine(IgnoreBlocked());
				}
				break;
		}
	}

	private Vector3 GetDirEditor(){
		switch (dir) {
			case Direction.Up:
				return Vector3.up;
			case Direction.Down:
				return Vector3.down;
			case Direction.Left:
				return Vector3.left;
			case Direction.Right:
				return Vector3.right;
			case Direction.Forward:
				return Vector3.forward;
			case Direction.Back:
				return Vector3.back;
			default:
				return Vector3.zero;
		}
	}

	private void MoveToPosition() {
		if (!stop) {
			if (Vector3.Distance(transform.position, startPos) < distance) {
				rBody.MovePosition(transform.position + direction * Time.deltaTime * speed);
			}
			else {
				//rBody.MovePosition(startPos + direction * distance);    // Fix position excess
				stop = true;
				if (pingPong) {
					startPos = transform.position;
					direction *= -1;
					stop = false;
				}
			}
		}
	}

	//== Coroutines =====================

	IEnumerator IgnoreBlocked() {
		yield return new WaitForSeconds(0.2f);
		ignoreBlocked = false;
		yield return null;
	}
}
