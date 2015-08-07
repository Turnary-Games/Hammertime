using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameController))]
public class _GameController : Editor {
	
	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		GUILayout.Space (12);

		if (GUILayout.Button ("Pause All")) {
			// Pause
			foreach (Pausable obj in FindObjectsOfType<Pausable>()) {
				obj.paused = true;
			}

			Debug.Log("Paused all Pauseable objects");
		}

		if (GUILayout.Button ("Unpause All")) {
			// Unpause
			foreach (Pausable obj in FindObjectsOfType<Pausable>()) {
				obj.paused = false;
			}

			Debug.Log("Unpaused all Pauseable objects");
		}
	}
	
}
