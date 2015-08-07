using UnityEngine;
using System.Collections;

public class Pausable : MonoBehaviour {
	
	private bool _paused;
	public bool paused {
		get {
			return _paused;
		}
		set {
			bool state = _paused;
			_paused = value;
			
			// Trigger events
			if (state == true && value == false)
				OnUnpause();
			if (state == false && value == true)
				OnPause();
		}
	}
	
	#region Virtual methods/Events
	protected virtual void OnPause() {}
	protected virtual void OnUnpause() {}
	#endregion

	#region Static methods
	public static void PauseAll() {
		foreach (Pausable obj in FindObjectsOfType<Pausable>()) {
			obj.paused = true;
		}
	}

	public static void PauseAll<T> () where T : Pausable {
		foreach (T obj in FindObjectsOfType<T>()) {
			obj.paused = true;
		}
	}

	public static void PauseAll<T> (System.Predicate<T> match) where T : Pausable {
		foreach (T obj in FindObjectsOfType<T>()) {
			if (match(obj)) {
				obj.paused = true;
			}
		}
	}

	public static void UnpauseAll() {
		foreach (Pausable obj in FindObjectsOfType<Pausable>()) {
			obj.paused = false;
		}
	}

	public static void UnpauseAll<T> () where T : Pausable {
		foreach (T obj in FindObjectsOfType<T>()) {
			obj.paused = false;
		}
	}

	public static void UnpauseAll<T> (System.Predicate<T> match) where T : Pausable {
		foreach (T obj in FindObjectsOfType<T>()) {
			if (match(obj)) {
				obj.paused = false;
			}
		}
	}
	#endregion
	
}

