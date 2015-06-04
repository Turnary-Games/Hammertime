using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {
	
	public int health = 1;
	private bool dead;

	// returns Boolean: true=died, false=survived
	public bool Damage(int amount = 1) {
		health -= amount;
		HealthChange ();
		return health <= 0;
	}

	void HealthChange() {
		if (health <= 0 && !dead) {
			Kill();
		}
	}

	void Kill() {
		dead = true;
		Destroy(gameObject);
	}

}
