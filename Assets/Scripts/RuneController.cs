using UnityEngine;
using System.Collections;

public class RuneController : Pausable {

	[Header("Variables (DONT ALTER)")]

	public ParticleSystem partSystem;
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
		trigger.Cleanup();
		trigger.ForEach (delegate(Minion obj) {
			obj.Arrive();
		});
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
