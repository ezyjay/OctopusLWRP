using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAttack : DetectPlayerFollowTarget
{
	[Header("Shark")]
	public Animator _sharkAnimator;

   private PlayerHiddenState _playerHiddenState;
   private MeshRenderer _renderer;

   private Vector2 _targetPos = Vector2.zero;
   private Vector2 _savedTarget, _point1Position;
   private WanderBetweenPoints _wanderPoints;
   private ParticleSystem _deathFX;

   protected override void Awake() {
	   base.Awake();
	   _deathFX = transform.parent.GetComponentInChildren<ParticleSystem>();
	   _playerHiddenState = GameUtil.Player._hiddenState;
	   _renderer = GetComponent<MeshRenderer>();
	   _wanderPoints = _target as WanderBetweenPoints;
   }

	protected override  void OnPlayerJustDetected() {
		_savedTarget = _raycastHit.point;
		_point1Position = transform.position;
	}
	
	protected override bool PlayerDetectable() {
		return !_playerHiddenState.IsHidden();
	}

    protected override void PlayerDetectedBehaviour() {

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
			if (!_sharkAnimator.GetBool("Attack"))
				_sharkAnimator.SetBool("Attack", true);

			transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speedWhenDetected * Time.deltaTime);
			
			//Go back to normal behaviour after attack
			if (Vector2.Distance(transform.position, _targetPos) < 1.5f ) {

				if (Vector2.Distance(_point1Position, _targetPos) > 10f) {
					
					//If target pos is past an obstacle
					RaycastHit hitInfo;
					if  (Physics.Linecast(_raycastOrigin.position, _point1Position, out hitInfo, GameUtil.GetLayerMask(LayerType.LEVEL)))
						_point1Position = hitInfo.point;

					//Set next wander points
					// _wanderPoints._point1.position = _point1Position;
					// _wanderPoints._point2.position = _targetPos;
				}
				_playerDetected = false;
				_targetPos = Vector2.zero;
				_sharkAnimator.SetBool("Attack", false);
			}
		}
   }

   public void FadeShark() {
	   _deathFX.transform.position = gameObject.transform.position;
	   _deathFX.Play();
	   StartCoroutine(FadeAndDisable(gameObject, _renderer.material, 0f, 0.2f, true));
   }

	private IEnumerator FadeAndDisable(GameObject go, Material material, float aValue, float aTime, bool destroy = false) {
		Color oldColor = material.GetColor("_BaseColor");
		Color newColor;
        float alpha = aValue;
		float timer = 0;
		while(timer < 1)
		{
            alpha = Mathf.Lerp(oldColor.a, aValue, timer);
			newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
			timer += Time.deltaTime/aTime;
            material.SetColor("_BaseColor", newColor);
			yield return null;
		}
		go.SetActive(false);
		if (destroy)
			Destroy(go);
	}

   private void OnCollisionEnter(Collision other) {
	   
	   if (other.collider.CompareTag("Player") && _doPlayerDetection && !GameUtil.Player._isInvinsible) {
		   StartCoroutine(KillPlayer());
	   }
   }

   private IEnumerator KillPlayer() {
	    GameUtil.Player._playerFX._deathFX.transform.position = GameUtil.Player.transform.position;
		GameUtil.Player._playerFX._deathFX.Play();
		GameUtil.Player.gameObject.SetActive(false);
		yield return new WaitForSeconds(1.5f);
		GameUtil.ActivateGameOver();
		
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
