using UnityEngine;
using System.Collections;

public class Living : MonoBehaviour
{
	[Header("Living settings")]
	
	[Tooltip("Hitpoints, can take /health/ damage before dying. Can only currently take damage from the hammer")]
	public int health;
	[HideInInspector]
	public bool dead;

	#region Damage and health (Damage, HealthChange, Kill)
	// returns Boolean: true=died, false=survived
	public virtual bool Damage(int amount = 1) {
		health -= amount;
		HealthChange ();
		return health <= 0;
	}
	
	protected virtual void HealthChange() {
		if (health <= 0 && !dead) {
			Die();
		}
	}
	
	protected virtual void Die() {
		if (!dead) {
			dead = true;
			Destroy (gameObject);
		}
	}
	#endregion

}

