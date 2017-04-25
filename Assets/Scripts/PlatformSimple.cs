using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformSimple : MonoBehaviour {

	public enum Direction { Up, Down, Left }

	public Direction dir;
	public float distance;
	public bool pingPong;
	public float speed;

	private Vector3 direction;
	private Rigidbody rBody;
	private Vector3 startPos;
	private bool stop;


	void Start() {
		rBody = GetComponent<Rigidbody>();
		stop = false;
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
		}
	}

	private void Update() {

	}

	void FixedUpdate() {

		MoveToPosition();
	}

	public Vector3 GetDirection {
		get { return direction; }
	}

	private void MoveToPosition() {
		if (stop == false && Vector3.Distance(transform.position, startPos) < distance) {
			rBody.MovePosition(transform.position + direction * Time.deltaTime * speed);
		}
		else {
			rBody.MovePosition(startPos + direction * distance);	// Fix position excess
			stop = true;
			if (pingPong) {
				startPos = transform.position;
				direction *= -1;
				stop = false;
			}
		}
	}

}
