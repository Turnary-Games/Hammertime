using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject explosionPrefab;

	[Header("Wave settings")]
	public float firstWaveWait = 1;
	public float waveWait = 1;
	public GameObject allyPrefab;
	public GameObject enemyPrefab;

	[Header("Money")]
	public TextMesh coinsText;
	public int coins;

	[Header("Coin prefab creation")]
	public GameObject coinPrefab;
	public float coinUpForce;
	public float coinForwardForce;

	[Header("Player Health")]
	public int playerHealth; // HP of user
	public int enemyHealth; // HP of evil tree
	public bool gameover = false;

	private MenuItem selectedItem;

#if UNITY_EDITOR
	private int coins_old;

	void OnValidate() {
		if (coins != coins_old)
			UpdateText ();
		coins_old = coins;
	}
#endif

	void Awake() {
		Explosion.prefab = explosionPrefab;
	}

	void Start() {
		StartCoroutine (MinionWave ());
		UpdateText ();
	}

	IEnumerator MinionWave() {
		yield return new WaitForSeconds(firstWaveWait);

		while (!gameover) {
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

	void UpdateText() {
		coinsText.text = coins.ToString ();
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
				UpdateText ();
			}
		}
	}

	public void AddCoins(int amount) {
		coins = Mathf.Min (coins + amount, 999);
		UpdateText ();
	}

	public bool IsSelected(MenuItem item) {
		return selectedItem == item;
	}

	void SpawnCoin(Vector3 pos, float angle) {
		GameObject clone = Instantiate (coinPrefab, pos, Quaternion.Euler (0, Random.Range (0, 360), 0)) as GameObject;
		Rigidbody rbody = clone.GetComponent<Rigidbody> ();
		
		Vector3 force = Vector3.up * coinUpForce;
		force += new Vector3 (Mathf.Sin (Mathf.Deg2Rad * angle), 0, Mathf.Cos (Mathf.Deg2Rad * angle)) * coinForwardForce;

		rbody.AddForce (force, ForceMode.Impulse);
	}

	public void SpawnCoins(Vector3 pos, int amount) {
		if (amount > 0) {
			float offset = Random.Range (0f, 360f);

			for (int i=0; i<amount; i++) {
				float angle = offset + 360f * i / amount;
				SpawnCoin (pos, angle);
			}
		}
	}

	public static GameController Get() {
		return FindObjectOfType<GameController> ();
	}

	public void Damage(Side sourceSide, int amount = 1) {
		if (gameover)
			return;

		if (sourceSide == Side.ally) {
			enemyHealth -= amount;
			if (enemyHealth <= 0)
				gameover = true;
		} else if (sourceSide == Side.enemy) {
			playerHealth -= amount;
			if (playerHealth <= 0)
				gameover = true;
		}
	}
	
}
