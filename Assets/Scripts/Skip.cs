using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour {
	bool playSound = false;
	public AudioClip voiceGuide;
	private AudioSource voiceFile;
	stopSounds stopAllSounds = new stopSounds();

	// Use this for initialization
	void Awake () {
		voiceFile = GetComponent<AudioSource>();
	}

	public void SoundActivate () {
		stopAllSounds.StopAllAudio();
		voiceFile.PlayOneShot(voiceGuide, 0.5f);
	}
		
}