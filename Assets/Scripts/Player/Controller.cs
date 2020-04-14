using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour 
{
    public Rigidbody _rb;
    public Rotation _playerRotation;
    public float _deccelerationModifier;
    public float _speed;
    public float _maxSpeed;

    private Vector2 _inputDirection;
    private Vector2 _previousInputDirection = Vector2.zero;
    private float _inputDirectionToStartMoving = 0.4f;

    public Vector2 InputDirection { get => _inputDirection; set => _inputDirection = value; }

    void Update()
    {
        _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate() {
        
        if (_inputDirection != Vector2.zero) { 

			if (Mathf.Abs(_inputDirection.y) > 0.2f && Mathf.Abs(_inputDirection.x) > 0.2f) {
				if(_rb.velocity.y == 0)
					_inputDirection = new Vector2(_inputDirection.x, 0);
				else if (_rb.velocity.x == 0)
					_inputDirection = new Vector2(0, _inputDirection.y);
			}

            _playerRotation.RotateTowardsDirection(_inputDirection);

            bool startMoving = Mathf.Abs(_inputDirection.x) > _inputDirectionToStartMoving || Mathf.Abs(_inputDirection.y) > _inputDirectionToStartMoving; 
            if (_rb.velocity.magnitude <= _maxSpeed && startMoving) 
                _rb.velocity = _inputDirection.normalized * _speed;

            _previousInputDirection = _inputDirection;
        } 
        else {

            _playerRotation.RotateBack(_previousInputDirection.normalized);

            if (_rb.velocity.magnitude > 0) {
                if (_rb.velocity.magnitude >= 0.4f * _maxSpeed)
                    _rb.velocity = _deccelerationModifier * _rb.velocity;
                else
                    _rb.velocity = 0.1f * _rb.velocity;
            }
                
        }
    }
}
