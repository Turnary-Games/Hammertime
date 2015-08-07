using UnityEngine;
using System.Collections;

public class Tower : Minion {

	[Header("Tower Variables (DONT ALTER)")]

	[Tooltip("Fire particle system")]
	public ParticleSystem partSystem;
	[Tooltip("Projectile object to be fired")]
	public GameObject projectilePrefab;
	[Tooltip("Position the projectile should be fired from")]
	public Transform projectileStart;

	protected override void Start () {
		GameController.Get ().AddHealthbar (this);
	}

	protected override void Update () {
		if (paused)
			return;
		
		// Check for enemies
		Minion inAttackRange = attackRange.NearestMinion ();

		if (inAttackRange != null || attackVictim != null) {
			// Attack dat bastard
			Attack (inAttackRange);
		} else {
			state = State.idle;
		}
	}

	protected override void Attack (Minion victim) {
		if (state != State.attacking) {
			state = State.attacking;
		}

		attackWindupTime += Time.deltaTime;

		// Fire cooldown
		if (attackWindupTime >= attackWindup) {
			// Reset cooldown
			attackWindupTime = 0;

			// Fire ze missiles!
			Fire (victim);
		}

	}

	void Fire(Minion target) {
		Projectile.SpawnProjectile (projectilePrefab, projectileStart.position, target);
	}

	public override void Arrive () {
		state = State.arrived;
	}

	#region Pause methods
	protected override void OnPause () {
		partSystem.Pause ();
	}

	protected override void OnUnpause () {
		partSystem.Play ();
	}
	#endregion

}

