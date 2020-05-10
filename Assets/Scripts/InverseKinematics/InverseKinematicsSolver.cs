using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicsSolver : MonoBehaviour
{
	[Header("Debug")]
	public bool _debug;
	public bool _debugDisableAngleClamp;

	[Header("Parameters")]
	public Transform _target;
	public Transform _endEffector;
	public float _learningRate, _samplingDistance, _distanceThreshold;

	[Header("Joints")]
    public Transform _parentBone;
	public List<Joint> _joints = new List<Joint>();
	[ReadOnly]
	public List<float> _angles = new List<float>();
	public Vector2 _minAngle, _maxAngle;
	public List<Vector3> _axis = new List<Vector3>();
	

	private Vector3 _endPoint;
	private bool _endPointReached;

#if UNITY_EDITOR

	[EditorButton]
	public void AddJointScriptToAllBones() {

		if (_parentBone == null) {
			Debug.LogError("Parent bone is null.");
			return;
		}

		foreach(Joint j in _joints) {
			if (j != null && j.GetComponent<Joint>())
				DestroyImmediate(j.GetComponent<Joint>());
		}
		_joints.Clear();

		Transform bone = _parentBone;
		while (bone.childCount > 0) {
			AddJointsToBone(bone);
			
			if (bone.GetChild(0) == null)
				break;
			
			bone = bone.GetChild(0);
		}

		AddJointsToBone(bone);

	}

	[EditorButton]
	public void AutomaticallySetAngles() {
		for(int i = 0; i < _joints.Count; i++) {
			float t = (float)i / (_joints.Count-1);
			_joints[i]._minAngle = Mathf.Lerp(_minAngle.x, _minAngle.y, t);
			_joints[i]._maxAngle = Mathf.Lerp(_maxAngle.x, _maxAngle.y, t);
			_joints[i]._axis = _axis[i % _axis.Count];
		}		
	}

	public void AddJointsToBone(Transform bone) {
		Joint joint = bone.gameObject.AddComponent<Joint>();
		_joints.Add(joint);
	}

#endif

	
	private void Start()
	{
		foreach(Joint j in _joints) 
			_angles.Add(j.GetAngle());
	}
	
	private void Update()
	{
		_endPointReached = false;

		//Inverse kinematics
		InverseKinematics(_target.transform.position);

		//Move arm
		for(int i = 0; i < _joints.Count-1; i++) 
			_joints[i].SetAngle(_angles[i]);

		//Debug
		if (_debug)
			Debug.DrawLine(_endEffector.transform.position, _target.position, Color.green);
	}

	public Vector3 ForwardKinematics (List<float> angles)
	{
		Vector3 prevPoint = _joints[0].transform.position;
		Quaternion rotation = Quaternion.identity;
		for (int i = 1; i < _joints.Count; i++)
		{
			// Rotates around a new axis
			rotation *= Quaternion.AngleAxis(angles[i - 1], _joints[i - 1]._axis);
			Vector3 nextPoint = prevPoint + rotation * _joints[i]._startOffset;
			
			if (_debug)
				Debug.DrawLine(prevPoint, nextPoint, Color.blue);

			prevPoint = nextPoint;
		}
		return prevPoint;
	}

	public float PartialGradient (Vector3 target, List<float> angles, int i)
	{
		// Save angle, it will be restored later
		float angle = angles[i];

		float f_x = DistanceFromTarget(target, angles);

		angles[i] += _samplingDistance;
		float f_x_plus_d = DistanceFromTarget(target, angles);

		// Gradient : [F(x+SamplingDistance) - F(x)] / h
		float gradient = (f_x_plus_d - f_x) / _samplingDistance;

		// Restore angle
		angles[i] = angle;

		return gradient;
	}

	public float DistanceFromTarget(Vector3 target, List<float> angles)
	{
		Vector3 point = ForwardKinematics (angles);
		_endPoint = point;
		return Vector3.Distance(point, target);
	}

	public void InverseKinematics (Vector3 target)
	{
		if (DistanceFromTarget(target, _angles) < _distanceThreshold) {
			_endPointReached = true;
			return;
		}
			
		for (int i = _joints.Count -1; i >= 0; i --)
		{
			// Gradient descent
			float gradient = PartialGradient(target, _angles, i);
			_angles[i] -= _learningRate * gradient;
	
			// Clamp
			if (!_debugDisableAngleClamp)
        		_angles[i] = Mathf.Clamp(_angles[i], _joints[i]._minAngle, _joints[i]._maxAngle);

			// Early termination
			if (DistanceFromTarget(target, _angles) < _distanceThreshold) {
				_endPointReached = true;
				return;
			}
		}
	}

	
	private void OnDrawGizmos()
	{
		if (_debug) {
			if (_endPointReached)
				Gizmos.color = Color.blue;
			else
				Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(_endPoint, 0.2f);
		}
	}
}
