using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBetweenPoints : ObjectTarget
{
    public Transform _point1, _point2;
    public float _distanceToChangePoint;
    
    private void Awake() {
        _currentTarget = _point1.position;
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, _currentTarget) <= _distanceToChangePoint)
            ChangeTarget();
    }

    public void ChangeTarget() {
        if (_currentTarget == (Vector2)_point1.position)
            _currentTarget = _point2.position;
        else
            _currentTarget = _point1.position;
    }
}
