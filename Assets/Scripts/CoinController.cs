using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	public float speed;
	public float fullSpeedAfter; // seconds
	public float delay;

	private float flyTime;
	private bool fly = false;

	private CoinFlyGoal goal;
	private Rigidbody rbody;

	void Start() {
		goal = FindObjectOfType<CoinFlyGoal> ();
		rbody = GetComponent<Rigidbody> ();
		Invoke ("StartFlying", delay);
	}

	void StartFlying() {
		fly = true;
		flyTime = Time.time;
	}

	void Update() {
		if (fly) {
			float p = Mathf.Min((Time.time - flyTime) / fullSpeedAfter, 1.0f);
			Vector3 delta = goal.transform.position - transform.position;

			Vector3 force = delta.normalized*speed*p;
			force.y *= 2;
			rbody.AddForce(force);
		}
	}

}
