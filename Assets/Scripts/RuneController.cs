using UnityEngine;
using System.Collections;

public class RuneController : MonoBehaviour {

	private GameController gameController;
	private Spawnpoint spawnpoint;

	void Start() {
		gameController = GameController.Get ();
		spawnpoint = GetComponent<Spawnpoint> ();
	}

	public void Purchase() {
		// Manually spawn a minion with the hammer
		gameController.PurchaseSelected(spawnpoint);
	}

}
