using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlayer : MonoBehaviour
{
   	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameUtil.Player._hiddenState.IsHiddenByEnvironment = true;
		}
   	}

   	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			GameUtil.Player._hiddenState.IsHiddenByEnvironment = false;
		}
   	}
}
