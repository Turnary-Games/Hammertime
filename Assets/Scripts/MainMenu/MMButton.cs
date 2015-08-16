using UnityEngine;
using System.Collections;

public class MMButton : MonoBehaviour {
	[Header("Variables (DONT ALTER)")]

	public Animator anim;

	[Header("Settings")]
	
	public ButtonAction action;
	public MenuTransition transition;
	public string nextLevelName;
	public bool disabled;

	[HideInInspector]
	public bool hover;
	[HideInInspector]
	public bool down;

	void Start() {
#if UNITY_EDITOR
		disabled = disabled || action == ButtonAction.quit;
#endif

		UpdateAnim ();
	}

	public void UpdateAnim() {
		anim.SetBool("Hover",hover);
		anim.SetBool("Down",down);
		anim.SetBool ("Disabled",disabled);
	}

	public void Intro() {
		anim.SetFloat ("Intro", 1);
	}

	public void Outro() {
		anim.SetFloat ("Intro", -1);
	}

	public void OnButtonDown(MMController controller) {
		switch (action) {
		case ButtonAction.levelChange:
			Fading.FadeOutAll(delegate {
				Application.LoadLevelAsync(nextLevelName);
			});
			break;

		case ButtonAction.quit:
			Application.Quit();
			break;

		case ButtonAction.subMenu:
			if (transition == MenuTransition.creditsToMain) controller.TransitionCreditsToMain();
			if (transition == MenuTransition.mainToCredits) controller.TransitionMainToCredits();
			if (transition == MenuTransition.optionsToMain) controller.TransitionOptionsToMain();
			if (transition == MenuTransition.mainToOptions) controller.TransitionMainToOption();
			break;
		}
	}

	public enum ButtonAction {
		doNothing,levelChange,subMenu,quit
	}
	public enum MenuTransition {
		noTransition,
		mainToOptions,optionsToMain,
		mainToCredits,creditsToMain
	}
}
