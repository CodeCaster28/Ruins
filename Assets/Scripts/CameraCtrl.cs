using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

	//== Fields =========================

	public Transform target;

	private float rotOffset;
	private Vector3 offset;
	private Vector3 targetRotation;
	private float rotateInput;
	private bool rotateAxisInUse;

	//== Mono ===========================

	private void Start() {
		rotOffset = 0;
		offset = transform.position - target.transform.position;
		rotateAxisInUse = false;
	}

	private void Update() {
		GetInput();
		SetTargetRotation();
	}

	private void LateUpdate() {
		MoveToTarget();
	}

	//== Public ========================

	public float RotOffset {
		get { return rotOffset; }
	}

	//== Private ========================

	private void GetInput() {
		rotateInput = Input.GetAxis("HorCamera");
	}

	private void SetTargetRotation() {
		if (rotateInput > 0 && !rotateAxisInUse) {
			rotOffset += 45;
			rotateAxisInUse = true;
			StartCoroutine(ResetButton());
			iTween.RotateBy(gameObject, new Vector3(0, 0.125f, 0), 0.17f);
		}
		else if (rotateInput < 0 && !rotateAxisInUse) {
			rotOffset -= 45;
			rotateAxisInUse = true;
			StartCoroutine(ResetButton());
			iTween.RotateBy(gameObject, new Vector3(0, -0.125f, 0), 0.17f);
		}
		if (rotOffset == 360) rotOffset = 0;
		if (rotOffset == -45) rotOffset = 315;
	}

	private void MoveToTarget() {
		if(target != null)
			transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, 11f);
	}

	//== Coroutines =====================

	private IEnumerator ResetButton() {
		yield return new WaitForSeconds(0.2f);
		rotateAxisInUse = false;
		yield return null;
	}
}

