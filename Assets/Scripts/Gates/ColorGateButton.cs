using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGateButton : GateButton
{
	public Color _buttonColor;
	public float _equalColorTolerance = 0.2f;

	private bool _isColorEqual = false;

    private void OnTriggerStay(Collider other) {
		
		if (other.CompareTag("Player") && !_gate.IsOpen) {
			
			_isColorEqual = GameUtil.IsColorEqual(GameUtil.Player._colorDetection.CurrentColor, _buttonColor, _equalColorTolerance);

			if (_enterTriggerTime == 0f && _isColorEqual) 
			{
				_enterTriggerTime = Time.time;
				Debug.Log("Start timer");
			}
			else if (!_isColorEqual) {
				_enterTriggerTime = 0f;
				Debug.Log("Restart timer");
			}
			
			if (_enterTriggerTime > 0f && Time.time - _enterTriggerTime > _detectionTime) {
				OpenGate();
				Debug.Log("Opened");
			}
		}
	}
	
}
