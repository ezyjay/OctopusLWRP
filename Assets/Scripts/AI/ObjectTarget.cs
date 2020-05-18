using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTarget : MonoBehaviour
{
    protected Vector3 _currentTarget;
    public Vector3 CurrentTarget { get => _currentTarget; set => _currentTarget = value; }

	void Start()
	{
        _currentTarget = transform.position;
	}

    void FixedUpdate()
    {
        _currentTarget = transform.position;
    }
}
