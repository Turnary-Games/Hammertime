using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : Pausable {

	[Header("Variables (DONT ALTER)")]

	public GameObject explosionPrefab;
	[Tooltip("When a minion is spawned they will be placed inside the /minionsParent/ gameobject as a child object.")]
	public Transform minionsParent;
	[Tooltip("When a coin is spawned they will be placed inside the /coinsParent/ gameobject as a child object.")]
	public Transform coinsParent;

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
	public BossHealthbar playerHealthbar;
	public BossHealthbar enemyHealthbar;

	[Header("Minion healthbars (DONT ALTER)")]

	[Tooltip("This prefab is cloned when a healthbar is needed.\n\n" +
			 "(PLEASE DONT ALTER)")]
	public GameObject healthbarPrefab;
	[Tooltip("This refers to the canvas the healthbar shall be placed on.\n\n" +
			 "(PLEASE DONT ALTER)")]
	public RectTransform healthbarCanvas;

	[Space]
	[Tooltip("This prefab is cloned when a damage indicator is needed.\n\n" +
			 "(PLEASE DONT ALTER)")]
	public GameObject damageIndicatorPrefab;
	[Tooltip("This refers to the canvas the damage indicators shall be placed on.\n\n" +
			 "(PLEASE DONT ALTER)")]
	public RectTransform damageIndicatorCanvas;

	[Header("Gameover text")]

	[Tooltip("Time (in seconds) it takes for the text to go from 0 size to full size.")]
	public float gameoverTextTime = 5f;
	public AnimationCurve gameoverTextCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
	[Space]
	public Transform gameoverTextParent;
	public GameObject gameoverVictory;
	public GameObject gameoverDefeat;

	private bool _gameover;
	public bool gameover {
		get {
			return _gameover;
		}
		set {
			// Trigger event
			if (!_gameover && value) {
				_gameover = value;
				OnGameover();
			}
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
		playerHealthbar.InitHealthbar(playerHealth, playerHealth);
		enemyHealthbar.InitHealthbar(enemyHealth, enemyHealth);
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
			point.Spawn(point.side == Side.ally ? allyPrefab : enemyPrefab).transform.SetParent(minionsParent!=null ? minionsParent : transform);
		}
	}
	#endregion

	#region Item menu methods
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

		clone.transform.SetParent(coinsParent!=null ? coinsParent : transform);

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

		// Activate the appropiet text
		(loser == Side.ally ? gameoverDefeat : gameoverVictory).SetActive(true);
		gameoverTextParent.localScale = Vector3.zero;

		// Pause all minions and projectiles.
		// This includes towers because they are child classes of Minion.
		/*
		Pausable.PauseAll<Minion> ();
		Pausable.PauseAll<Projectile> ();
		Pausable.PauseAll<HammerController>();
		*/

		// No, just pause EVERYTHING
		Pausable.PauseAll();

		// Reset deathlist
		deathlist = new List<Living> ();

		// Get a deathlist, minions that are supposed to die
		foreach (Minion obj in FindObjectsOfType<Minion>()) {
			if (obj.side == loser) deathlist.Add(obj);
		}

		deathlist.Shuffle ();

		// Reset time elapsed, just in case
		gameoverElapsed = 0;
	}

	// Runned each update when the game is over
	void GameoverStep() {
		gameoverElapsed += Time.deltaTime;

		// Part one, kill all losers
		if (deathlist.Count > 0) {
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

		// Part two, reveal the text

		float time = gameoverElapsed / gameoverTextTime;
		float value = gameoverTextCurve.Evaluate(time);

		gameoverTextParent.localScale = Vector3.one * value;
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
			enemyHealthbar.SetHealth(enemyHealth);
			if (enemyHealth <= 0)
				gameover = true;
		} else if (sourceSide == Side.enemy) {
			playerHealth -= amount;
			playerHealthbar.SetHealth(playerHealth);
			if (playerHealth <= 0)
				gameover = true;
		}
	}
	
	public Healthbar AddHealthbar(Living living) {
		// Create at canvas
		GameObject clone = Instantiate(healthbarPrefab, healthbarCanvas.transform.position, healthbarCanvas.transform.rotation) as GameObject;
		// Set parent without moving
		clone.transform.SetParent(healthbarCanvas.transform, true);
		// Reset scale
		clone.transform.localScale = Vector3.one;

		// Get the object
		Healthbar healthbar = clone.GetComponent<Healthbar>();

		// Set some values
		healthbar.canvas = healthbarCanvas;
		healthbar.living = living;

		return healthbar;
	}

	public Healthbar AddHealthbar(Living living, Vector2 offset) {
		Healthbar healthbar = AddHealthbar(living);
		healthbar.offset = offset;

		return healthbar;
	}

	public DamageIndicator AddDamageIndicator(Vector3 position, int damage) {
		// Create at canvas
		GameObject clone = Instantiate(damageIndicatorPrefab, damageIndicatorCanvas.transform.position, damageIndicatorCanvas.transform.rotation) as GameObject;
		// Set parent without moving
		clone.transform.SetParent(healthbarCanvas.transform, true);
		// Reset scale
		clone.transform.localScale = Vector3.one;

		// Get the object
		DamageIndicator indicator = clone.GetComponent<DamageIndicator>();

		// Set some values
		indicator.canvas = damageIndicatorCanvas;
		indicator.text.text = "-" + damage.ToString();

		// Set the position
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
		indicator.indicator.anchoredPosition = screenPoint - indicator.canvas.sizeDelta / 2f;

		return indicator;
	}
	#endregion

}
