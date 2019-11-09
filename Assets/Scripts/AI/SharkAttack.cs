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
   public GameObject _exclamation;
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
	   _exclamation.SetActive(false);
   }

   protected override void FixedUpdate()
   {
		if (_doPlayerDetection) {

			//Detect player
			if (!_playerDetected) {

				if (GameUtil.PlayerObject)
					_playerToSharkDirection = GameUtil.PlayerObject.transform.position - transform.position;
				
				//If player is in field of view
				if (Vector3.Angle(_direction, _playerToSharkDirection) < fieldOfView) {
					
					//Ray cast to see if not behind an obstacle
					Debug.DrawRay(_raycastOrigin.position, _playerToSharkDirection.normalized * _detectionDistance, Color.red);
					Physics.SphereCast(_raycastOrigin.position, _sphereCastRadius, _playerToSharkDirection, out _raycastHit, _detectionDistance);
					if(_raycastHit.collider != null && _raycastHit.collider.CompareTag("Player")) {
						if (_timePlayerDetected == 0f) {
							_savedTarget = _raycastHit.point;
							_timePlayerDetected = Time.time;
							_exclamation.SetActive(true);
						}
						if (!_playerColor.IsHidden() && _timePlayerDetected != 0 && Time.time - _timePlayerDetected >= _timePlayerInRayBeforeAttack) {
							_playerDetected = true;
							_exclamation.SetActive(false);
							_point1Position = transform.position;
						}
					} else {
						_timePlayerDetected = 0f;
						_exclamation.SetActive(false);
					}
				} else {
					_timePlayerDetected = 0f;
					_exclamation.SetActive(false);
				}

				
			} 

			//Attack if player detected
			if (_playerDetected) 
			{
				Vector2 direction = (Vector2)transform.position - _savedTarget;
				_rotation.RotateTowardsDirection(direction);

				//Calculate target slighlty further than detected player
				if (_targetPos == Vector2.zero)
					_targetPos = (Vector2)_savedTarget + direction.normalized * -5;

				//Move towards player
				else {
					transform.position = Vector3.MoveTowards(transform.position, _targetPos, _attackSpeed * Time.deltaTime);
					
					//Go back to normal behaviour after attack
					if (Vector2.Distance(transform.position, _targetPos) < 1.5f ) {

						if (Vector2.Distance(_point1Position, transform.position) > 10f) {
							//TO DO: Test if point is not behind an obstacle
							_wanderPoints._point1.position = _point1Position;
							_wanderPoints._point2.position = transform.position;
						}
						_playerDetected = false;
						_targetPos = Vector2.zero;
					}
				}

			//If player not detected, do follow target from base class	
			} else {
				base.FixedUpdate();
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
       Gizmos.DrawWireSphere(_targetPos,1f);
   }
}
