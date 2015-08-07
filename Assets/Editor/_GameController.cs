using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameController))]
public class _GameController : Editor {

	bool foldout = true;

	int mask = -1; // 0 = Nothing, -1 = Everything
	string[] options = new string[] {
		"Minions (+ towers)",
		"Projectiles",
		"Coins",
		"Boss",
		"Waves",
		"Hammer",
		"Rotators",
	};

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();

		// Get the controller
		GameController script = target as GameController;

		GUILayout.Space (12);

		foldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), foldout, "Extra utility", true);
		if (foldout) Extras (script);


	}

	void Extras(GameController script) {
		GUI.enabled = Application.isPlaying;
		if (GUILayout.Button ("Gameover for the player")) script.Damage(Side.enemy, script.playerHealth);
		if (GUILayout.Button ("Gameover for the evil tree")) script.Damage(Side.ally, script.enemyHealth);
		
		GUILayout.Space (12);
		
		if (GUILayout.Button ("Pause"))	CustomPause ();
		if (GUILayout.Button ("Unpause")) CustomUnpause ();

		mask = EditorGUILayout.MaskField ("Pause mask", mask, options);
		GUI.enabled = true;

		GUILayout.Space (12);

		if (!Application.isPlaying)
			EditorGUILayout.HelpBox ("These functions are only enabled in play mode!", MessageType.Warning);
	}

	void CustomPause() {
		if (mask == -1)
			Pausable.PauseAll ();
		if ((1 & mask) == 1)
			Pausable.PauseAll<Minion> ();
		if ((2 & mask) == 2)
			Pausable.PauseAll<Projectile> ();
		if ((4 & mask) == 4)
			Pausable.PauseAll<CoinController> ();
		if ((8 & mask) == 8)
			Pausable.PauseAll<BossController> ();
		if ((16 & mask) == 16)
			Pausable.PauseAll<GameController> ();
		if ((32 & mask) == 32)
			Pausable.PauseAll<HammerController> ();
		if ((64 & mask) == 64)
			Pausable.PauseAll<Rotate> ();
	}

	void CustomUnpause() {
		if (mask == -1)
			Pausable.UnpauseAll ();
		if ((1 & mask) == 1)
			Pausable.UnpauseAll<Minion> ();
		if ((2 & mask) == 2)
			Pausable.UnpauseAll<Projectile> ();
		if ((4 & mask) == 4)
			Pausable.UnpauseAll<CoinController> ();
		if ((8 & mask) == 8)
			Pausable.UnpauseAll<BossController> ();
		if ((16 & mask) == 16)
			Pausable.UnpauseAll<GameController> ();
		if ((32 & mask) == 32)
			Pausable.UnpauseAll<HammerController> ();
		if ((64 & mask) == 64)
			Pausable.UnpauseAll<Rotate> ();
	}

}
