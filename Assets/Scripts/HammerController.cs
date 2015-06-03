using UnityEngine;
using System.Collections;

public class HammerController : MonoBehaviour {

	public LayerMask raycastMask;
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
			anim.SetTrigger("Swing");
		}

		point = hit.point;
	}

}
