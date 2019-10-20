using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject _uiPanel;

	void Awake()
	{
		GameUtil.GameOver += OnGameOver;
	}

	private void OnDestroy() {
		GameUtil.GameOver -= OnGameOver;
	}

	public void LoadLevel(int level) {
       SceneManager.LoadScene(level);
	}

	public void OnGameOver() {
		_uiPanel.SetActive(true);
	}
}
