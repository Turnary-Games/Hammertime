using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour {

	public GameObject prefab;
	public int price = 1;

	public void Select() {
		FindObjectOfType<GameController> ().Select (this);
	}

}
