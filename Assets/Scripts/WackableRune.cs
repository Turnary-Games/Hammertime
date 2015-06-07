using UnityEngine;
using System.Collections;

public class WackableRune : Wackable {

	private RuneController rune;
	
	void Start() {
		rune = GetComponent<RuneController> ();
	}
	
	public override void Wack(int damage = 1) {
		rune.SpawnMinion ();
	}

}
