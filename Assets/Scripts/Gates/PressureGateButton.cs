using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureGateButton : GateButton
{
	public Transform _button;

    private void OnTriggerStay(Collider other) {
		
		if (!_gate.IsOpen && _button.localPosition.y < 0) {
			
			if (_enterTriggerTime == 0f) 
			{
				_enterTriggerTime = Time.time;
			}

			if (_enterTriggerTime > 0f && Time.time - _enterTriggerTime > _detectionTime) {
				OpenGate();
			}
		}
	}

	private void Update()
	{
		if (_button.localPosition.y > 0 && _gate.IsOpen) {
			CloseGate();
		}
	}
}
