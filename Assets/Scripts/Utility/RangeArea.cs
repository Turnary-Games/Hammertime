using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeArea : MonoBehaviour {

	public Side lookFor;
	private List<Minion> insideTrigger = new List<Minion>();
	
	void OnTriggerEnter(Collider other) {
		Minion minion = null;
		if (other.attachedRigidbody != null)
			minion = other.attachedRigidbody.GetComponent<Minion> ();
		else
			minion = other.GetComponent<Minion> ();
		
		if (minion != null) {
			if (!insideTrigger.Contains (minion) && minion.side == lookFor) {
				insideTrigger.Add (minion);
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		Minion minion = other.GetComponent<Minion>();
		
		if (minion != null) {
			insideTrigger.Remove (minion);
		}
	}

	public void Cleanup() {
		insideTrigger.RemoveAll (delegate(Minion obj) {
			return obj == null;
		});
	}

	public void ForEach(System.Action<Minion> action) {
		insideTrigger.ForEach (action);
	}

	public Minion NearestMinion() {
		Minion nearest = null;
		Cleanup ();

		insideTrigger.ForEach (delegate(Minion minion) {
			if (nearest == null) nearest = minion;
			else
				if (Vector3.Distance (transform.position, minion.transform.position)
				  < Vector3.Distance (transform.position, nearest.transform.position))
					nearest = minion;
		});

		return nearest;
	}
}
