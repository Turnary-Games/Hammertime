using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

	public List<GameObject> items = new List<GameObject>();
	public float yDifferance;

	void Start() {
		// Spawn items
		for (int i=0; i<items.Count; i++) {
			GameObject clone = Instantiate(items[i],Vector3.down*yDifferance*i,Quaternion.identity) as GameObject;
			clone.transform.SetParent(transform,false);
		}
	}

}
