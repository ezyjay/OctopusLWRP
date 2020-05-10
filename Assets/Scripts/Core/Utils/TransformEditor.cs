using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// [@CustomEditor(typeof(Transform))]
// class TransformEditor : Editor {
// 	public override void OnInspectorGUI() {

// 		var transform = target as Transform;

// 		if (!EditorGUIUtility.wideMode)
// 		{
// 			EditorGUIUtility.wideMode = true;
// 			EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
// 		}		

// 		EditorGUILayout.BeginHorizontal ();
// 		transform.localPosition = EditorGUILayout.Vector3Field("Position", transform.localPosition);
// 		EditorGUILayout.EndHorizontal ();
		
// 		EditorGUILayout.BeginHorizontal ();
// 		EditorGUI.BeginDisabledGroup(true);
// 		transform.position = EditorGUILayout.Vector3Field("Global", transform.position);
// 		EditorGUILayout.EndHorizontal ();
// 		EditorGUI.EndDisabledGroup();

// 		EditorGUILayout.BeginHorizontal ();
// 		transform.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", transform.localEulerAngles);
// 		EditorGUILayout.EndHorizontal ();

// 		EditorGUILayout.BeginHorizontal ();
// 		transform.localScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);
// 		EditorGUILayout.EndHorizontal ();

// 	}
// }
