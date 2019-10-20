using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttack : FollowTarget
{
   public float _detectionDistance;
   public float _timePlayerInRayBeforeAttack;
   public float _attackSpeed;
   public Transform _raycastOrigin;

   private RaycastHit _raycastHit;
   private float _timePlayerDetected;
   private bool _playerDetected;
   private ColorDetection _playerColor;

   private void Awake() {
	   _playerColor = GameUtil.PlayerObject.GetComponent<ColorDetection>();
   }

   protected override void FixedUpdate()
   {
		Debug.DrawRay(_raycastOrigin.position, _direction * _detectionDistance, Color.red);
		if (!_playerDetected && Physics.SphereCast(_raycastOrigin.position, 1.3f, _direction, out _raycastHit, _detectionDistance, GameUtil.GetLayerMask(LayerType.PLAYER))) {
			_timePlayerDetected = Time.time;
			if (!_playerColor.IsHidden())
				_playerDetected = true;
		} 

		if (_playerDetected) 
		{

			if( _timePlayerDetected != 0 && Time.time - _timePlayerDetected >= _timePlayerInRayBeforeAttack)
			 	_rotation.RotateTowardsDirection(transform.position - _raycastHit.point);
				transform.position = Vector3.Lerp(transform.position, _raycastHit.point, _attackSpeed * Time.deltaTime);
				
			if (Vector2.Distance(transform.position, _raycastHit.point) < 2f) {
				_playerDetected = false;
		}
		} else {
			base.FixedUpdate();
		}

		
   }

   private void OnCollisionEnter(Collision other) {
	   
	   if (other.collider.CompareTag("Player")) {
			GameUtil.PlayerObject.SetActive(false);
			GameUtil.ActivateGameOver();
	   }
   }

   void OnDrawGizmos()
   {
       Gizmos.DrawWireSphere(_raycastOrigin.position, 1.3f);
   }
}
