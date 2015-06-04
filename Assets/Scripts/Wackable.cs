using UnityEngine;
using System.Collections;

public class Wackable : MonoBehaviour {
	
	[HideInInspector]
	public bool dead; // On wackingArea.CleanUp() all dead instances will be removed
	
	public virtual void Wack(int damage = 1) {}

}
