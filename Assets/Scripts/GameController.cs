using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject explosionPrefab;
	[Space(12)]
	public float firstWaveWait = 1;
	public float waveWait = 1;
	[Space(12)]
	public GameObject allyPrefab;
	public GameObject enemyPrefab;

	void Awake() {
		Explosion.prefab = explosionPrefab;
	}

	void Start() {
		StartCoroutine (MinionWave ());
	}

	IEnumerator MinionWave() {
		yield return new WaitForSeconds(firstWaveWait);

		while (true) {
			SpawnWave();

			yield return new WaitForSeconds(waveWait);
		}
	}

	void SpawnWave() {
		List<Spawnpoint> spawnpoints = new List<Spawnpoint> (FindObjectsOfType<Spawnpoint> ());

		spawnpoints.ForEach (delegate(Spawnpoint obj) {
			obj.Spawn(obj.side == Side.ally ? allyPrefab : enemyPrefab).transform.SetParent(transform);
		});
	}

}
