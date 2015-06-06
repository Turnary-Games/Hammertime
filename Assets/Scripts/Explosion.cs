using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public static GameObject prefab;

	void AnimationStop() {
		Destroy (gameObject);
	}

	public static void CreateExplosion(Vector3 position) {
		Instantiate (Explosion.prefab, position, Quaternion.identity);
	}

}
