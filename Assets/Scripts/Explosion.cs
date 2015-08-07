using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : Pausable {

	[Header("Variables (DONT ALTER)")]

	public Animator anim;

	[Header("Settings")]

	[Tooltip("This is the default damage.\nIf damage is not specified in the CreateExplosion method then it will use this value.")]
	public int damage;
	
	[HideInInspector]
	public static GameObject prefab;
	[HideInInspector]
	public List<Side> whitelist = new List<Side>();

	private bool canDamage = true;

	void OnTriggerEnter(Collider other) {
		if (canDamage) {
			GameObject obj = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;
			Minion minion = obj.GetComponent<Minion> ();

			if (minion != null && damage > 0 && whitelist.Count > 0)
				Damage (minion);
		}
	}

	void DisableDamage() {
		canDamage = false;
	}
	
	void Damage(Minion minion) {
		if (whitelist.Contains(minion.side))
			minion.Damage (damage);
	}

	// Called from within the animation
	void AnimationStop() {
		Destroy (gameObject);
	}

	#region Pausing
	protected override void OnPause () {
		anim.speed = 0;
	}

	protected override void OnUnpause () {
		anim.speed = 1;
	}
	#endregion

	#region Variations of CreateExplosion
	/// <summary>
	/// Creates an explosion at <paramref name="position"/> dealing <paramref name="damage"/> points to each minion within a radius of <paramref name="radius"/> units.
	/// Will only damage minions of a side in the <paramref name="whitelist"/> list. If the list is left empty then it won't damage anything.
	/// </summary>
	/// <param name="position">Position in the world.</param>
	/// <param name="damage">Damage points dealt to each minion inside the damage radius.</param>
	/// <param name="radius">Radius in units of the explosion size.</param>
	/// <param name="whitelist">Damage whitelist. Will only damage minions of a side in the whitelist. If left empty then it won't damage anything.</param>
	public static void CreateExplosion(Vector3 position, int damage, float radius, params Side[] whitelist) {
		GameObject clone = Instantiate (Explosion.prefab, position, Quaternion.identity) as GameObject;
		Explosion script = clone.GetComponent<Explosion> ();

		script.transform.localScale = Vector3.one * radius / 2;
		script.damage = damage;
		script.whitelist = new List<Side>(whitelist);
	}

	/// <summary>
	/// Creates an explosion at <paramref name="position"/> dealing <paramref name="damage"/> points to each minion within a radius of the prefabs size.
	/// Will only damage minions of a side in the <paramref name="whitelist"/> list. If the list is left empty then it won't damage anything.
	/// </summary>
	/// <param name="position">Position in the world.</param>
	/// <param name="damage">Damage points dealt to each minion inside the damage radius.</param>
	/// <param name="whitelist">Damage whitelist. Will only damage minions of a side in the whitelist. If left empty then it won't damage anything.</param>
	public static void CreateExplosion(Vector3 position, int damage, params Side[] whitelist) {
		GameObject clone = Instantiate (Explosion.prefab, position, Quaternion.identity) as GameObject;
		Explosion script = clone.GetComponent<Explosion> ();

		script.damage = damage;
		script.whitelist = new List<Side>(whitelist);
	}

	/// <summary>
	/// Creates an explosion at <paramref name="position"/> dealing the same damage points as in the prefab to each minion within a radius of the prefabs size.
	/// Will only damage minions of a side in the <paramref name="whitelist"/> list. If the list is left empty then it won't damage anything.
	/// </summary>
	/// <param name="position">Position in the world.</param>
	/// <param name="whitelist">Damage whitelist. Will only damage minions of a side in the whitelist. If left empty then it won't damage anything.</param>
	public static void CreateExplosion(Vector3 position, params Side[] whitelist) {
		GameObject clone = Instantiate (Explosion.prefab, position, Quaternion.identity) as GameObject;
		Explosion script = clone.GetComponent<Explosion> ();

		script.whitelist = new List<Side>(whitelist);
	}
	#endregion
}
