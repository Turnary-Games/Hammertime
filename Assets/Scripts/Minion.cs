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
	private bool dead;
	private State state = State.idle;

	// Pathfinding
	private NavMeshAgent agent;
	private Target target;

	void Start() {
		agent = GetComponent<NavMeshAgent>();

		Move ();
	}

	void Update() {
		// Check for enemies
		Minion inAttackRange = attackRange.NearestMinion ();
		Minion inVisionRange = visionRange.NearestMinion ();

		if (inAttackRange != null) {
			// Attack dat bastard
			Attack (inAttackRange);
		} else if (inVisionRange != null) {
			// Follow dat fellow
			Follow (inVisionRange);
		} else {
			Move ();
		}
	}

	void Attack(Minion victim) {
		if (state != State.attacking) {
			state = State.attacking;

			// Initial
			agent.Stop();
			agent.updateRotation = false;
		}

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

	void Stare(Vector3 position) {
		Vector3 lookDir = new Vector3 (position.x, transform.position.y, position.z) - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, lookDir, Time.deltaTime * stareAngularSpeed, 0.0f);
		transform.rotation = Quaternion.LookRotation (newDir);
	}

	/*
	void OnDrawGizmos() {
		if (target != null) {
			if (target.targetType == Target.TargetType.checkpoint)
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.red;

			Gizmos.DrawLine (transform.position, target.GetPosition ());
		}
	}
	*/

	void SetTarget(Target target) {
		this.target = target;

		agent.SetDestination (target.GetPosition ());
	}

	public static bool SameSide(Minion a, Minion b) {
		return a.SameSide (b);
	}
	public bool SameSide (Minion other) {
		return this.side == other.side;
	}

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
