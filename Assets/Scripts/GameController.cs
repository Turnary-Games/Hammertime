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
	[Space(12)]
	public int coins;
	private MenuItem selectedItem;

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

	public void Select(MenuItem item) {
		selectedItem = item;
	}

	public void Deselect() {
		selectedItem = null;
	}

	public void PurchaseSelected(Spawnpoint spawnpoint) {
		if (selectedItem != null) {
			if (coins >= selectedItem.price) {
				spawnpoint.Spawn(selectedItem.prefab);
				coins -= selectedItem.price;
			}
		}

		Deselect ();
	}

	public bool IsSelected(MenuItem item) {
		return selectedItem == item;
	}

}
