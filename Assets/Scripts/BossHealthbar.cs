using UnityEngine;
using System.Collections;

public class BossHealthbar : MonoBehaviour {

	[Header("Variables (DONT ALTER)")]

	public TextMesh currentHealthMesh;
	public TextMesh maxHealthMesh;
	public Transform bar;

	[Header("Settings")]

	public Side side;

	// Used in calculating the scale of the bar
	private int maxHealth;

	public void SetHealth(int health) {
		if (currentHealthMesh != null) {
			currentHealthMesh.text = health.ToString();

			if (maxHealth != 0) {
				float p = Mathf.Clamp(health / (float)maxHealth, 0f, 1f);
				bar.localScale = new Vector3(p, 1f, 1f);
			}
		}
	}

	public void InitHealthbar(int current, int max) {
		bar.localScale = new Vector3(0f, 1f, 1f);
		if (maxHealthMesh != null) {
			maxHealthMesh.text = max.ToString();
			maxHealth = max;
		}
		SetHealth(current);
	}
	
}
