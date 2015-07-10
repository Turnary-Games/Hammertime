using UnityEngine;
using System.Collections;

public class WackableMenuBG : Wackable {

	private GameController gameController;

	void Start() {
		gameController = FindObjectOfType<GameController> ();
	}

	public override void Wack (int damage = 1) {
		gameController.Deselect ();
	}

}
