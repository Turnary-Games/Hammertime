using UnityEngine;
using System.Collections;

public class Rotate : Pausable 
{
	public Vector3 rotation;
	public Space relativeTo;

	void Update () {
		if (paused)
			return;

		transform.Rotate (Time.deltaTime * rotation, relativeTo);
	}
}
