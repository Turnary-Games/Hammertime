using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : Pausable {

	public GameObject explosionPrefab;

	[Header("Wave settings")]

	[Tooltip("/waveWait/ seconds are waited between each wave.")]
	public float waveWait = 1;
	[Tooltip("This prefab will be cloned when spawning an ally minion at a spawnpoint rune.\n\n" +
	         "(PLEASE DONT ALTER)")]
	public GameObject allyPrefab;
	[Tooltip("This prefab will be cloned when spawning an enemy minion at a spawnpoint rune.\n\n" +
	         "(PLEASE DONT ALTER)")]
	public GameObject enemyPrefab;

	[Header("Money")]

	[Tooltip("The text object that displays the coins to the player.\n\n" +
	         "(PLEASE DONT ALTER)")]
	public TextMesh coinsText;
	[Tooltip("The amount of coins the player have to purchase minions.\n" +
	         "The value is capped at 0 and 999.")]
	public int coins;

	[Header("Coin prefab creation")]

	[Tooltip("The coin prefab that gets cloned whenever a coin shall be spawned.\n\n" +
	         "(PLEASE DONT ALTER)")]
	public GameObject coinPrefab;
	[Tooltip("When spawned, a coin gets /coinUpForce/ force added to its rigidbody upwards.")]
	public float coinUpForce;
	[Tooltip("When spawned, a coin gets /coinForwardForce/ force added to its rigidbody forward. " + 
	         "The forward direction depends on how many coins are spawned together.")]
	public float coinForwardForce;

	[Header("Player Health")]

	[Tooltip("Health of the player. If an enemy minion steps on an ally rune this value is decreased.")]
	public int playerHealth; // HP of user
	[Tooltip("Health of the evil tree. If an ally minion steps on an enemy rune this value is decreased.")]
	public int enemyHealth; // HP of evil tree
	[Tooltip("When the game is over all losing minions will one by one get destroyed in random order. " + 
	         "Wait /gameoverDelay/ seconds between each minion.")]
	public float gameoverDelay;

	[Header("Minion healthbars (DONT ALTER)")]

	[Tooltip("This prefab is cloned when a healthbar is needed.\n\n" +
	         "(PLEASE DONT ALTER)")]
	public GameObject healthbarPrefab;
	[Tooltip("This refers to the canvas the healthbar shall be placed on.\n\n" +
	         "(PLEASE DONT ALTER)")]
	public GameObject healthbarCanvas;

	private bool _gameover;
	public bool gameover {
		get {
			return _gameover;
		}
		set {
			bool state = _gameover;
			_gameover = value;
			
			// Trigger event
			if (state != value && value)
				OnGameover ();
		}
	}

#if UNITY_EDITOR
	private int coins_old;

	void OnValidate() {
		if (coins != coins_old) {
			coins = Mathf.Clamp(coins,0,999);
			UpdateText ();
		}
		coins_old = coins;
	}
#endif

	void Awake() {
		Explosion.prefab = explosionPrefab;
	}

	void Start() {
		UpdateText ();
	}

	void Update() {
		if (gameover) {
			GameoverStep ();
		} else if (!paused)
			WaveStep ();
	}

	#region Wave methods
	private float waveTimeLapsed;

	void WaveStep() {
		// Minion wave
		waveTimeLapsed += Time.deltaTime;
		
		if (waveTimeLapsed >= waveWait) {
			waveTimeLapsed -= waveWait;
			
			SpawnWave();
		}
	}

	void SpawnWave() {
		foreach(Spawnpoint point in FindObjectsOfType<Spawnpoint> ()) {
			point.Spawn(point.side == Side.ally ? allyPrefab : enemyPrefab).transform.SetParent(transform);
		}
	}
	#endregion

	#region Menu methods
	private MenuItem selectedItem;

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
	#endregion

	#region Coins
	void UpdateText() {
		coinsText.text = coins.ToString ();
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
	#endregion

	#region Gameover methods
	private List<Living> deathlist = new List<Living> ();
	private Side loser;
	private float gameoverElapsed;

	// Activated once when the game is over (someone lost)
	void OnGameover() {
		// Find out the loser
		loser = playerHealth <= 0 ? Side.ally : Side.enemy;

		// Pause all minions and projectiles.
		// This includes towers because they are child classes of Minion.
		Pausable.PauseAll<Minion> ();
		Pausable.PauseAll<Projectile> ();

		// Reset deathlist
		deathlist = new List<Living> ();

		// Get a deathlist, minions that are supposed to die
		foreach (Minion obj in FindObjectsOfType<Minion>()) {
			if (obj.side == loser) deathlist.Add(obj);
		}
		deathlist.AddRange (FindObjectsOfType<Projectile> ());

		deathlist.Shuffle ();

		// Reset time elapsed, just in case
		gameoverElapsed = 0;
	}

	// Runned each update when the game is over
	void GameoverStep() {
		// Dont have anything to do if all minions is dead
		if (deathlist.Count == 0)
			return;

		gameoverElapsed += Time.deltaTime;

		if (gameoverElapsed >= gameoverDelay) {
			gameoverElapsed -= gameoverDelay;

			// Time to destroy a minion!
			Living living = deathlist.Pop();

			Minion minion = living as Minion;
			Projectile proj = living as Projectile;

			if (minion) minion.Kill(false);
			if (proj) proj.Kill(true);
		}
	}
	#endregion

	#region Misc methods
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
	
	public void AddHealthbar(Living living) {
		GameObject clone = Instantiate (healthbarPrefab) as GameObject;
		clone.transform.SetParent (healthbarCanvas.transform);
		
		Healthbar healthbar = clone.GetComponent<Healthbar> ();
		healthbar.living = living;
	}
	#endregion

}
