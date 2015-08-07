using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Minion),true)]
public class _Minion : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();
	}
	
}
