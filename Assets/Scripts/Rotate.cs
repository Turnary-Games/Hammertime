using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{
	public float speed; 

	void Update () 
	{
		transform.Rotate (new Vector3 (0, 30, 0) * Time.deltaTime * speed);
	}
}
