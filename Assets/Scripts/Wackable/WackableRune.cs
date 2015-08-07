using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RuneController))]
public class WackableRune : Wackable {

	private RuneController rune;
	
	public override void Init() {
		rune = GetComponent<RuneController> ();
	}
	
	public override void Wack(int damage = 1) {
		rune.Purchase ();
	}

}
