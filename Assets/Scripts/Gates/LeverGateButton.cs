using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGateButton : GateButton
{
	[Header("Lever specific")]
	public LeverPosition _leverPosition;
	public float _minDistanceForValidation = 1f;

	private bool IsLeverActivated() {
		return Vector3.Distance(_leverPosition._leverHandle.position, _leverPosition._rightSide.position) <= _minDistanceForValidation;
	}

	private void Update() {
		if (_gate.IsOpen && !IsLeverActivated())
			CloseGate();
	}

    private void OnTriggerStay(Collider other) {
		
		if (!_gate.IsOpen && IsLeverActivated()) {
			
			if (_enterTriggerTime == 0f) 
			{
				_enterTriggerTime = Time.time;
			}

			if (_enterTriggerTime > 0f && Time.time - _enterTriggerTime > _detectionTime) {
				OpenGate();
			}
		}
	}
}
