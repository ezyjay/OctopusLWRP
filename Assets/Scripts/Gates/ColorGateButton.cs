using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGateButton : GateButton
{
	public Color _validButtonColor;
	public float _equalColorTolerance = 0.2f;
	public Color _gateOpenColor, _invalidColor;
	public MeshRenderer _buttonRenderer;

	private bool _isColorEqual = false;

	private void Awake()
	{
		_buttonRenderer.material.SetColor("_BaseColor", _validButtonColor);
	}

	private void OnDisable() {
		StopAllCoroutines();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !_gate.IsOpen) {
			_isColorEqual = GameUtil.IsColorEqual(GameUtil.Player._colorDetection.CurrentColor, _validButtonColor, _equalColorTolerance);
			if (!_isColorEqual) 
				StartCoroutine(ChangeColors());
		}
	}

    private void OnTriggerStay(Collider other) {
		
		if (other.CompareTag("Player") && !_gate.IsOpen) {
			
			_isColorEqual = GameUtil.IsColorEqual(GameUtil.Player._colorDetection.CurrentColor, _validButtonColor, _equalColorTolerance);

			if (_enterTriggerTime == 0f && _isColorEqual) 
				_enterTriggerTime = Time.time;
			else if (!_isColorEqual) 
				_enterTriggerTime = 0f;
			
			if (_enterTriggerTime > 0f && Time.time - _enterTriggerTime > _detectionTime) {
				OpenGate();
				_buttonRenderer.material.SetColor("_BaseColor", _gateOpenColor);
			}
		}
		
	}
	
	private IEnumerator ChangeColors() {
		_buttonRenderer.material.SetColor("_BaseColor", _invalidColor);
		yield return new WaitForSeconds(1f);
		_buttonRenderer.material.SetColor("_BaseColor", _validButtonColor);
	}
}
