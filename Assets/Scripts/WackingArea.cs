using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WackingArea : MonoBehaviour {

	//[HideInInspector]
	public List<Wackable> insideTrigger;

	void OnTriggerEnter(Collider other) {
		Wackable wack = GetWack (other);

		if (wack != null) {
			if (!insideTrigger.Contains (wack)) {
				insideTrigger.Add (wack);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		Wackable wack = GetWack (other);

		if (wack != null) {
			if (insideTrigger.Contains (wack)) {
				insideTrigger.Remove (wack);
			}
		}
	}

	Wackable GetWack(Collider other) {
		if (other.attachedRigidbody != null)
			return other.attachedRigidbody.GetComponent<Wackable> ();
		else
			return other.GetComponent<Wackable> ();
	}

	public void Cleanup() {
		insideTrigger.RemoveAll (IsDead);
	}

	static bool IsDead(Wackable item) {
		return item.dead;
	}
}
