using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayer : MonoBehaviour
{
	private PlayerHiddenState _playerHiddenState;

	private void Awake()
	{
		_playerHiddenState = GameUtil.PlayerObject.GetComponent<PlayerHiddenState>();
	}

   	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			_playerHiddenState.IsHiddenByEnvironment = true;
		}
   	}

   	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			_playerHiddenState.IsHiddenByEnvironment = false;
		}
   	}
}
