using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiddenState : MonoBehaviour
{
	private bool  _isHiddenByEnvironment;
   	private ColorDetection _colorDetection;

	public bool IsHiddenByEnvironment { get => _isHiddenByEnvironment; set => _isHiddenByEnvironment = value; }

    private void Awake()
	{
		_colorDetection = GameUtil.Player._colorDetection;
	}

    public bool IsHidden() {
		return (_colorDetection.IsOverColorObject || _colorDetection.IsOverUseColorObject) && _colorDetection.HasAssimilatedColor || _isHiddenByEnvironment;
	}
}
