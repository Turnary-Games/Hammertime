using UnityEngine;
using System.Collections;

public class Living : Pausable
{
	[Header("Living settings")]
	
	[Tooltip("Hitpoints, can take /health/ damage before dying")]
	public int health;
	[HideInInspector]
	public bool dead;

	#region Damage and health (Damage, HealthChange, Kill)
	// returns Boolean: true=died, false=survived
	public virtual bool Damage(int amount = 1) {

		GameController.Get().AddDamageIndicator(transform.position, amount);

		health -= amount;
		HealthChange ();
		return health <= 0;
	}
	
	protected virtual void HealthChange() {
		if (health <= 0 && !dead) {
			Kill();
		}
	}

	public virtual void Kill() {
		if (!dead) {
			dead = true;
			Destroy (gameObject);
		}
	}
	#endregion

}

