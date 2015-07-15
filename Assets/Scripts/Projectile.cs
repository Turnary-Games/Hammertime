using UnityEngine;
using System.Collections;

public class Projectile : Living {

	[HideInInspector] public Minion target;

	[Header("Variables (DONT ALTER)")]
	
	public ProjectileMark mark;
	public BoxCollider boxCollider;

	[Header("Behaviour settings")]

	[Tooltip("What should the projectile do if its target dies before it reaches it?\n\n"+
	         "<Explode In Place>\nJust explode the instance it notices its target is dead.\n\n"+
	         "<Find New Target>\nTry to find a new target, if not then just /explode in place/.\n\n"+
	         "<Explode At Target>\nKeep travelling towards where the target died and explode there.")]
	public TargetDeadAction ifTargetDies;
	[Tooltip("When has the projectile arrived at the target?\n\n"+
	         "<Contact With Mark>\nOnly explode when it reaches the mark.\n\n"+
	         "<Contact With Minion>\nOnly explode when it comes in contact with the target minion. "+
	         "Not compatible with /Explode At Target/ action if the target dies before it reaches it.\n\n"+
	         "<Both>\nWill explode when it reaches the mark if it doesn't collide with the target minion on the way.")]
	public ExplodeType explodeWhen;
	[Tooltip("How should the damage be dealt?\n\n"+
	         "<Splash>\nDeals damage from the mark to all nearby minions in a radius defined in the explosion prefab.\n\n"+
	         "<Only To Target>\nWill only deal damage to the target. "+
	         "Not compatible with /Explode At Target/ action if the target does before it reaches it.")]
	public DamageType damageType;

	[Header("Other settings")]
	[Tooltip("Movement speed in units per second")]
	public float speed;
	[Tooltip("Damage dealt to target minion on impact")]
	public int damage = 1;
	[Tooltip("Coins dropped if killed by the hammer")]
	public int reward = 1;
	[Tooltip("Should the projectile spawn in with a healthbar above it?")]
	public bool spawnWithHealthbar;

	void Start() {

		if (ifTargetDies == TargetDeadAction.explodeAtTarget) {
			if (explodeWhen == ExplodeType.contactWithMinion)
				Debug.LogError("Warning! ExplodeAtTarget and ContactWithMinion are not compatible!");
			if (damageType == DamageType.onlyToTarget)
				Debug.LogError("Warning! ExplodeAtTarget and OnlyToTarget are not compatible!");
		}

		if (target != null) {
			mark.transform.SetParent(transform.parent,true);
			mark.transform.rotation = Quaternion.identity;
			MoveMark ();

			if (spawnWithHealthbar)
				GameController.Get().AddHealthbar(this);
		} else
			Die (true);
	}

	void OnTriggerEnter(Collider other) {
		GameObject obj = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

		switch (explodeWhen) {

		case ExplodeType.contactWithMark:
			ContactWithMark(obj);
			break;

		case ExplodeType.contactWithMinion:
			ContactWithMinion(obj);
			break;

		case ExplodeType.both:
			ContactWithMinion(obj);
			ContactWithMark(obj);
			break;
		}

	}

	void ContactWithMark(GameObject obj) {
		if (ifTargetDies == TargetDeadAction.explodeAtTarget) {
			ProjectileMark _mark = obj.GetComponent<ProjectileMark> ();
			if (_mark == mark) {
				Die ();
			}
		}
	}

	void ContactWithMinion(GameObject obj) {
		Minion _minion = obj.GetComponent<Minion> ();
		if (_minion == target && target != null) {
			Die ();
			if (damageType == DamageType.onlyToTarget)
				target.Damage(damage);
		}
	}

	#region Movement (MoveSkull, MoveMark, FindTarget)
	void MoveSkull() {
		transform.position = Vector3.MoveTowards (transform.position, mark.transform.position, speed * Time.deltaTime);
		transform.LookAt (mark.transform);
	}

	void MoveMark() {
		mark.transform.position = target.transform.position;
	}

	void Update() {
		if (target == null) {
			switch(ifTargetDies) {

			case TargetDeadAction.findNewTarget:
				Minion closest = NearestTarget(transform.position);
				
				if (closest != null) // Found one
					target = closest;
				else // EVERYONE IS DEAD :'(
					Die ();

				break;

			case TargetDeadAction.explodeAtTarget:
				MoveSkull();
				break;

			case TargetDeadAction.explodeInPlace:
				Die (true);
				break;

			}
		} else {
			MoveMark();
			MoveSkull();
		}
	}
	#endregion

	#region Damage and health (Damage, HealthChange, Kill)
	// returns Boolean: true=died, false=survived
	public override bool Damage(int amount = 1) {
		return base.Damage (amount);
	}
	
	protected override void HealthChange() {
		if (health <= 0 && !dead) {
			Die(true);
		}
	}

	protected override void Die() {
		Die (false);
	}

	protected void Die(bool withoutExplosion) {
		if (!dead) {
			dead = true;
			Destroy (gameObject);

			Destroy (mark.gameObject);
			if (!withoutExplosion)
				Explosion.CreateExplosion (mark.transform.position, damageType == DamageType.splash ? damage : 0, Side.ally);
		}
	}
	#endregion
	
	#region Static methods
	public static void SpawnProjectile(GameObject prefab, Vector3 source) {
		Minion target = NearestTarget (source);
		
		if (target != null) {
			Quaternion rotation = Quaternion.LookRotation(target.transform.position - source);
			GameObject clone = Instantiate (prefab, source, rotation) as GameObject;

			Projectile script = clone.GetComponent<Projectile>();
			script.target = target;
		}
	}
	
	static Minion NearestTarget(Vector3 source) {
		Minion closest = null;
		
		foreach (Minion minion in FindObjectsOfType<Minion>()) {
			// HARD CODING FTW
			if (minion.side == Side.enemy) {
				continue;
			}

			if (closest == null) {
				closest = minion;
			} else {
				// Distance to this iteration
				float distToMinion = Vector3.Distance(source, minion.transform.position);
				// Distance to the closest
				float distToClosest = Vector3.Distance(source, closest.transform.position);
				
				if (distToMinion < distToClosest) {
					closest = minion;
				}
			}
		}

		return closest;
	}
	#endregion

	#region Enums
	public enum TargetDeadAction {
		explodeInPlace,
		findNewTarget,
		explodeAtTarget
	};

	public enum ExplodeType {
		contactWithMark,
		contactWithMinion,
		both
	};

	public enum DamageType {
		splash,
		onlyToTarget
	};
	#endregion
}
