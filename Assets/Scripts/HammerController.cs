using UnityEngine;
using System.Collections;

public class HammerController : MonoBehaviour {

	public LayerMask raycastPlainMask;
	public LayerMask raycastObstacleMask;
	public ParticleSystem thump;
	public GameObject mesh;
	[Space(12)]
	public int damage = 1;
	public float attackCooldown;

	private bool canAttack = true;
	private bool visable = true;
	private bool isVisable = true;
	private Vector3 startPoint;
	private Vector3 point;
	private Animator anim;

	[HideInInspector] public Wackable wackingTarget;

	void Start() {
		anim = GetComponent<Animator> ();
		startPoint = transform.position;
	}

	void Update () {
		Raycast ();
		
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
			point = obj.transform.position;
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
		if (canAttack) {
			if (isVisable) {
				anim.SetTrigger("Swing");
				StartCoroutine(AttackCooldown());
			} else {
				Wack ();
			}
		}
	}

	IEnumerator AttackCooldown() {
		canAttack = false;
		yield return new WaitForSeconds (attackCooldown);
		canAttack = true;
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

}
