using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
    public Transform _raycastOrigin;
    public float _colorLerpTime = 2f;
    public float _timeToStayNewColorOnLeave = 3f;

	private MeshRenderer _renderer;
    private Color _originalColor;
    private Color _oldColor, _newColor;
    private RaycastHit _raycastHit;
	private float _timer = 0;
    private bool _doColorLerp, _doInverseColorLerp;
    private float _timeChangedColor = 0f;
    private bool _isOverColorObject;
	private bool _hasAssimilatedColor;

	public bool IsHidden() {
		return _isOverColorObject && _hasAssimilatedColor;
	}

    private void Awake() {
		_renderer = GetComponent<MeshRenderer>();
        _originalColor = _renderer.material.GetColor("_BaseColor");
    }

    private void OnDisable() {
        _renderer.material.SetColor("_BaseColor", _originalColor);
    }

    void Update()
    {
        //Are we over a color giving object ?
		Debug.DrawRay(_raycastOrigin.position, Vector3.forward * 500);
        if (Physics.SphereCast(_raycastOrigin.position, 0.4f, Vector3.forward * 500, out _raycastHit, 500, GameUtil.GetLayerMask(LayerType.COLOR_OBJECT)))
            _isOverColorObject = true;
        else
            _isOverColorObject = false;

        //Have we clicked mouse button
        if (Input.GetMouseButtonDown(0) && _isOverColorObject) 
        {
            Renderer renderer = _raycastHit.collider.GetComponent<MeshRenderer>();
            _newColor = renderer.material.GetColor("_BaseColor");
            _doColorLerp = true;
            _timeChangedColor = Time.time;
			_hasAssimilatedColor = true;
			_oldColor = _renderer.material.color;
        } 
        //Have we stopped clicking or left the color object
        else if (Input.GetMouseButtonUp(0) || !_isOverColorObject && Time.time - _timeChangedColor > _timeToStayNewColorOnLeave) 
        {
			_doColorLerp = true;
			_hasAssimilatedColor = false;
			_oldColor = _newColor;
            _newColor = _originalColor;
        }

        //Do lerp 
        if (_doColorLerp) {
			Color color = Color.Lerp(_oldColor, _newColor, _timer);
			if (_timer < 1) _timer += Time.deltaTime/_colorLerpTime;
            _renderer.material.SetColor("_BaseColor",color);

			if (_timer >= 1) {
				_doColorLerp = false;
				_timer = 0;
			}
		}
    }
    

	private void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(_raycastOrigin.position, 0.4f);
	}
}
