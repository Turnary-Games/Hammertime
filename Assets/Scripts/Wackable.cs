using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Minion))]
public class Wackable : MonoBehaviour {
	
	[HideInInspector]
	public bool dead;
	private Minion minion;

	void Start() {
		minion = GetComponent<Minion> ();
	}

	public void Wack(int damage = 1) {
		dead = minion.Damage (damage) ? true : dead;
	}

}
