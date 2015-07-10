using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	[HideInInspector] public Minion target;
	public Transform mark;
	public float speed;

	public int health = 1;
	public int damage = 1;
	public int reward = 1;
	private bool dead;

	void Update() {
		FindTarget ();
		if (target != null) {
			MoveSkull ();
			MoveMark ();
		}
	}

	void OnTriggerEnter(Collider other) {
		Minion minion = GetMinion (other);
		
		if (minion == target) {
			HitTarget ();
		}
	}

	Minion GetMinion(Collider other) {
		if (other.attachedRigidbody != null)
			return other.attachedRigidbody.GetComponent<Minion> ();
		else
			return other.GetComponent<Minion> ();
	}

	#region Movement (MoveSkull, MoveMark, FindTarget)
	void MoveSkull() {
		transform.position = Vector3.MoveTowards (transform.position, target.transform.position, speed * Time.deltaTime);
		transform.LookAt (target.transform);
	}

	void MoveMark() {
		mark.position = target.transform.position;
	}

	void FindTarget() {
		if (target == null) {
			Minion closest = NearestTarget(transform.position);

			if (closest != null) // Found one
				target = closest;
			else // EVERYONE IS DEAD :'(
				Kill ();
		}
	}
	#endregion

	#region Damage and health (HitTarget, Damage, HealthChange, Kill)
	void HitTarget() {
		target.Damage (damage);
		Kill (target.health <= 0);
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
	
	void Kill(bool withoutExplosion = false) {
		dead = true;
		Destroy(gameObject);
		if (!withoutExplosion)
			Explosion.CreateExplosion (transform.position);
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

}
