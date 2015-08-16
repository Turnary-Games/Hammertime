using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fading : MonoBehaviour {

	public Animator anim;
	public bool autoFadeIn;
	public float fadeTime = 1;

	private System.Action fadeInAction;
	private System.Action fadeOutAction;

	private static List<Fading> scripts = new List<Fading>();

	void Start() {
		scripts.Add (this);
		if (autoFadeIn)
			FadeIn ();
	}
	
	public void FadeIn(System.Action onFadeComplete = null) {
		if (anim != null) {
			fadeInAction = onFadeComplete;
			anim.speed = 1/fadeTime;
			anim.SetTrigger ("Fade in");
		}
	}

	public void FadeOut(System.Action onFadeComplete = null) {
		if (anim != null) {
			fadeOutAction = onFadeComplete;
			anim.speed = 1/fadeTime;
			anim.SetTrigger ("Fade out");
		}
	}

	void OnFadeOutComplete() {
		if (fadeOutAction != null)
			fadeOutAction ();
	}

	void OnFadeInComplete() {
		if (fadeInAction != null)
			fadeInAction ();
	}

	public static void FadeInAll(System.Action onFadeComplete = null) {
		scripts.ForEach (delegate(Fading obj) {
			obj.FadeIn (onFadeComplete);
		});
	}

	public static void FadeOutAll(System.Action onFadeComplete = null) {
		scripts.ForEach (delegate(Fading obj) {
			obj.FadeOut (onFadeComplete);
		});
	}

}
