using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyScript : MonoBehaviour
{
	public float _moveSpeed;
	public float _maxMoveSpeed;

	private Vector2 _floatDirection = Vector2.up;
	private Rigidbody _rb;

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		if (_rb.velocity.y <= _maxMoveSpeed) {
				Vector3 vect = _floatDirection * _moveSpeed;
                _rb.velocity += vect;
		}
	}
}
