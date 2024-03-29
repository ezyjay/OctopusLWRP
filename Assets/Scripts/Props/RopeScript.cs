﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public HingeJoint _lastHingeJointRope;
	private Rigidbody _rb;

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("Shark") || other.CompareTag("Crab")) && _lastHingeJointRope != null) {
			_lastHingeJointRope.gameObject.SetActive(false);
			_rb.isKinematic = false;
			StartCoroutine(WaitRopeFallen());
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	IEnumerator WaitRopeFallen() {
		yield return new WaitForSeconds(3f);
		gameObject.SetActive(false);
	}
}
