using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
	private static readonly SaveSystem _instance = new SaveSystem();
    public static SaveSystem Instance => _instance;

    private SaveData _saveData = new SaveData();
    public SaveData SaveData { get => _saveData; }

    public void SavePlayerPosition(Vector3 newPosition) {
		if (_saveData == null)
			_saveData = new SaveData();
		_saveData.playerPosition = newPosition;
	}

	public void ClearSave() {
		_saveData = new SaveData();
		SaveGame();
	}

	public void SaveGame() {
		
		if (_saveData == null)
			_saveData = new SaveData();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
		bf.Serialize(file, _saveData);
		file.Close();

		Debug.Log("Game Saved");
	}

	public void LoadGame() {
		
		if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
			
			//Load file
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			_saveData = (SaveData)bf.Deserialize(file);
			file.Close();

			//Set data
			GameUtil.Player.transform.position = _saveData.playerPosition;
			
			Debug.Log("Game Loaded");
		}
		else
		{
			Debug.Log("No game saved!");
		}
	}
}
