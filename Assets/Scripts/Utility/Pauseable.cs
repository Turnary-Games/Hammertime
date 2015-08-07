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
	
	#region Paused methods
	protected virtual void OnPause() {}
	protected virtual void OnUnpause() {}
	#endregion
	
}

