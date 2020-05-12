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

	public Transform _movePositionOut, _movePositionIn;

	private Vector3 _lerpPosition = Vector3.zero;
	private float _closeDistance = 0.1f;
	private Vector3 _idlePosition1, _idlePosition2, _newIdlePosition;
	private Vector3 _movementPosition1, _movementPosition2, _newMovementPosition;

	void Awake()
	{
		_lerpPosition = _idlePosition.position;	

		_idlePosition1 = _idlePosition.localPosition;
		_idlePosition2 = _idlePosition.localPosition + _idleMovementVariance;
		_movementPosition1 = _movementPosition.localPosition;
		_movementPosition2 = _movementPosition.localPosition + _movementVariance;
	}

	private void Update()
	{
		if (_playerController._rb.velocity.magnitude <= 0.1f) {

			if (Vector3.Distance(_idlePosition.localPosition, _idlePosition1) < _closeDistance)
				_newIdlePosition = _idlePosition2;
			else if (Vector3.Distance(_idlePosition.localPosition, _idlePosition2) < _closeDistance)
				_newIdlePosition = _idlePosition1;
			
			_idlePosition.localPosition = Vector3.Lerp(_idlePosition.localPosition, _newIdlePosition, _idleLerpSpeed * Time.deltaTime);
			_lerpPosition = _idlePosition.position;	
		}
		else {
			// if (Vector3.Distance(_movementPosition.transform.localPosition, _movementPosition1) < _closeDistance)
			// 	_newMovementPosition= _movementPosition2;
			// else if (Vector3.Distance(_movementPosition.transform.localPosition, _movementPosition2) < _closeDistance)
			// 	_newMovementPosition = _movementPosition1;

			if (Vector3.Distance(_movementPosition.localPosition, _movePositionIn.localPosition) < _closeDistance)
				_newMovementPosition = _movePositionOut.localPosition;
			else if (Vector3.Distance(_movementPosition.localPosition, _movePositionOut.localPosition) < _closeDistance)
				_newMovementPosition = _movePositionIn.localPosition;

			_movementPosition.localPosition = Vector3.Lerp(_movementPosition.localPosition, _newMovementPosition, _movementLerpSpeed * Time.deltaTime);
			_lerpPosition = _movementPosition.position;	
		}

		_targetToMove.position = Vector3.Lerp(_targetToMove.position, _lerpPosition, _targetLerpSpeed * Time.deltaTime);
	}

}
