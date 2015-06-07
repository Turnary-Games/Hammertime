using UnityEngine;
using System.Collections;

public class RuneController : MonoBehaviour {

	// Called from WackableRune.Wack()
	public void SpawnMinion() {
		print ("Spawn a minion @ " + gameObject.name);
	}

}
