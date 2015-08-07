using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : Pausable {

	[Header("Variables (DONT ALTER)")]

	[Tooltip("Spawnpoints for the skull projectiles that is.")]
	public List<Transform> spawnpoints;
	[Tooltip("When this object is paused the containing particle systems will be paused, " + 
	         "and when the object is unpaused the particle systems will be resumed.")]
	public List<ParticleSystem> partSystems;
	public GameObject projectilePrefab;

	[Header("Settings")]

	[Tooltip("Time in seconds the boss waits when trying to spawn a skull projectile. " +
	         "If it fails (like if theres no allied minions on the field) it will just wait /spawnDelay/ seconds again.")]
	public float spawnDelay;

	private float timeElapsed;

	void Update() {
		if (paused)
			return;

		timeElapsed += Time.deltaTime;

		if (timeElapsed >= spawnDelay) {
			Projectile.SpawnProjectile (projectilePrefab, RandomSpawnpoint ());
			timeElapsed -= spawnDelay;
		}
	}

	Vector3 RandomSpawnpoint() {
		return spawnpoints [Random.Range (0, spawnpoints.Count)].position;
	}

	#region Pause methods
	protected override void OnPause () {
		partSystems.ForEach (delegate(ParticleSystem obj) {
			obj.Pause ();
		});
	}

	protected override void OnUnpause () {
		partSystems.ForEach (delegate(ParticleSystem obj) {
			obj.Play ();
		});
	}
	#endregion

}
