using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WackingArea : MonoBehaviour {

	//[HideInInspector]
	public List<Wackable> insideTrigger;

	void OnTriggerEnter(Collider other) {

		Wackable wack = other.GetComponent<Wackable>();

		if (wack != null) {
			if (!insideTrigger.Contains (wack)) {
				insideTrigger.Add (wack);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		Wackable wack = other.GetComponent<Wackable>();

		if (wack != null) {
			if (insideTrigger.Contains (wack)) {
				insideTrigger.Remove (wack);
			}
		}
	}

	public void Cleanup() {
		insideTrigger.RemoveAll (IsDead);
	}

	static bool IsDead(Wackable item) {
		return item.dead;
	}
}
