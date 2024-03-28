using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClipContainer : MonoBehaviour {
	public  AudioClip[] movingSounds;
	public AudioClip[] fallingSounds;
	public AudioClip[] touchingSounds;
	private bool playingFalling = false;
	private int  clipPlayCount = 0;
	public  AudioClip[] GetMovingSounds() {
		return movingSounds;
	}
	public AudioClip[] GetFallingSounds()
	{
		return fallingSounds;
	}

	public AudioClip[] GetTouchingSounds()
	{
		return touchingSounds;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void StopFallingSound()
	{
		clipPlayCount--;
	//	Debug.Log("StopFalling!!"+clipPlayCount);
	}

	public void EndedPlaying() {
		playingFalling = false;
	}

	public void ReportFalling(int currentFallingObjects)
	{
		clipPlayCount++;
		if(playingFalling == false) {
			AudioSource audiosrc = GetComponent<AudioSource>();
			audiosrc.clip = fallingSounds[Random.Range(0, fallingSounds.Length)];
			playingFalling = true;
			audiosrc.Play();
			Invoke("EndedPlaying", audiosrc.clip.length);
		}

	}
}
