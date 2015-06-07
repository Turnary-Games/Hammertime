using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

	public Side side;
	public Checkpoint goal;
	public float height = 1;
	public float rotation;

	void OnDrawGizmos() {
		Vector3 from = new Vector3(transform.position.x, height, transform.position.z);
		Vector3 to = from + Quaternion.Euler (0, rotation, 0) * Vector3.forward * 5;

		Gizmos.color = Color.red;

		Gizmos.DrawLine (transform.position, from);
		Gizmos.DrawLine (from, to);
		Gizmos.DrawSphere (to, 0.3f);
	}

	public GameObject Spawn(GameObject prefab) {
		Vector3 pos = new Vector3(transform.position.x, height, transform.position.z);
		Quaternion rot = Quaternion.Euler (0, rotation, 0);

		GameObject clone = Instantiate (prefab, pos, rot) as GameObject;

		Minion minion = clone.GetComponent<Minion> ();
		minion.goal = goal;

		return clone;
	}

}
