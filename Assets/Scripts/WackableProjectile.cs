using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Projectile))]
public class WackableProjectile : Wackable {

	private Projectile projectile;
	
	public override void Init() {
		projectile = GetComponent<Projectile> ();
	}

	public override void Wack(int damage = 1) {
		if (projectile.Damage (damage))
			GameController.Get ().SpawnCoins (transform.position, projectile.reward);
	}

}
