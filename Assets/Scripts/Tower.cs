using UnityEngine;
using System.Collections;

public class Tower : Minion {

	[Header("Tower specific")]

	[Tooltip("Projectile object to be fired")]
	public GameObject projectilePrefab;
	[Tooltip("Position the projectile should be fired from")]
	public Transform projectileStart;

	private float fireTime;

	protected override void Start () {
		// do nothing
	}

	protected override void Update () {
		// Check for enemies
		Minion inAttackRange = attackRange.NearestMinion ();

		if (inAttackRange != null || attackVictim != null) {
			// Attack dat bastard
			Attack (inAttackRange);
		}
	}

	protected override void Attack (Minion victim) {
		if (state != State.attacking) {
			state = State.attacking;
		}

		// Fire cooldown
		if (Time.time - fireTime >= attackWindup) {
			// Reset cooldown
			fireTime = Time.time;

			// Fire ze missiles!
			Fire (victim);
		}

	}

	void Fire(Minion victim) {
		Vector3 direction = (victim.transform.position - transform.position).normalized;
		Quaternion rotation = Quaternion.LookRotation (direction);

		GameObject clone = Instantiate (projectilePrefab, projectileStart.position, rotation) as GameObject;
		Projectile projectile = clone.GetComponent<Projectile> ();

		if (projectile != null)
			projectile.target = victim;
		else
			Destroy (clone);
	}

	public override void Arrive () {
		state = State.arrived;
	}

}

