using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
	public AudioClip[] moving;
	public AudioClip[] falling;
	public AudioClip[] touching;

	public AudioClip current;
	private AudioSource source;
	private bool init = false;
	public float randMax = 0;
	// Use this for initialization
	void Start ()
	{
		source = GetComponent<AudioSource> ();
		if(source == null) {
			source = gameObject.AddComponent<AudioSource>();
		}

	}
	bool Init() {
		if (init) return true;
		GameObject sound = GameObject.FindWithTag("SoundContainer");
		if(sound) {
			SoundClipContainer scc = sound.GetComponent<SoundClipContainer>();
			moving = scc.GetMovingSounds();
			falling = scc.GetFallingSounds();
			touching = scc.GetTouchingSounds();
			init = true;
			return true;
		}

		return false;
	}

	public void PlayFalling() {
		if (Init() == false)
		{
			return;
		}
		Play(falling);
	}
	public void PlayTouch() {
		if (Init() == false)
		{
			return;
		}
		Play(touching);
	}
	public void PlayMoving() {
		if (Init() == false)
		{
			return;
		}
		Play(moving);
	}
	private void Play (AudioClip[] clips)
	{


		   source.enabled = true;
		if (source.isPlaying) {
			//source.Stop ();

		}
		if(clips == null ||clips.Length == 0) {
			Debug.Log("No clip provided, can't play null so aborting.");
			return;
		}
		source.clip = clips [Random.Range (0, clips.Length)];

		source.Play ();

		source.pitch = Random.Range (1.0f - randMax, 1.0f + randMax);

	}
}
