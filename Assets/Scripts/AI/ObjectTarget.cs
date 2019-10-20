using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTarget : MonoBehaviour
{
    protected Vector2 _currentTarget;
    public Vector2 CurrentTarget { get => _currentTarget; set => _currentTarget = value; }

    void FixedUpdate()
    {
        _currentTarget = transform.position;
    }
}
