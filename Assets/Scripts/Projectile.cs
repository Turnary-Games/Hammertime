﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Minion target;
	public Transform mark;
	public float speed;

	void Update() {
		FindTarget ();
		if (target != null) {
			MoveSkull ();
			MoveMark ();
		}
	}

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

	void HitTarget() {
		target.Damage ();
	}

	void Kill() {
		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider other) {
		Minion minion = other.GetComponent<Minion>();
		
		if (minion == target) {
			HitTarget ();
			Kill ();
		}
	}
	
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

}
