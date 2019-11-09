using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
	public Animator _gateAnimator;
	public bool _openTowardsRight = true;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Shark")) {
			SharkAttack sharkAttack = other.gameObject.GetComponentInChildren<SharkAttack>();
			sharkAttack.DoPlayerDetection = false;
			sharkAttack.FadeShark();
			other.gameObject.GetComponent<Rigidbody>().useGravity = true;
			Open();
		}
	}

	public void Open() {
		if (_openTowardsRight)
			_gateAnimator.SetBool("open", true);
		else
			_gateAnimator.SetBool("openInverse", true);
	}

	public void Close() {
		if (_openTowardsRight)
			_gateAnimator.SetBool("open", false);
		else
			_gateAnimator.SetBool("openInverse", false);
	}
}
