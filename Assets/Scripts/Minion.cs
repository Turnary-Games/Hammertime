using UnityEngine;
using System.Collections;

public enum Side { ally, enemy };
public enum State { moving, attacking, following, idle, arrived };

static class SideMethods {
	public static Side Invert(this Side side) {
		if (side == Side.ally)
			return Side.enemy;
		else
			return Side.ally;
	}
}

public class Minion : Living {

	[Header("Variables (DONT ALTER)")]

	public Checkpoint goal;
	public RangeArea attackRange;
	public RangeArea visionRange;
	public GameObject arrivePrefab;

	[Header("Settings")]

	[Tooltip("Will only attack minions on other /side/")]
	public Side side;
	[Tooltip("ENEMY ONLY! Gold dropped upon death")]
	public int reward = 1; // coins
	[Tooltip("Damage dealt to minions")]
	public int damage = 1;
	[Tooltip("Delay in seconds an attack takes")]
	public float attackWindup;
	[Tooltip("Movement speed")]
	public float agentSpeed;
	[Tooltip("Rotation speed for turning towards a minion when fighting")]
	public float stareAngularSpeed = 1;

	protected Minion attackVictim;
	protected Coroutine attackCoroutine;

	protected State state = State.idle;
	protected float attackWindupTime;
	
	// Pathfinding
	private NavMeshAgent agent;

	protected virtual void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.speed = agentSpeed;
		
		Move ();

		GameController.Get ().AddHealthbar (this);
	}

	protected virtual void Update() {
		if (paused)
			return;

		// Check for enemies
		Minion inAttackRange = attackRange.NearestMinion ();
		Minion inVisionRange = visionRange.NearestMinion ();

		if (state != State.arrived) {
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
	}

	#region Main instructions (Attack, Follow, Move, Arrive)
	protected virtual void Attack(Minion victim) {
		if (state != State.attacking) {
			state = State.attacking;

			// Initial
			agent.Stop();
			agent.updateRotation = false;
		}

		// Victim change
		if (attackVictim != victim) {
			attackVictim = victim;
			WindupStart();
		}

		if (attackVictim != null) {
			Stare (attackVictim.transform.position);
			WindupStep();
		}
	}

	protected void Follow(Minion victim) {
		if (state != State.following) {
			state = State.following;

			// Initial
			SetTarget(new Target(victim));
			agent.updateRotation = false;
		}
		
		Stare (victim.transform.position);
	}

	protected void Move() {
		if (state != State.moving) {
			state = State.moving;

			// Initial
			if (goal == null)
				Debug.LogError("The minion must have a goal with life :)");

			SetTarget(new Target(goal));
			agent.updateRotation = true;
		}
	}

	public virtual void Arrive() {
		if (state != State.arrived) {
			state = State.arrived;

			// Remove all unnessesary stuffs
			Destroy(gameObject);

			GameObject clone = Instantiate(arrivePrefab,transform.position,transform.rotation) as GameObject;
			MinionArrival arrival = clone.GetComponent<MinionArrival>();
			arrival.side = side;
		}
	}
	#endregion

	protected void Stare(Vector3 position) {
		Vector3 lookDir = new Vector3 (position.x, transform.position.y, position.z) - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, lookDir, Time.deltaTime * stareAngularSpeed, 0.0f);
		transform.rotation = Quaternion.LookRotation (newDir);
	}

	#region Attacking (WindupStart, WindupComplete, WindupStep)
	protected void WindupStart() {
		attackWindupTime = 0;
		// TODO: Add animation thingie here.
	}

	protected void WindupComplete() {
		// Time to kill
		attackVictim.Damage(damage);
		
		// Reset timer
		attackWindupTime -= attackWindup;
	}

	protected void WindupStep() {
		attackWindupTime += Time.deltaTime;

		// TODO: Or add the animation thingie here.

		if (attackWindupTime >= attackWindup) {
			WindupComplete();
		}
	}
	#endregion

	protected void SetTarget(Target target) {
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

	#region Raw damage methods (Damage, HealthChange, Die, Kill)
	// returns Boolean: true=died, false=survived
	public override bool Damage(int amount = 1) {
		return base.Damage (amount);
	}

	protected override void HealthChange() {
		base.HealthChange ();
	}

	// Kill the minion
	public override void Kill() {
		Kill (true);
	}

	public void Kill(bool coins) {
		if (!dead) {
			dead = true;
			Destroy (gameObject);
			
			Explosion.CreateExplosion (transform.position, 0);
			
			if (coins && side == Side.enemy)
				GameController.Get ().SpawnCoins (transform.position, reward);
		}
	}
	#endregion

	#region Pause methods
	// Solution to pausing NavMeshAgent:
	// http://answers.unity3d.com/questions/351049/navmesh-agent-pause.html
	// Credits to Morm91 on Unity Answers
	private Vector3 lastAgentVelocity;
	private NavMeshPath lastAgentPath;

	protected override void OnPause () {
		state = State.idle;

		// Store agent data
		lastAgentVelocity = agent.velocity;
		lastAgentPath = agent.path;
		
		// Reset agent
		agent.velocity = Vector3.zero;
		agent.ResetPath ();
	}

	protected override void OnUnpause () {
		// Resume agent
		agent.velocity = lastAgentVelocity;
		agent.SetPath (lastAgentPath);
	}
	#endregion

}
