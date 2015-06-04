using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour {

	public List<Transform> spawnpoints;
	public GameObject projectilePrefab;
	public float spawnDelay;

	void Start() {
		StartCoroutine (AutoSpawner ());
	}

	IEnumerator AutoSpawner() {
		while (true) {
			Projectile.SpawnProjectile (projectilePrefab, RandomSpawnpoint ());
			yield return new WaitForSeconds (spawnDelay);
		}
	}

	Vector3 RandomSpawnpoint() {
		return spawnpoints [Random.Range (0, spawnpoints.Count)].position;
	}


}
