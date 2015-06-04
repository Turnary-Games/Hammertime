using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeArea : MonoBehaviour {

	//[HideInInspector]
	public List<Minion> insideTrigger;
	private Minion parent;

	void Start() {
		parent = GetComponentInParent<Minion> ();
	}
	
	void OnTriggerEnter(Collider other) {
		Minion minion = null;
		if (other.attachedRigidbody != null)
			minion = other.attachedRigidbody.GetComponent<Minion> ();
		else
			minion = other.GetComponent<Minion> ();
		
		if (minion != null) {
			if (!insideTrigger.Contains (minion) && minion.side != parent.side) {
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

	public Minion NearestMinion() {
		Minion nearest = null;

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
