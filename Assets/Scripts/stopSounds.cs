using UnityEngine;
using System.Collections;

public class stopSounds : MonoBehaviour {

	//Stop all sounds
	private AudioSource[] allAudioSources;

	// Use this for initialization
	void Awake () {
		StopAllAudio();
	}
	
	public void StopAllAudio() {
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach(AudioSource audioS in allAudioSources) {
			audioS.Stop();
		}
	}
}
