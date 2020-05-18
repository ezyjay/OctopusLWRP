using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFX : MonoBehaviour
{
	[Header("Bubbles")]
	public ParticleSystem _bubbleBurst; 
	public ParticleSystem _bubbleIdle;
	public ParticleSystem _bubblesMoving;
	
	[Header("Player Idle")]
	public bool _bobPlayerOnIdle = true;
	public float _bobDistance = 3f;
    public float _smoothTime = 0.3f;

	[Header("Events")]
	public UnityEvent PlayerStartedMoving;
	public UnityEvent PlayerStoppedMoving;

	private GameObject _playerParent;
	private bool _isIdle = false;
	private float _localPositionY = 0f;
	private float _topBobPositionY, _bottomBobPositionY;
	private float _closeDistance = 0.05f;
	private float _lerpPosition = 0f;
    private float _yVelocity = 0.0f;

	private void OnEnable()
	{
		_bottomBobPositionY = _localPositionY;
		_playerParent = GameUtil.Player.transform.parent.gameObject;
		_localPositionY = _playerParent.transform.localPosition.y;
		_topBobPositionY = _localPositionY + _bobDistance;
		GameUtil.Player._controller.PlayerMoving += OnPlayerMoving;
	}

	private void OnDisable() {
		GameUtil.Player._controller.PlayerMoving -= OnPlayerMoving;
	}

	private void Update() {
		if (_bobPlayerOnIdle) {
			if (_isIdle) {
				if (Mathf.Abs(_playerParent.transform.localPosition.y - _topBobPositionY) < _closeDistance) {
					_lerpPosition = _bottomBobPositionY;
				}
				else if (Mathf.Abs(_playerParent.transform.localPosition.y - _bottomBobPositionY) < _closeDistance) {
					_lerpPosition = _topBobPositionY;
				}

				float newY = Mathf.SmoothDamp(_playerParent.transform.localPosition.y, _lerpPosition, ref _yVelocity, _smoothTime);
				_playerParent.transform.localPosition = new Vector3(_playerParent.transform.localPosition.x, newY,_playerParent.transform.localPosition.z);
			}
		}
	}

	public void OnPlayerMoving(bool isMoving) {
		if (isMoving) {
			
			_isIdle = false;
			_bubbleBurst.Play();
			_bubblesMoving.Play();
			_bubbleIdle.Stop();
			PlayerStartedMoving.Invoke();
		}
		else {
			_isIdle = true;
			_bubbleBurst.Stop();
			_bubblesMoving.Stop();
			_bubbleIdle.Play();
			PlayerStoppedMoving.Invoke();
		}
	}
	
}
