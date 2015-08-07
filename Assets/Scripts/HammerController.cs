using UnityEngine;
using System.Collections;

public class HammerController : Pausable {

	[Header("Variables (DONT ALTER)")]

	public LayerMask raycastPlainMask;
	public LayerMask raycastObstacleMask;
	public ParticleSystem thump;
	public GameObject mesh;

	[Header("Settings")]

	[Tooltip("Damage dealt to minions and towers when wacked")]
	public int damage = 1;
	[Tooltip("After each wack with the hammer wait /attackCooldown/ seconds before you can wack again")]
	public float attackCooldown;

	private float attackCooldownTime;
	private bool visable = true;
	private bool isVisable = true;
	private Vector3 startPoint;
	private Vector3 point;
	private Animator anim;

	[HideInInspector]
	public Wackable wackingTarget;

	void Start() {
		anim = GetComponent<Animator> ();
		startPoint = transform.position;
	}

	void Update () {
		if (paused)
			return;

		// Cooldown
		CooldownStep ();

		// Look for /stuff/ in the way, get the position of the mouse in 3D space
		Raycast ();

		// Move to point
		transform.position = new Vector3 (point.x, Mathf.Max (point.y, startPoint.y), point.z);
	}

	void Raycast() {
		// Variables
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit obstacleHit;
		RaycastHit plainHit;

		visable = true;

		// Cast a ray for the plain
		if(Physics.Raycast (ray, out plainHit, Mathf.Infinity, raycastPlainMask))
			HandlePlainHit(plainHit);

		// Cast a ray for any misc. obstacles
		if (Physics.Raycast (ray, out obstacleHit, Mathf.Infinity, raycastObstacleMask))
			HandleObstacleHit (obstacleHit);
		else
			wackingTarget = null;

		SetVisable (visable);

		if (Input.GetMouseButtonDown (0)) {
			Punch ();
		}

	}

	void HandleObstacleHit(RaycastHit hit) {
		GameObject obj = hit.collider.attachedRigidbody != null ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;

		if (obj.tag == "InvisHammer")
			visable = false;

		Wackable wack = obj.GetComponent<Wackable> ();
		if (wack) {
			wackingTarget = wack;
			point = wack.customPivot == null ? wack.transform.position : point = wack.customPivot.position;
		} else {
			wackingTarget = null;
		}
	}

	void HandlePlainHit(RaycastHit hit) {
		point = hit.point;
		if (hit.collider.tag == "InvisHammer")
			visable = false;
	}

	void Punch() {
		if (CanAttack ()) {
			if (isVisable) {
				anim.SetTrigger ("Swing");
				CooldownStart ();
			} else {
				// Skip the animation
				Wack ();
			}
		}
	}

	// Called from animation event of the "Swing"
	void Wack() {
		if (wackingTarget != null) {
			wackingTarget.Wack (damage);

			if (isVisable)
				thump.Play ();
		}
	}

	void SetVisable(bool state) {
		if (state != isVisable) {
			mesh.SetActive (state);
			isVisable = state;
		}
	}

	#region Cooldown
	void CooldownStart() {
		attackCooldownTime = Mathf.Max (0f, attackCooldownTime - attackCooldown);
	}
	
	void CooldownStep() {
		if (!CanAttack ())
			attackCooldownTime += Time.deltaTime;
	}

	bool CanAttack() {
		return attackCooldownTime >= attackCooldown;
	}
	#endregion

	#region Pause methods
	protected override void OnPause () {
		anim.speed = 0;
	}

	protected override void OnUnpause () {
		anim.speed = 1;
	}
	#endregion

}
