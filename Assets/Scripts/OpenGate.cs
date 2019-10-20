using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
	public Animator _gateAnimator;

	public void Open() {
		_gateAnimator.SetBool("open", true);
	}

	public void Close() {
		_gateAnimator.SetBool("open", false);
	}
}
