using UnityEngine;
using System.Collections;

public class Wackable : MonoBehaviour {

	[Tooltip("Animator component for /Selected/ and /Unselected/ animation clips")]
	public Animator anim;
	[Tooltip("[Optional] If set then hammer will focus on this position instead of this gameobjects position")]
	public Transform customPivot;

	[HideInInspector]
	public HammerController hammer;
	
	public virtual void Wack(int damage = 1) {}
	public virtual void Init() {}

	void Start() {
		hammer = FindObjectOfType<HammerController> ();

		Init ();
	}


	protected virtual void Update () {
		if (anim != null && hammer != null)
			anim.SetBool ("Selected", hammer.wackingTarget == this);
		else
			Debug.LogError ("Please make sure the animator and hammer variables are getting their values. ("+ name +")");
	}

}
