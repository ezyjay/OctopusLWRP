using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabFlee : DetectPlayerFollowTarget {

	private WanderBetweenPoints _wanderPoints; 
   
	protected override void Awake() {
		base.Awake();
		_wanderPoints = _target as WanderBetweenPoints;
	}

	//Flee when detected
	protected override void PlayerDetectedBehaviour() {

		Vector3 direction = transform.position - GameUtil.Player.gameObject.transform.position;
		Vector3 target = new Vector3(transform.position.x + Mathf.Sign(direction.x) * 8, transform.position.y, transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, target, _speedWhenDetected * Time.deltaTime);
	
		_wanderPoints._point1.position = transform.position;
		_wanderPoints.CurrentTarget = _wanderPoints._point1.position;
		_wanderPoints._point2.position = transform.position + new Vector3(4 * Mathf.Sign(direction.x), 0, 0); 

		RaycastHit hitInfo;
		if  (Physics.Linecast(_raycastOrigin.position, _wanderPoints._point2.position, out hitInfo, GameUtil.GetLayerMask(LayerType.LEVEL)))
			_wanderPoints._point2.position = hitInfo.point;

	}
}
