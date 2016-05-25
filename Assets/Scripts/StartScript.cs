using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour {
	//bool playSound = false;
	//public AudioClip voiceGuideStart;
	private AudioSource voiceFile;
	//bool audioHasPlayed = false;
	bool videoHasPlayed = false;

	private MovieTexture movie;
	private Renderer r;

	void Awake () {
		// Get audio
		voiceFile= GetComponent<AudioSource> ();

		// Get movie
		r = GetComponent<Renderer>();
		movie = (MovieTexture)r.material.mainTexture;

		// Loops the movie. Should be outcommented when we have the final animation.
		movie.loop = false;

		// Plays the sound on awake
		if (!Application.isLoadingLevel){
			voiceFile.clip = movie.audioClip;
			//audioHasPlayed = true;
			//voiceFile.PlayOneShot(voiceGuideStart, 0.5f);
			movie.Play();
			voiceFile.Play();
			videoHasPlayed = true;
		}
	}

	void Update(){
		//print(audioHasPlayed);
		// For loading in the next scene:
		//if (!voiceFile.isPlaying && !movie.isPlaying && audioHasPlayed && videoHasPlayed){
		if (!movie.isPlaying && videoHasPlayed){
			//print("HERE");
			SceneManager.LoadScene("Draw"); 
		}
		// For playing the movie
		/*else if (!voiceFile.isPlaying){
			//print("start movie");
			movie.Play();
			videoHasPlayed = true;
		}*/

	}
			
}