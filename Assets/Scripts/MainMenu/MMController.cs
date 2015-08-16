using UnityEngine;
using System.Collections;

public class MMController : MonoBehaviour {

	public Camera cam;
	public Animator anim;
	public LayerMask raycastLayer;
	private MMButton button;

	void Update () {
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, raycastLayer)) {
			MMButton btn = hit.collider.gameObject.GetComponent<MMButton> ();
			if (btn != null && !btn.disabled) {
				button = btn;
				button.down = Input.GetMouseButton (0);
				button.hover = true;
				button.UpdateAnim();

				if (Input.GetMouseButtonDown(0)) {
					button.OnButtonDown(this);
				}
			}
		} else {
			button = null;
		}

		foreach (MMButton btn in FindObjectsOfType<MMButton>()) {
			if (btn != button) {
				btn.down = false;
				btn.hover = false;
				btn.UpdateAnim();
			}
		}
	}

	public void ButtonIntro(string matchingName) {
		foreach (MMButton btn in FindObjectsOfType<MMButton>()) {
			if (btn.name == matchingName) {
				btn.Intro();
				print ("Intro for "+btn.name);
			}
		}
	}

	public void ButtonOutro(string matchingName) {
		foreach (MMButton btn in FindObjectsOfType<MMButton>()) {
			if (btn.name == matchingName) {
				btn.Outro();
				print ("Outro for "+btn.name);
			}
		}
	}

	public void SetRaycastLayer(string layerName) {
		raycastLayer = LayerMask.GetMask (layerName);
	}

	public void TransitionMainToOption() {
		anim.SetTrigger ("MainToOptions");
	}
	public void TransitionOptionsToMain() {
		anim.SetTrigger ("OptionsToMain");
	}
	public void TransitionMainToCredits() {
		anim.SetTrigger ("MainToCredits");
	}
	public void TransitionCreditsToMain() {
		anim.SetTrigger ("CreditsToMain");
	}


}

