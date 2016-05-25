using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour {
	public AudioClip voiceIntroToChallenge5;
	public AudioClip voiceSpirit;
	private AudioSource voiceFileIntroToChallenge5;

	void Start () {
		if(ApplicationModel.restartChallenge) {
			SoundActivate();
		}
		else
			voiceFileIntroToChallenge5.PlayOneShot(voiceIntroToChallenge5, 0.5f);
	}

	void Awake () {
		voiceFileIntroToChallenge5 = GetComponent<AudioSource> ();
	}

	public void SoundActivate () {
		voiceFileIntroToChallenge5.PlayOneShot(voiceSpirit, 0.5f);
	}

	public void Restart () {
		ApplicationModel.restartChallenge = true;
		SceneManager.LoadScene ("Draw"); 
	}
}


