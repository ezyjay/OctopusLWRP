using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public GameObject _normalBoulder, _crackedBoulder;

	private MeshRenderer _normalBoulderRenderer;
	private Collider _normalBoulderCollider;

	private void Awake()
	{
		_normalBoulderRenderer = _normalBoulder.GetComponent<MeshRenderer>();
		_normalBoulderCollider = _normalBoulder.GetComponent<Collider>();
	}

	public void BreakBolder() {
		_normalBoulderRenderer.enabled = false;
		_normalBoulderCollider.enabled = false;
		_crackedBoulder.SetActive(true);
		StartCoroutine(FadeBoulder());
	}

	private IEnumerator FadeBoulder() {
		yield return new WaitForSeconds(4f);
		foreach (Transform t in _crackedBoulder.transform) {
			t.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.2f);
		}
		Destroy(gameObject);
	}

	private void OnDestroy() 
	{
		StopAllCoroutines();
	}
}
