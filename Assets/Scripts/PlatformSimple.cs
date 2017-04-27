using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformSimple : MonoBehaviour {

	public enum Direction { Up, Down, Left, Right, Forward, Back }
	[SerializeField]
	public Direction dir;
	public float distance;
	public bool pingPong;
	public float speed;

	private Vector3 direction;
	private Rigidbody rBody;
	private Vector3 startPos;
	private bool stop;
	private bool touchingPlayer;
	private GameObject player;

	void Start() {
		rBody = GetComponent<Rigidbody>();
		stop = false;
		touchingPlayer = false;
		startPos = transform.position;
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
		if (touchingPlayer == true)
			CheckPlayerBlocked(player);
	}

	void FixedUpdate() {


			MoveToPosition();
	}

	public Vector3 GetDirection {
		get { return direction; }
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(0.15f, 0.85f, 1, 1);
		Gizmos.DrawSphere(transform.position, 0.3f);
		Gizmos.DrawRay(transform.position, GetDirEditor() * distance);
		Vector3 right = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(0, 180 + 20.0f, 0) * new Vector3(0, 0, 1);
		Vector3 left = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(0, 180 - 20.0f, 0) * new Vector3(0, 0, 1);
		Vector3 front = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(-90 + 20.0f, 0, 0) * new Vector3(0, 1, 0);
		Vector3 back = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(-90 - 20.0f, 0, 0) * new Vector3(0, 1, 0);
		Gizmos.DrawRay(transform.position + GetDirEditor() * distance, right * 0.5f);
		Gizmos.DrawRay(transform.position + GetDirEditor() * distance, left * 0.5f);
		Gizmos.DrawRay(transform.position + GetDirEditor() * distance, front * 0.5f);
		Gizmos.DrawRay(transform.position + GetDirEditor() * distance, back * 0.5f);
		if(pingPong == true) {
			Vector3 rightp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(0, 0 + 20.0f, 0) * new Vector3(0, 0, 1);
			Vector3 leftp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(0, 0 - 20.0f, 0) * new Vector3(0, 0, 1);
			Vector3 frontp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(-270 + 20.0f, 0, 0) * new Vector3(0, 1, 0);
			Vector3 backp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(-270 - 20.0f, 0, 0) * new Vector3(0, 1, 0);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, rightp * 0.4f);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, leftp * 0.4f);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3 , frontp * 0.4f);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, backp * 0.4f);
		}
	}

	private void OnCollisionStay(Collision other) {
		if(other.gameObject.tag == "Player") {
			player = other.gameObject;
			touchingPlayer = true;
		}
	}

	private void CheckPlayerBlocked(GameObject player) {
		if (player.GetComponent<CollisionCtrl>().Blocked) {
			rBody.MovePosition(transform.position - direction * 0.05f * speed);
			enabled = false;
			stop = true;
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
		if (stop == false) {
			if (Vector3.Distance(transform.position, startPos) < distance) {
				rBody.MovePosition(transform.position + direction * Time.deltaTime * speed);
			}
			else {
				rBody.MovePosition(startPos + direction * distance);    // Fix position excess
				stop = true;
				if (pingPong) {
					startPos = transform.position;
					direction *= -1;
					stop = false;
				}
			}
		}
	}
}
