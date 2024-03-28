using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchPlaySound : MonoBehaviour {
	private PlayRandomSound sfx = null;
	// Use this for initialization
	void Start () {
		sfx = GetComponent<PlayRandomSound>();
		if(sfx == null) {
			sfx = gameObject.AddComponent<PlayRandomSound>();
		}
	}

	void OnMouseDown()
	{
		sfx.PlayTouch();
	}
	

}
