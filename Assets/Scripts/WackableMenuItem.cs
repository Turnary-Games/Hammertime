using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MenuItem))]
public class WackableMenuItem : Wackable {

	private MenuItem menuItem;
	
	void Start() {
		menuItem = GetComponent<MenuItem> ();
	}
	
	public override void Wack(int damage = 1) {
		menuItem.Select ();
	}

}
