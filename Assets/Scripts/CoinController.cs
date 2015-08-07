using UnityEngine;
using System.Collections;

public class CoinController : Pausable {

	[Header("Variables (DONT ALTER)")]

	public ParticleSystem partSystem;

	[Header("Settings")]

	[Tooltip("Speed of flight")]
	public float speed;
	[Tooltip("Time it takes for the coin to reach full speed in seconds.")]
	public float fullSpeedAfter; // seconds
	[Tooltip("Time delay from creation before the coin starts flying")]
	public float delay;

	private float flyTime;
	private bool started;
	private Vector3 start;

	private CoinFlyGoal goal;
	private Rigidbody rbody;

	void Start() {
		goal = FindObjectOfType<CoinFlyGoal> ();
		rbody = GetComponent<Rigidbody> ();
	}

	void Update() {
		if (paused)
			return;

		// Increment time
		flyTime += Time.deltaTime;

		if (flyTime >= delay)
			Move ();
	}

	void Move() {
		if (!started) {
			started = true;
			start = transform.position;

			// Disable the rigidbody
			rbody.isKinematic = true;
			rbody.velocity = Vector3.zero;
		}
		
		// Position variable
		Vector3 pos = transform.position;
		Vector3 tar = goal.transform.position; // aka target
		
		// Move towards goal horizontally using Vec2.MoveTowards
		MoveXZ (ref pos, tar);
		
		// Move Y in a cosine curve towards target Y
		MoveY (ref pos, tar);

		// Finally set the pos
		transform.position = pos;
	}

	void MoveXZ(ref Vector3 pos, Vector3 tar) {
		// Calculate the movement speed scale (percentage)
		float scale = Mathf.Clamp((flyTime-delay) / fullSpeedAfter, 0f, 1f);

		// Z instead of Y in the Vec2
		Vector2 pos2 = Vector2.MoveTowards (new Vector2 (pos.x, pos.z), new Vector2 (tar.x, tar.z), speed * scale * Time.deltaTime);
		pos.x = pos2.x;
		pos.z = pos2.y;
	}

	void MoveY(ref Vector3 pos, Vector3 tar) {
		// Calculate the distance travelled in procent
		// Without the "1-X" it represents the distance left in procent 
		float x = 1 - Vector3.Distance (transform.position, tar) / Vector3.Distance (start, tar);

		// The max height of the cosine
		// This can explain it a little: https://www.desmos.com/calculator/wcdef3owbn
		float h = tar.y;
		
		// Calculate the final equation
		float y = -Mathf.Cos (x * Mathf.PI) * h / 2 + h / 2;

		pos.y = Mathf.Lerp (pos.y, y, Time.deltaTime * speed);
	}

	#region Pause methods
	protected override void OnPause () {
		partSystem.Pause ();
	}

	protected override void OnUnpause () {
		partSystem.Play ();
	}
	#endregion

}
