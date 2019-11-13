using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
	public Animator _gateAnimator;
	public bool _openTowardsRight = true;
	public bool _isSolid = true;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Shark")) {

			//If solid gate, shark dies
			if(_isSolid) {
				Open();
				SharkAttack sharkAttack = other.gameObject.GetComponentInChildren<SharkAttack>();
				sharkAttack.DoPlayerDetection = false;
				sharkAttack.FadeShark();
				other.gameObject.GetComponent<Rigidbody>().useGravity = true;
			}

			//If wooden gate, gate breaks, shark continues
			else {
				foreach (Transform t in transform) {
					Rigidbody rb = t.GetComponent<Rigidbody>();
					rb.isKinematic = false;
					// if (_openTowardsRight)
					// 	rb.AddForce(Vector2.right * 10,ForceMode.Impulse);
					// else
					// 	rb.AddForce(Vector2.left * 10,ForceMode.Impulse);
				}
			}
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
