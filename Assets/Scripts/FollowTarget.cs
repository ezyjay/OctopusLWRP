using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public ObjectTarget _target;
    public Vector2 _offset;
    public float _moveTime;
    public bool _onlyFollowInX;
    public bool _rotate;
    public bool _onlyRotateInX;
    public Rotation _rotation;

    protected Vector2 _direction;

    protected virtual void FixedUpdate()
    {
        Vector3 newPos = Vector2.zero;
        if (_onlyFollowInX)
            newPos = new Vector3(_target.CurrentTarget.x + _offset.x, transform.position.y, transform.position.z);
        else
            newPos = new Vector3(_target.CurrentTarget.x + _offset.x, _target.CurrentTarget.y + _offset.y, transform.position.z);
        
        if (_rotate) {
            if (_onlyRotateInX)
                _rotation.RotateTowardsDirection(new Vector2(transform.position.x - _target.CurrentTarget.x, 0));
            else
                _rotation.RotateTowardsDirection((Vector2)transform.position - _target.CurrentTarget);
        }
        transform.position = Vector3.Lerp(transform.position, newPos, _moveTime * Time.deltaTime);
        _direction = (newPos - transform.position).normalized;
    }
}
