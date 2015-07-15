using UnityEngine;
using System.Collections;

public class WackableMenuBG : Wackable {

	private GameController gameController;

	public override void Init() {
		gameController = FindObjectOfType<GameController> ();
	}

	public override void Wack (int damage = 1) {
		gameController.Deselect ();
	}

}
