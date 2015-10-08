using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageIndicator : Pausable {

	[Header("Variables (DONT ALTER)")]
	public Text text;
	public RectTransform indicator;
	[HideInInspector]
	public RectTransform canvas;

	[Header("Settings")]
	public float lifetime = 1;
	public AnimationCurve movement;
	public float multiplier = 1;

	private Vector2 startPos;
	private float time;

	void Start() {
		startPos = indicator.anchoredPosition;
	}

	void Update() {
		if (paused)
			return;

		// Update time!
		time += Time.deltaTime;
		float p = Mathf.Clamp(time / lifetime, 0f, 1f);

		// Update color
		text.color = new Color(text.color.r, text.color.g, text.color.b, 1-p);

		// Update position
		indicator.anchoredPosition = startPos + Vector2.up * movement.Evaluate(p) * multiplier;

		if (p == 1f) {
			// Time's up
			Destroy(gameObject);
		}
	}

}
