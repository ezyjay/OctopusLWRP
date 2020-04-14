using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    public OpenGate _gate;

	public void OpenGate() {
		_gate.Open();
	}
}
