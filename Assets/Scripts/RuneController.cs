using UnityEngine;
using System.Collections;

public class RuneController : MonoBehaviour {

	public RangeArea trigger;
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

	void Update() {
		trigger.ForEach (delegate(Minion obj) {
			obj.Arrive();
		});
	}

}
