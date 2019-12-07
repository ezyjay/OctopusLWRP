using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
	public Animator _gateAnimator;
	public bool _openTowardsRight = true;
	public bool _openTowardsTop = false;
	public bool _isSolid = true;

	private bool _collidingWithPlayer, _collidingWithBuoy;
	private Collider _collider;
	private Controller _playerController;

	private void Awake()
	{
		_collider = gameObject.GetComponent<Collider>();
		_playerController = GameUtil.PlayerObject.GetComponent<Controller>();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Shark")) {

			//If solid gate, shark dies
			if(_isSolid) {
				Open();
				SharkAttack sharkAttack = other.gameObject.GetComponentInChildren<SharkAttack>();
				sharkAttack.DoPlayerDetection = false;
				sharkAttack.FadeShark();
				other.gameObject.GetComponent<Rigidbody>().useGravity = true;
			}

			//If wooden gate, gate breaks, shark continues
			else OpenWoodGate();
			
		}

		if (other.CompareTag("Player"))
			_collidingWithPlayer = true;
		if (other.CompareTag("Buoy"))
			_collidingWithBuoy = true;
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player"))
			_collidingWithPlayer = false;
		if (other.CompareTag("Buoy"))
			_collidingWithBuoy = false;
	}

	private void OnTriggerStay(Collider other) {
		if ((other.CompareTag("Player") || other.CompareTag("Buoy")) && _collidingWithPlayer && _collidingWithBuoy) {
			if (_playerController.InputDirection.y > 0.1f && _playerController._rb.velocity.y > 2f) {
				if (_isSolid)
					Open();
				else
					OpenWoodGate(true, Vector2.up, 2);
			}
		}
	}

	private IEnumerator FadeWood() {
		yield return new WaitForSeconds(4f);
		foreach (Transform t in transform) {
			t.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		}
	}

	public void OpenWoodGate(bool addForce = false, Vector2 directionOfForce = default(Vector2), float forcePower = 0f) {
		_collider.enabled = false;
		foreach (Transform t in transform) {
			Rigidbody rb = t.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			if (addForce)
				rb.AddForce(directionOfForce * forcePower,ForceMode.Impulse);
		}
		StartCoroutine(FadeWood());
	}

	public void Open() {
		_collider.enabled = false;
		if (_openTowardsRight)
			_gateAnimator.SetBool("open", true);
		else if (_openTowardsTop)
			_gateAnimator.SetBool("openUp", true);
		else
			_gateAnimator.SetBool("openInverse", true);
	}

	public void Close() {
		if (_openTowardsRight)
			_gateAnimator.SetBool("open", false);
		else if (_openTowardsTop)
			_gateAnimator.SetBool("openUp", false);
		else
			_gateAnimator.SetBool("openInverse", false);
	}

}
