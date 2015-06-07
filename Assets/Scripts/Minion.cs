using UnityEngine;
using System.Collections;

public enum Side { ally, enemy };
public enum State { moving, attacking, following, idle };

public class Minion : MonoBehaviour {

	[Space(12)]
	public Checkpoint goal;
	public RangeArea attackRange;
	public RangeArea visionRange;
	public float stareAngularSpeed = 1;
	[Space(12)]
	public Side side;
	public int health = 1;
	public int damage = 1;
	public float attackWindup;

	private Minion attackVictim;
	private Coroutine attackCoroutine;

	private bool canAttack = true;
	private bool dead;
	private State state = State.idle;

	// Pathfinding
	private NavMeshAgent agent;

	void Start() {
		agent = GetComponent<NavMeshAgent>();

		Move ();
	}

	void Update() {
		// Check for enemies
		Minion inAttackRange = attackRange.NearestMinion ();
		Minion inVisionRange = visionRange.NearestMinion ();

		if (inAttackRange != null || attackVictim != null) {
			// Attack dat bastard
			Attack (inAttackRange);
		} else if (inVisionRange != null) {
			// Chase 'em
			Follow (inVisionRange);
		} else {
			// Move towards goal
			Move ();
		}
	}

	#region Main instructions (Attack, Follow, Move)
	void Attack(Minion victim) {
		if (state != State.attacking) {
			state = State.attacking;

			// Initial
			agent.Stop();
			agent.updateRotation = false;
		}

		// No existing victim
		if (attackVictim == null)
			attackVictim = victim;

		// Victim change
		if (attackVictim != victim) {
			canAttack = true;
			if (attackCoroutine != null)
				StopCoroutine(attackCoroutine);
		}

		Punch ();
		Stare (victim.transform.position);
	}

	void Follow(Minion victim) {
		if (state != State.following) {
			state = State.following;

			// Initial
			SetTarget(new Target(victim));
			agent.updateRotation = false;
		}
		
		Stare (victim.transform.position);
	}

	void Move() {
		if (state != State.moving) {
			state = State.moving;

			// Initial
			
			SetTarget(new Target(goal));
			agent.updateRotation = true;
		}
	}
	#endregion

	void Stare(Vector3 position) {
		Vector3 lookDir = new Vector3 (position.x, transform.position.y, position.z) - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, lookDir, Time.deltaTime * stareAngularSpeed, 0.0f);
		transform.rotation = Quaternion.LookRotation (newDir);
	}

	#region Attacking (Punch, AttackCooldown)
	void Punch() {
		if (canAttack) {
			attackCoroutine = StartCoroutine(AttackWindup());
		}
	}

	IEnumerator AttackWindup() {
		canAttack = false;
		yield return new WaitForSeconds (attackWindup);

		// Hasen't died while winding up
		if (attackVictim != null)
			attackVictim.Damage (damage);

		attackCoroutine = null;
		attackVictim = null;
		canAttack = true;
	}
	#endregion

	void SetTarget(Target target) {
		agent.SetDestination (target.GetPosition ());
		agent.Resume ();
	}

	#region Comparison methods (SameSide)
	public static bool SameSide(Minion a, Minion b) {
		return a.SameSide (b);
	}
	public bool SameSide (Minion other) {
		return this.side == other.side;
	}
	#endregion

	#region Raw damage methods (Damage, HealthChange, Die)
	// returns Boolean: true=died, false=survived
	public bool Damage(int amount = 1) {
		health -= amount;
		HealthChange ();
		return health <= 0;
	}

	void HealthChange() {
		if (health <= 0 && !dead) {
			Die();
		}
	}

	void Die() {
		dead = true;
		Destroy(gameObject);
		Explosion.CreateExplosion (transform.position);
	}
	#endregion

}
