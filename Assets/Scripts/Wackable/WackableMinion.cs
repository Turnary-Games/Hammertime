using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Minion))]
public class WackableMinion : Wackable {

	private Minion minion;
	
	public override void Init() {
		minion = GetComponent<Minion> ();
	}

	public override void Wack(int damage = 1) {
		minion.Damage (damage);
	}
}
