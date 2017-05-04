using System.Collections;
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
		if (touchingPlayer == true)
			CheckPlayerBlocked(player);
	}

	private void FixedUpdate() {
		MoveToPosition();
	}

	private void OnDrawGizmos() {
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
		if (pingPong == true) {
			Vector3 rightp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(0, 0 + 20.0f, 0) * new Vector3(0, 0, 1);
			Vector3 leftp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(0, 0 - 20.0f, 0) * new Vector3(0, 0, 1);
			Vector3 frontp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(-270 + 20.0f, 0, 0) * new Vector3(0, 1, 0);
			Vector3 backp = Quaternion.LookRotation(GetDirEditor() * distance) * Quaternion.Euler(-270 - 20.0f, 0, 0) * new Vector3(0, 1, 0);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, rightp * 0.4f);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, leftp * 0.4f);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, frontp * 0.4f);
			Gizmos.DrawRay(transform.position + GetDirEditor() * distance / 3, backp * 0.4f);
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
				if (ignoreBlocked == false) {
					ignoreBlocked = true;
					
					Debug.Log("Old: " + startPos + " , New: " + (startPos + (distance * direction)));

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
		if (stop == false) {
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

	IEnumerator IgnoreBlocked() {
		yield return new WaitForSeconds(0.2f);
		ignoreBlocked = false;
		yield return null;
	}
}
