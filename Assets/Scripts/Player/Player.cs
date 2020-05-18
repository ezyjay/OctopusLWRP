using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Controller _controller;
	public PlayerHiddenState _hiddenState;
	public ColorDetection _colorDetection;
	public PlayerFX _playerFX;

	public bool _isInvinsible = false;
}
