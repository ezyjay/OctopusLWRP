using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float _rotationSpeed = 1f;
    
    public void RotateTowardsDirection(Vector2 dir) {

        Vector3 rotation = AngleLerp(transform.rotation.eulerAngles, GetRotation(dir), _rotationSpeed * Time.deltaTime);
        Quaternion quat = Quaternion.Euler(0, rotation.y, rotation.z);
        transform.rotation = quat;
    }

    public void RotateBack(Vector2 previousInputDir) {

        float yRotation = 0;
        if (previousInputDir.x > 0.1f) yRotation = 0f;
        else if (previousInputDir.x < -0.1f) yRotation = 180f;
        else if (transform.rotation.eulerAngles.y > 90f) yRotation = 180f;
        else yRotation = 0;

        Vector3 rotation = AngleLerp(transform.rotation.eulerAngles, new Vector3(0, yRotation, 0), _rotationSpeed * Time.deltaTime);
        Quaternion quat = Quaternion.Euler(0, rotation.y, rotation.z);
        transform.rotation = quat;
    }

    public static Vector3 AngleLerp(Vector3 startAngle, Vector3 finishAngle, float t) {

        float xLerp = Mathf.LerpAngle(startAngle.x, finishAngle.x, t);
        float yLerp = Mathf.LerpAngle(startAngle.y, finishAngle.y, t);
        float zLerp = Mathf.LerpAngle(startAngle.z, finishAngle.z, t);
        return new Vector3(xLerp, yLerp, zLerp); ;
    }

    public Vector3 GetRotation(Vector2 dir) {

        Vector2 normDir = dir.normalized;
        float yAngle = 0f;
        float zAngle = 0f;

        if (normDir.x > 0.1f) {
            yAngle = 0f;
		} else if (normDir.x < -0.1f) {
            yAngle = 180f;
        } else {
			
			if(normDir.y > 0.1f)
				yAngle = 270;
			else if (normDir.y < -0.1f)
				yAngle = 90;
			else
            	yAngle = transform.rotation.eulerAngles.y > 90 ? 180 : 0;
		}

        zAngle = normDir.y * 90;

        return new Vector3(0, yAngle, zAngle);
    }
}
