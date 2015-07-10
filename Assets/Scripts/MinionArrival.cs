using UnityEngine;
using System.Collections;

public class MinionArrival : MonoBehaviour {
	
	public Material goodParticles;
	public Material evilParticles;
	public ParticleSystem particles;
	public ParticleSystemRenderer particleRenderer;

	[HideInInspector]
	public Side side;

	void Start () {
		Material mat = side == Side.ally ? goodParticles : evilParticles;

		particles.startColor = mat.color;
		particleRenderer.material = mat;

		particles.Play ();
		Destroy (gameObject, particles.startLifetime);

		GameController.Get ().Damage (side, 1);
	}
}
