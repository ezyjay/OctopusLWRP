using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	[Header("Follow target")]
    public ObjectTarget _target;
    public Vector2 _offset;
    public float _moveTime;
    public bool _onlyFollowInX;
    public bool _rotate;
    public bool _onlyRotateInX;
    public Rotation _rotation;
	public bool _lerp = true;
	public bool _followInZ = false;
	public float _zOffset;
	public bool _setPositionStart = false;

    protected Vector2 _direction;

	private void Start() 
	{
		if (_setPositionStart) {
			if (_onlyFollowInX)
				transform.position = new Vector3(_target.CurrentTarget.x, transform.position.y, transform.position.z);
			else if (_followInZ)
				transform.position = new Vector3(_target.CurrentTarget.x, _target.CurrentTarget.y, _target.CurrentTarget.z);
			else
				transform.position = new Vector3(_target.CurrentTarget.x, _target.CurrentTarget.y, transform.position.z);
		}
	}

    protected virtual void FixedUpdate()
    {
        Vector3 newPos = Vector2.zero;
        if (_onlyFollowInX)
            newPos = new Vector3(_target.CurrentTarget.x + _offset.x, transform.position.y, transform.position.z);
        else if (_followInZ)
			newPos = new Vector3(_target.CurrentTarget.x + _offset.x, _target.CurrentTarget.y + _offset.y, _target.CurrentTarget.z + _zOffset);
		else
            newPos = new Vector3(_target.CurrentTarget.x + _offset.x, _target.CurrentTarget.y + _offset.y, transform.position.z);
        
        if (_rotate) {
            if (_onlyRotateInX)
                _rotation.RotateTowardsDirection(new Vector2(transform.position.x - _target.CurrentTarget.x, 0));
            else
                _rotation.RotateTowardsDirection(transform.position - _target.CurrentTarget);
        }
		if(_lerp)transform.position = Vector3.Lerp(transform.position, newPos, _moveTime * Time.deltaTime);
		else transform.position = Vector3.MoveTowards(transform.position, newPos, _moveTime * Time.deltaTime);
        _direction = (newPos - transform.position).normalized;
    }
}
