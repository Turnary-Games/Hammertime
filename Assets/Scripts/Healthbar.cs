using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour {
	
	public Living living;
	public Slider slider;

	public Vector2 size;
	public Vector2 offset;

	private RectTransform canvas;
	private RectTransform healthbar;

	void Start() {
		healthbar = GetComponent<RectTransform> ();
		canvas = healthbar.parent.GetComponent<RectTransform> ();

		if (living != null) {
			slider.maxValue = living.health;
			slider.value = living.health;
		} else
			Debug.LogError ("Please make sure the living variable gets assigned!");
	}

	void Update() {

		if (living == null)
			// My target died D:
			Destroy (gameObject);
		else {

			// Set position to be @ the minion
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, living.transform.position);
			healthbar.anchoredPosition = screenPoint - canvas.sizeDelta / 2f; 

			// Set size
			healthbar.sizeDelta = new Vector2 (canvas.sizeDelta.x * size.x / 10, canvas.sizeDelta.y * size.y / 10);
			healthbar.anchoredPosition += new Vector2 (canvas.sizeDelta.x * offset.x / 10, canvas.sizeDelta.y * offset.y / 10);

			// set slider value
			slider.value = living.health;
		}
	}

}
