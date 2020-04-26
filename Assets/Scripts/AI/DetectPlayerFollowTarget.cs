using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerFollowTarget : FollowTarget
{
    [Header("Player Detection")]
	public bool _alwaysDetectPlayer = true;
	public bool _resetPlayerDetectedWhenOutOfRaycast = true;
	public float _detectionDistance;
   	public float _timePlayerInRayBeforeAction;
   	public float _speedWhenDetected;
  	public Transform _raycastOrigin;
   	public float _sphereCastRadius = 1.5f;
   	public SpriteRenderer _exclamation;
    public bool _useFieldOfView = false;
    public float _fieldOfView = 20f;

	protected RaycastHit _raycastHit;
   	protected float _timePlayerDetected;
   	protected bool _playerDetected;
	protected bool _doPlayerDetection = true;
	protected Vector3 _playerToObjectDirection = Vector3.zero;

   	public bool DoPlayerDetection { get => _doPlayerDetection; set => _doPlayerDetection = value; }

	protected virtual void Awake() {
	   _exclamation.gameObject.SetActive(false);
	}

	protected override void FixedUpdate()
	{
			if (_doPlayerDetection) {

				//Detect player
				if (_alwaysDetectPlayer || !_alwaysDetectPlayer && !_playerDetected) 
					DetectPlayer();

				//If player detected, attack, otherwise do base movement
				if (_playerDetected) 
					PlayerDetectedBehaviour();	
				else 
					base.FixedUpdate();
			}
	}

	protected virtual void OnPlayerJustDetected() {}

	protected virtual void PlayerDetectedBehaviour() {}

	protected virtual bool PlayerDetectable() {
		return true;
	}

	protected bool IsPlayerInFieldOfView() {
		return !_useFieldOfView || _useFieldOfView && Vector3.Angle(_direction, _playerToObjectDirection) < _fieldOfView;
	}

	protected virtual void DetectPlayer() {

		//Get vector between shark and player
		_playerToObjectDirection = GameUtil.Player.transform.position - transform.position;
				
		if (IsPlayerInFieldOfView()) {
			
			//Ray cast to see if not behind an obstacle
			Debug.DrawRay(_raycastOrigin.position, _playerToObjectDirection.normalized * _detectionDistance, Color.green);
			Physics.SphereCast(_raycastOrigin.position, _sphereCastRadius, _playerToObjectDirection, out _raycastHit, _detectionDistance);

			//If we've hit the player
			if(_raycastHit.collider != null && _raycastHit.collider.CompareTag("Player")) {

				Debug.DrawRay(_raycastOrigin.position, _playerToObjectDirection.normalized * _detectionDistance, Color.red);

				//If we haven't started the timer start it, ie player just seen
				if (_timePlayerDetected == 0f && PlayerDetectable()) {
					OnPlayerJustDetected();
					_timePlayerDetected = Time.time;
					_exclamation.color = new Color(_exclamation.color.r, _exclamation.color.g, _exclamation.color.r, 0);
					_exclamation.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
					_exclamation.gameObject.SetActive(true);
				
				//Timer has started, lerp indicators
				} else {
					_exclamation.transform.localScale = Vector3.Lerp(_exclamation.transform.localScale, Vector3.one * 0.1f, _timePlayerInRayBeforeAction * Time.deltaTime);
					_exclamation.color = Color.Lerp(_exclamation.color, new Color(_exclamation.color.r, _exclamation.color.g, _exclamation.color.r, 1), _timePlayerInRayBeforeAction * Time.deltaTime);
					
				}

				//If the player can be detected and was seen for long enoug
				if (PlayerDetectable() && (_alwaysDetectPlayer || _timePlayerDetected != 0 && Time.time - _timePlayerDetected >= _timePlayerInRayBeforeAction)) {
					_playerDetected = true;
				}
			}
			//If raycast didn't hit the player, reset timer 
			else if (Time.time - _timePlayerDetected > 0.2f) {
				_timePlayerDetected = 0f;
				_exclamation.gameObject.SetActive(false);
				if (_resetPlayerDetectedWhenOutOfRaycast)
					_playerDetected = false;
			}
		} 
		
		//if player not in field of view, reset timer
		else if (_useFieldOfView && Vector3.Angle(_direction, _playerToObjectDirection) > _fieldOfView)
		{
			_timePlayerDetected = 0f;
			_exclamation.gameObject.SetActive(false);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(_raycastOrigin.position, _sphereCastRadius);
	}
}
