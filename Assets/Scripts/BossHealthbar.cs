using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthbar : Pausable {

	[Header("Variables (DONT ALTER)")]

	public TextMesh currentHealthMesh;
	public TextMesh maxHealthMesh;
	public Transform bar;
	public Image flashEffect;

	[Header("Settings")]

	public Side side;
	public float flashTime = 0.5f;
	public float maxAlpha = 100f;

	// Used in calculating the scale of the bar
	private int maxHealth;
	private float flash;

	public void SetHealth(int health) {
		if (currentHealthMesh != null) {
			currentHealthMesh.text = health.ToString();

			// Change bar size
			if (maxHealth != 0) {
				float p = Mathf.Clamp(health / (float)maxHealth, 0f, 1f);
				bar.localScale = new Vector3(p, 1f, 1f);
			}

			// Flash effect
			Flash();
		}
	}

	public void InitHealthbar(int current, int max) {
		bar.localScale = new Vector3(0f, 1f, 1f);
		if (maxHealthMesh != null) {
			maxHealthMesh.text = max.ToString();
			maxHealth = max;
		}
		SetHealth(current);
		flash = 0;
	}

	public void Flash() {
		flash = flashTime;
	}

	void Update() {
		if (paused)
			return;

		if (flashTime == 0)
			return;

		flash = Mathf.Max(flash - Time.deltaTime, 0f);

		Color color = flashEffect.color;
		color.a = (maxAlpha / 255f) * (flash / flashTime);
        flashEffect.color = color;
		
	}
	
}
