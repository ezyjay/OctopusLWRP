using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
	public Controller _playerController;
	public Transform _targetToMove;
	public float _targetLerpSpeed = 1f, _movementLerpSpeed = 1f, _idleLerpSpeed = 1f;
    public Transform _idlePosition;
	public Transform _movementPosition;
	public Vector3 _idleMovementVariance, _movementVariance;

	private Vector3 _lerpPosition = Vector3.zero;
	private float _closeDistance = 0.1f;
	private Vector3 _idlePosition1, _idlePosition2, _newIdlePosition;
	private Vector3 _movementPosition1, _movementPosition2, _newMovementPosition;

	void Awake()
	{
		_lerpPosition = _idlePosition.transform.position;	

		_idlePosition1 = _idlePosition.transform.localPosition;
		_idlePosition2 = _idlePosition.transform.localPosition + _idleMovementVariance;
		_movementPosition1 = _movementPosition.transform.localPosition;
		_movementPosition2 = _movementPosition.transform.localPosition + _movementVariance;
	}

	private void Update()
	{
		if (_playerController._rb.velocity.magnitude <= 0.1f) {


			if (Vector3.Distance(_idlePosition.transform.localPosition, _idlePosition1) < _closeDistance)
				_newIdlePosition = _idlePosition2;
			else if (Vector3.Distance(_idlePosition.transform.localPosition, _idlePosition2) < _closeDistance)
				_newIdlePosition = _idlePosition1;
			
			_idlePosition.transform.localPosition = Vector3.Lerp(_idlePosition.transform.localPosition, _newIdlePosition, _idleLerpSpeed * Time.deltaTime);
			_lerpPosition = _idlePosition.transform.position;	
		}
		else {
			if (Vector3.Distance(_movementPosition.transform.localPosition, _movementPosition1) < _closeDistance)
				_newMovementPosition= _movementPosition2;
			else if (Vector3.Distance(_movementPosition.transform.localPosition, _movementPosition2) < _closeDistance)
				_newMovementPosition = _movementPosition1;

			_movementPosition.transform.localPosition = Vector3.Lerp(_movementPosition.transform.localPosition, _newMovementPosition, _movementLerpSpeed * Time.deltaTime);
			_lerpPosition = _movementPosition.transform.position;	
		}

		_targetToMove.transform.position = Vector3.Lerp(_targetToMove.transform.position, _lerpPosition, _targetLerpSpeed * Time.deltaTime);
	}

}
