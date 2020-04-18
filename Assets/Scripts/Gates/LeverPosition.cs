using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPosition : MonoBehaviour
{
	public float _force = 10f;
	public Rigidbody _rigidbody;
    public Transform _leverHandle;
	public Transform _leftSide, _rightSide;

    private void Update()
    {
        //If closest to left side
		if (Vector3.Distance(_leverHandle.position, _leftSide.position) <= Vector3.Distance(_leverHandle.position, _rightSide.position)) {
			_rigidbody.AddForce(Vector3.left * _force);
		}
		//If closest to right side
		else {
			_rigidbody.AddForce(Vector3.right * _force);
		}
    }
}
