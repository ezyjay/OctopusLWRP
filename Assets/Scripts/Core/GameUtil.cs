using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayerType {
    COLOR_OBJECT = 9,
    PLAYER = 10,
    LEVEL = 11,
}

public class GameUtil : MonoBehaviour
{
    private static Player _player;
    public static Player Player { 
        get {			
			if(_player == null)
				_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();		
			
			return _player;
		}
    }

    public static LayerMask GetLayerMask(LayerType layerType) {
        return LayerMask.GetMask(LayerMask.LayerToName((int)layerType));
    }

	public static event Action GameOver;
	public static void ActivateGameOver() {
		GameOver.Invoke();
	}

	public static bool IsColorEqual(Color a, Color b, float tolerance) {
		Vector3 vectorA = new Vector3(a.r, a.g, a.b);
		Vector3 vectorB = new Vector3(b.r, b.g, b.b);

		if (Vector3.Distance(vectorA, vectorB) < tolerance)
			return true;
		
		return false;
	}
}
