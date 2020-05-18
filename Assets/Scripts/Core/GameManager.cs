using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject _uiPanel;
	public Vector3 _playerStartPosition;

	public bool _loadSave = true;

	[EditorButton]
	public void ResetSaveData() {
		SaveSystem.Instance.SavePlayerPosition(_playerStartPosition);
		SaveSystem.Instance.SaveGame();
	}

	private void Awake()
	{
		if (_loadSave)
			SaveSystem.Instance.LoadGame();
			
		GameUtil.GameOver += OnGameOver;
		RenderSettings.fog = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!_uiPanel.activeSelf) {
				_uiPanel.SetActive(true);
				Time.timeScale = 0;
			}
			else {
				_uiPanel.SetActive(false);
				Time.timeScale = 1;
			}
		}
	}

	public void ExitGame() {
		Application.Quit();
	}

	private void OnDestroy() {
		GameUtil.GameOver -= OnGameOver;
	}

	public void LoadLevel(int level) {
		_uiPanel.SetActive(false);
		Time.timeScale = 1;
       SceneManager.LoadScene(level);
	}

	public void OnGameOver() {
		_uiPanel.SetActive(true);
	}
}
