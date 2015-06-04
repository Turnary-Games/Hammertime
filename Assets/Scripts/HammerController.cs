using UnityEngine;
using System.Collections;

public class HammerController : MonoBehaviour {

	public LayerMask raycastMask;
	public WackingArea wackingArea;
	[Space(12)]
	public int damage = 1;
	public float attackCooldown;

	private bool canAttack = true;
	private Vector3 point;
	private Animator anim;

	void Start() {
		anim = GetComponent<Animator> ();
	}

	void Update () {
		Raycast ();
		
		transform.position = point;
	}

	void Raycast() {
		// Variables
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// Cast the ray
		if(Physics.Raycast (ray, out hit, Mathf.Infinity, raycastMask)) {
			HandleHit(hit);
		}
	}

	void HandleHit(RaycastHit hit) {
		if (Input.GetMouseButtonDown (0)) {
			Punch ();
		}

		if (hit.collider.tag == "Skull projectile") {
			point = hit.collider.transform.position + Vector3.up * hit.collider.bounds.size.y/2;
		} else {
			point = hit.point;
		}
	}

	void Punch() {
		if (canAttack) {
			Wack ();
			anim.SetTrigger("Swing");
			StartCoroutine(AttackCooldown());
		}
	}

	IEnumerator AttackCooldown() {
		canAttack = false;
		yield return new WaitForSeconds (attackCooldown);
		canAttack = true;
	}

	void Wack() {
		foreach (Wackable wack in wackingArea.insideTrigger) {
			wack.Wack ();
		}
		wackingArea.Cleanup();
	}

}
