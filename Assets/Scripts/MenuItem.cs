using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour {

	public GameObject prefab;
	public int price = 1;
	public TextMesh priceText;

	private GameController gameController;

	public void Start() {
		priceText.text = price.ToString ();
		gameController = GameController.Get ();
	}

	public void Select() {
		gameController.Select (this);
	}

	public bool IsSelected() {
		return gameController.IsSelected (this);
	}

}
