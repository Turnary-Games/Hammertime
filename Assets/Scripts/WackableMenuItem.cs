using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MenuItem))]
public class WackableMenuItem : Wackable {

	private MenuItem menuItem;
	private HammerController hammer;
	private GameController gameController;
	
	void Start() {
		menuItem = GetComponent<MenuItem> ();
		hammer = FindObjectOfType<HammerController> ();
		gameController = FindObjectOfType<GameController> ();
	}
	
	public override void Wack(int damage = 1) {
		if (gameController.coins >= menuItem.price)
			menuItem.Select ();
	}

	void Update () {
		if (anim != null) {
			anim.SetBool ("Hover", hammer.wackingTarget == this);
			anim.SetBool ("Selected", menuItem.IsSelected ());
			anim.SetBool("Too Pricy", gameController.coins < menuItem.price);
		}
	}

}
