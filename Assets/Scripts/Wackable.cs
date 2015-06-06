using UnityEngine;
using System.Collections;

public class Wackable : MonoBehaviour {

	public Animator anim;
	[HideInInspector]
	public bool dead; // On wackingArea.CleanUp() all dead instances will be removed
	
	public virtual void Wack(int damage = 1) {}

	void Update () {
		anim.SetBool ("Selected", FindObjectOfType<HammerController> ().wackingTarget == this);
	}

}
