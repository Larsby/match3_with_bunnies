using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFallingPlaySound : MonoBehaviour {
	Vector3 previous_position;
	static int anyoneFlying;
	SoundClipContainer scc = null;
	bool init = false;
	bool reported = false;
	bool beenFalling = false;
	// Use this for initialization
	void Start () {
		previous_position = transform.position;
		Init();
	}
	bool Init() {
		if (init) return true;
		GameObject me = GameObject.FindWithTag("SoundContainer");
	
		if(me != null) {
			scc = me.GetComponent<SoundClipContainer>();
			if (scc == null)
				return false;
			init = true;
			return true;
		}
		return true;
	}
	// Update is called once per frame
	void Update () {
		float travel_distance = transform.position.y - previous_position.y;

		if(travel_distance>0.002f) {
			
			if(!beenFalling && Init()) {
				beenFalling = true;
				anyoneFlying++;
				scc.ReportFalling(anyoneFlying);
			}
			reported = false;
			beenFalling = true;
	
		} 
		if (travel_distance == 0.0f && reported == false)
		{
			anyoneFlying--;
			if (anyoneFlying < 0)
				anyoneFlying = 0;
			if(beenFalling) {
				beenFalling = false;
				if (Init())
				{
					scc.StopFallingSound();
				}	
			}
			reported = true;
		}

		previous_position = transform.position;
  	}
}
