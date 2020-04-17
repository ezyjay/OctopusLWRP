using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureGateButton : GateButton
{
    private void OnTriggerStay(Collider other) {
		
		if (other.CompareTag("Boulder") && !_gate.IsOpen) {
			
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
