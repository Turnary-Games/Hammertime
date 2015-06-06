using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject explosionPrefab;

	void Awake() {
		Explosion.prefab = explosionPrefab;
	}

}
