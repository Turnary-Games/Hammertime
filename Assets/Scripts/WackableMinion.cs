using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Minion))]
public class WackableMinion : Wackable {

	private Minion minion;
	
	void Start() {
		minion = GetComponent<Minion> ();
	}

	public override void Wack(int damage = 1) {
		dead = minion.Damage (damage) ? true : dead;
	}
}
