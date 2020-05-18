using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
	public Transform _spawnPoint;
	public ParticleSystem _saved;

	private SphereCollider _collider;

	private void OnTriggerEnter(Collider other) {
		
		if (other.CompareTag("Player") 
		&& SaveSystem.Instance.SaveData.playerPosition != _spawnPoint.position && _spawnPoint.position.x > SaveSystem.Instance.SaveData.playerPosition.x) {
			SaveSystem.Instance.SavePlayerPosition(_spawnPoint.position);
			SaveSystem.Instance.SaveGame();
			_saved.Play();
		}

	}

	void OnDrawGizmos()
	{
		if (_collider == null) 
			_collider = GetComponent<SphereCollider>();

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, _collider.radius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(_spawnPoint.position, 0.3f);
	}
}
