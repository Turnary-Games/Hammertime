using UnityEngine;
using System.Collections;

public class RuneController : MonoBehaviour {

	public void Purchase() {
		// Manually spawn a minion with the hammer
		FindObjectOfType<GameController>().PurchaseSelected(GetComponent<Spawnpoint>());
	}

}
