﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyScript : MonoBehaviour
{
	public float _moveSpeed;
	public float _maxMoveSpeed;
	public HingeJoint _lastHingeJointRope;
	public Rigidbody _ropeRB;

	private Vector2 _floatDirection = Vector2.up;
	private Rigidbody _rb;

    public void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag("Shark") && _lastHingeJointRope != null) {
			_lastHingeJointRope.gameObject.SetActive(false);
			_ropeRB.isKinematic = false;
			StartCoroutine(WaitRopeFallen());
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
	}

	IEnumerator WaitRopeFallen() {
		yield return new WaitForSeconds(3f);
		_ropeRB.gameObject.SetActive(false);
	}

	private void FixedUpdate() {
		if (_rb.velocity.y <= _maxMoveSpeed) {
				Vector3 vect = _floatDirection * _moveSpeed;
                _rb.velocity += vect;
		}
	}
}
