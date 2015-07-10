using UnityEngine;
using System.Collections;

public class CoinFlyGoal : MonoBehaviour {

	public ParticleSystem particles;
	private GameController gameController;
	private Animator anim;

	void Start() {
		gameController = GameController.Get ();
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other) {
		Rigidbody rbody = other.attachedRigidbody;

		if (rbody) {
			CoinController coin = rbody.GetComponent<CoinController>();
			if (coin) {
				Destroy(coin.gameObject);
				gameController.AddCoins(1);
				particles.Play();
				anim.SetTrigger("Highlight");
			} // if
		} // if
	}// OnTriggerEnter

}