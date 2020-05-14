using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFX : MonoBehaviour
{
	public ParticleSystem _bubbleBurst, _bubbleIdle, _bubblesMoving;
	public UnityEvent PlayerStartedMoving, PlayerStoppedMoving;

	private void OnEnable()
	{
		GameUtil.Player._controller.PlayerMoving += OnPlayerMoving;
	}

	private void OnDisable() {
		GameUtil.Player._controller.PlayerMoving -= OnPlayerMoving;
	}

	public void OnPlayerMoving(bool isMoving) {
		if (isMoving) {

			_bubbleBurst.Play();
			_bubblesMoving.Play();
			_bubbleIdle.Stop();
			PlayerStartedMoving.Invoke();
		}
		else {

			_bubbleBurst.Stop();
			_bubblesMoving.Stop();
			_bubbleIdle.Play();
			PlayerStoppedMoving.Invoke();
		}
	}
	
}
