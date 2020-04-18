using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    public OpenGate _gate;
	public float _detectionTime = 1f;

	protected float _enterTriggerTime = 0f;

	public void OpenGate() {
		_gate.Open();
	}

	public void CloseGate() {
		_gate.Close();
	}
}
