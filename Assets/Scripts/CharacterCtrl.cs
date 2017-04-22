using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

	//== Fields =========================

	public float inputDelay = 0.1f;
	public float maxVel = 7;
	public GameObject pairedCamera;
	public float jumpForce;
	public GameObject playerModel;
	public BoxCollider collisionBox;
	public GameObject feets;

	private float currentVel;
	//private float distanceToGround;
	private Quaternion targetRotation;
	private RaycastHit groundHit;
	private Rigidbody rBody;
	private Vector3 direction;
	private float forwardInput, sideInput;
	//private bool isGrounded;

	//== Mono ===========================

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Jump();
		}
		GetInput();
		
		//Debug.DrawRay(collisionBox.transform.position, new Vector3 (0,-distanceToGround, 0), Color.green);
	}

	private void FixedUpdate() {
		Run();
		Turn();
		/*if (!IsGrounded()) {
			currentVel -= 0.1f;
		}
		else currentVel = maxVel;*/
	}

	void Start () {
		currentVel = maxVel;
		targetRotation = transform.rotation;
		//distanceToGround = (transform.localScale.y * collisionBox.size.y) / 2;

		if (GetComponent<Rigidbody>()) {
			rBody = GetComponent<Rigidbody>();
		}
		else Debug.LogError("Player has no rigidbody");
		forwardInput = sideInput = 0;

	}

	//== Publics ========================

	public Quaternion TargetRotation {
		get { return targetRotation; }
	}



	//== Methods ========================

	private void GetInput() {
		forwardInput = Input.GetAxis("Vertical");
		sideInput = Input.GetAxis("Horizontal");

	}

	private bool IsGrounded() {
		/*if (Physics.Raycast(collisionBox.transform.position, Vector3.down, distanceToGround)) {
			return true;
		}
		else {
			return false;
		}*/

		if (feets.GetComponent<Feets>().OnGround())
			return true;
		else return false;
	}

	private void Run() {

		if (Mathf.Abs(forwardInput) > inputDelay || Mathf.Abs(sideInput) > inputDelay) {
			currentVel = maxVel;
			direction = new Vector3(sideInput, 0f, forwardInput);

			// Moving based on camera rotation:
			direction = Quaternion.Euler(0, pairedCamera.GetComponent<CameraCtrl>().RotOffset, 0) * direction;

			// Cap diagonal speed
			if (direction.magnitude > 1)
				direction = direction / direction.magnitude;

			// * (IsGrounded() == true ? moveVel : moveVel/3)
			var vel = direction * (IsGrounded() == true ? currentVel : currentVel * 0.93f);
			vel.y = rBody.velocity.y;
			rBody.velocity = vel;

			//rBody.AddForce((direction * currentVel), ForceMode.VelocityChange);

			targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		}
		else {
			if (IsGrounded()) { 
				var vel = Vector3.zero;
				vel.y = rBody.velocity.y;
				rBody.velocity = vel;
			}
			//else currentVel -= 0.1f;
		}
	}

	private void Jump() {
		if (IsGrounded() == true) {
			rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			Debug.Log("Jump");
		}
	}

	private void Turn() { 
		targetRotation = Quaternion.Slerp(playerModel.transform.rotation, targetRotation, Time.deltaTime * 12f);
		playerModel.transform.rotation = targetRotation;
	}
}
