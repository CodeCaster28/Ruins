﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

	//== Fields =========================

	public float inputDelay = 0.1f;
	public float maxVel = 7;
	public GameObject pairedCamera;
	public float jumpForce;

	//public GameObject playerModel;
	private GameObject playerModel;
	//private BoxCollider collisionBox;
	private Rigidbody rBody;

	private Feets feets;
	

	private float currentVel;
	private Quaternion targetRotation;
	private RaycastHit groundHit;
	private Vector3 direction;
	private float forwardInput, sideInput;
	private Coroutine jumpCoroutine = null;

	//== Mono ===========================

	void Update() {
		GetInput();
	}

	private void FixedUpdate() {
		Run();
		Turn();
	}

	void Start () {
		currentVel = maxVel;
		targetRotation = transform.rotation;
		forwardInput = sideInput = 0;

		playerModel = transform.Find("Base/Model").gameObject;
		if (playerModel == null) Debug.Log("No player model 'Player/Base/Model' gameobject found!");

		
		if (transform.Find("Base").gameObject == null) Debug.Log("No player base 'Player/Base' gameobject found!");
		//collisionBox = transform.Find("Base").gameObject.GetComponent<BoxCollider>();
		rBody = transform.Find("Base").gameObject.GetComponent<Rigidbody>();

		if (transform.Find("Base/Feets").gameObject == null) Debug.Log("No player model 'Player/Base/Feets' gameobject found!");
		feets = transform.Find("Base/Feets").gameObject.GetComponent<Feets>();
	}

	//== Publics ========================

	public Quaternion TargetRotation {
		get { return targetRotation; }
	}

	//== Methods ========================

	private void GetInput() {
		forwardInput = Input.GetAxis("Vertical");
		sideInput = Input.GetAxis("Horizontal");
		if (Input.GetButtonDown("Jump")) {
			Jump();
		}
	}

	private bool IsGrounded() {
		if (feets.OnGround())
			return true;
		else return false;
	}

	private void Run() {
		var momentum = Vector3.zero;

		// Generate momentum from rigidbody player standing on
		// Debug.Log("Last collider picked: " + feets.GetComponent<Feets>().GetLastCollider());
		if (feets.GetLastCollider() != null) {
			momentum = feets.GetLastCollider().GetComponent<Rigidbody>().velocity;
			if (momentum.y > 0) momentum.y = 0;
		}
		else
			momentum = Vector3.zero;

		// Move the player basing on pressed keys, add momentum to velocity
		if (Mathf.Abs(forwardInput) > inputDelay || Mathf.Abs(sideInput) > inputDelay) {
			currentVel = maxVel;
			direction = new Vector3(sideInput, 0f, forwardInput);

			// Moving based on camera rotation:
			direction = Quaternion.Euler(0, pairedCamera.GetComponent<CameraCtrl>().RotOffset, 0) * direction;

			// Cap diagonal speed
			if (direction.magnitude > 1)
				direction = direction / direction.magnitude;

			// Air control velocity is smaller
			var vel = direction * (IsGrounded() == true ? currentVel : currentVel * 0.93f);
			rBody.drag = 0;
			vel.y = rBody.velocity.y;
			rBody.velocity = vel + momentum;

			targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		}
		else if (IsGrounded()) {
				var vel = Vector3.zero;
				rBody.velocity = momentum;
				rBody.drag = (momentum == Vector3.zero ? 60 : 0);
		}
		else
			rBody.drag = 0;
	}

	private void Jump() {
		if (IsGrounded() == true) {
			StartCoroutine(Jumping());
			rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			//Debug.Log("Jump");
		}
		else {
			if (jumpCoroutine != null)
				StopCoroutine(jumpCoroutine);
			jumpCoroutine = StartCoroutine(DelayJump());
		}
	}
	
	private void Turn() { 
		targetRotation = Quaternion.Slerp(playerModel.transform.rotation, targetRotation, Time.deltaTime * 12f);
		playerModel.transform.rotation = targetRotation;
	}

	IEnumerator DelayJump() {

		for(int i = 0; i < 3; i++) {
			yield return new WaitForSeconds(0.03f);
			if (IsGrounded() == true) {
				Jump();
				yield return null;
			}
		}
		yield return null;
	}

	IEnumerator Jumping() {
		feets.ForceFlight = true;
		yield return new WaitForSeconds(0.1f);
		feets.ForceFlight = false;
		yield return null;
	}
}
