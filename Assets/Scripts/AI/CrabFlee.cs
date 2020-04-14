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
		direction = new Vector3(direction.x + Mathf.Sign(direction.x)*4, direction.y, direction.z);
		transform.position = Vector3.MoveTowards(transform.position, direction, _speedWhenDetected * Time.deltaTime);
	
		_wanderPoints._point1.position = transform.position;
		_wanderPoints.CurrentTarget = _wanderPoints._point1.position;
		_wanderPoints._point2.position = transform.position + new Vector3(4 * direction.normalized.x, 0, 0); 
		
	}

}
