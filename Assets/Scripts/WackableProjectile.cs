using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Projectile))]
public class WackableProjectile : Wackable {

	private Projectile projectile;
	
	void Start() {
		projectile = GetComponent<Projectile> ();
	}

	public override void Wack(int damage = 1) {
		dead = projectile.Damage (damage) ? true : dead;

		if (dead)
			GameController.Get ().SpawnCoins (transform.position, projectile.reward);
	}

}
