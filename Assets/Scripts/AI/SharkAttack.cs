using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttack : FollowTarget
{
	[Header("Shark attack")]
   public float _detectionDistance;
   public float _timePlayerInRayBeforeAttack;
   public float _attackSpeed;
   public Transform _raycastOrigin;
   public float _sphereCastRadius = 1.5f;
   public SpriteRenderer _exclamation;
   public float fieldOfView = 20f;

   private RaycastHit _raycastHit;
   private float _timePlayerDetected;
   private bool _playerDetected;
   private ColorDetection _playerColor;
   private bool _doPlayerDetection = true;
   private MeshRenderer _renderer;

   private Vector2 _targetPos = Vector2.zero;
   private Vector2 _savedTarget, _playerToSharkDirection = Vector2.zero, _point1Position;
   private WanderBetweenPoints _wanderPoints;

   public bool DoPlayerDetection { get => _doPlayerDetection; set => _doPlayerDetection = value; }

   private void Awake() {
	   _playerColor = GameUtil.PlayerObject.GetComponent<ColorDetection>();
	   _renderer = GetComponent<MeshRenderer>();
	   _wanderPoints = _target as WanderBetweenPoints;
	   _exclamation.gameObject.SetActive(false);
   }

   protected override void FixedUpdate()
   {
		if (_doPlayerDetection) {

			//Detect player
			if (!_playerDetected) DetectPlayer();

			//If player detected, attack, otherwise do base movement
			if (_playerDetected) DoAttack();	
			else base.FixedUpdate();
		}
   }

	private void DetectPlayer() {

		//Get vector between shark and player
		if (GameUtil.PlayerObject)
			_playerToSharkDirection = GameUtil.PlayerObject.transform.position - transform.position;
				
		//If player is in field of view
		if (Vector3.Angle(_direction, _playerToSharkDirection) < fieldOfView) {
			
			//Ray cast to see if not behind an obstacle
			Debug.DrawRay(_raycastOrigin.position, _playerToSharkDirection.normalized * _detectionDistance, Color.red);
			Physics.SphereCast(_raycastOrigin.position, _sphereCastRadius, _playerToSharkDirection, out _raycastHit, _detectionDistance);

			//If we've hit the player
			if(_raycastHit.collider != null && _raycastHit.collider.CompareTag("Player")) {

				//If we haven't started the timer start it, ie player just seen
				if (_timePlayerDetected == 0f) {
					_savedTarget = _raycastHit.point;
					_point1Position = transform.position;
					_timePlayerDetected = Time.time;
					_exclamation.color = new Color(_exclamation.color.r, _exclamation.color.g, _exclamation.color.r, 0);
					_exclamation.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
					_exclamation.gameObject.SetActive(true);
				
				//Timer has started, lerp indicators
				} else {
					_exclamation.transform.localScale = Vector3.Lerp(_exclamation.transform.localScale, Vector3.one * 0.1f, _timePlayerInRayBeforeAttack * Time.deltaTime);
					_exclamation.color = Color.Lerp(_exclamation.color, new Color(_exclamation.color.r, _exclamation.color.g, _exclamation.color.r, 1), _timePlayerInRayBeforeAttack * Time.deltaTime);
				}

				//If the player can be detected and was seen for long enoug
				if (!_playerColor.IsHidden() && _timePlayerDetected != 0 && Time.time - _timePlayerDetected >= _timePlayerInRayBeforeAttack) {
					_playerDetected = true;
					_exclamation.gameObject.SetActive(false);
				}
			}
			//If raycast didn't hit the player, reset timer 
			else {
				_timePlayerDetected = 0f;
				_exclamation.gameObject.SetActive(false);
			}
		} 
		
		//if player not in field of view, reset timer
		else {
			_timePlayerDetected = 0f;
			_exclamation.gameObject.SetActive(false);
		}

	}

   private void DoAttack() {

	   Vector2 direction = (Vector2)transform.position - _savedTarget;
		_rotation.RotateTowardsDirection(direction);

		//Calculate target slighlty further than detected player
		if (_targetPos == Vector2.zero) {

			_targetPos = (Vector2)_savedTarget + direction.normalized * -5;

			//If target pos is past an obstacle
			RaycastHit hitInfo;
			if  (Physics.Linecast(_raycastOrigin.position, _targetPos, out hitInfo, GameUtil.GetLayerMask(LayerType.LEVEL)))
				_targetPos = hitInfo.point;
				
		}

		//Move towards player
		else {
			transform.position = Vector3.MoveTowards(transform.position, _targetPos, _attackSpeed * Time.deltaTime);
			
			//Go back to normal behaviour after attack
			if (Vector2.Distance(transform.position, _targetPos) < 1.5f ) {

				if (Vector2.Distance(_point1Position, _targetPos) > 10f) {
					
					//If target pos is past an obstacle
					RaycastHit hitInfo;
					if  (Physics.Linecast(_raycastOrigin.position, _point1Position, out hitInfo, GameUtil.GetLayerMask(LayerType.LEVEL)))
						_point1Position = hitInfo.point;

					//Set next wander points
					_wanderPoints._point1.position = _point1Position;
					_wanderPoints._point2.position = _targetPos;
				}
				_playerDetected = false;
				_targetPos = Vector2.zero;
			}
		}
   }

   public void FadeShark() {
	   StartCoroutine(FadeAndDisable(0f, 2f));
   }

	private IEnumerator FadeAndDisable(float aValue, float aTime) {
		Color oldColor = _renderer.material.GetColor("_BaseColor");
		Color newColor;
        float alpha = aValue;
		float timer = 0;
		while(timer < 1)
		{
            alpha = Mathf.Lerp(oldColor.a, aValue, timer);
			newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
			timer += Time.deltaTime/aTime;
            _renderer.material.SetColor("_BaseColor", newColor);
			yield return null;
		}
		Destroy(gameObject);
	}

   private void OnCollisionEnter(Collision other) {
	   
	   if (other.collider.CompareTag("Player") && _doPlayerDetection) {
			GameUtil.PlayerObject.SetActive(false);
			GameUtil.ActivateGameOver();
	   }
   }

   void OnDrawGizmos()
   {
       Gizmos.DrawWireSphere(_raycastOrigin.position,_sphereCastRadius);
	   Gizmos.color = Color.yellow;
       Gizmos.DrawWireSphere(_targetPos,0.2f);
	   Gizmos.color = Color.green;
	   Gizmos.DrawWireSphere(_point1Position,0.2f);
   }
}
