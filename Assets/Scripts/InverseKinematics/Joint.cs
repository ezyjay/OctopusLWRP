using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
   	public Vector3 _axis;
    public Vector3 _startOffset;

	public float _minAngle;
    public float _maxAngle;
 
    private void Awake ()
    {
        _startOffset = transform.localPosition;
    }

	public float GetAngle() {
		if (_axis.x != 0f)
			return transform.localEulerAngles.x;
		else if(_axis.y != 0f)
			return transform.localEulerAngles.y;
		else if(_axis.z != 0f)
			return transform.localEulerAngles.z;
		return 0;
	}

	public void SetAngle(float angle) {
		if (_axis.x != 0f) 
			transform.localEulerAngles = new Vector3(angle, 0, 0);
		else if (_axis.y != 0) 
			transform.localEulerAngles = new Vector3(0, angle, 0);
		else if (_axis.z != 0) 
			transform.localEulerAngles = new Vector3(0, 0, angle);
	}
}
