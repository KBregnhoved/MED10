using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]

public class followPath : MonoBehaviour {
	// All variables:
	float currentPosition = 0.0f;
	float percentSec = 0.02f;
	float largeMapMultiplyX = 6.5f;
	float largeMapMultiplyY = 5.9091f;
	int largeMapAddY = 1800;// 1749;
	int largeMapAddX = 1400;//2735;
	bool skip = false;
	bool pathFinished = false;
	bool pathDone = false;
	bool detectSoundHasPlayed;
	bool outOfMap = false;
	bool soundscapeHasStarted = false;
	int zValMax = 3000;//800;
	int length = 0;
	int numberOfPathPoints = 0;
	int handPosMax = 20;
	UDPReceive udpRec;
	SphereCollider start;
	SphereCollider end;
	//SphereCollider startLarge;
	//SphereCollider endLarge;
	GameObject[] pathPoints;
	//GameObject[] pathPointsLarge;
	SphereCollider currentPathPoint;
	//BoxCollider mapCollider;
	Transform follower;
	bool wereInsideStart = false;
	bool[] haveBeenAtPathPoint;
	List<Vector3> path = new List<Vector3>();
	List<Vector3> pathLargeMap = new List<Vector3>();
	stopSounds stopAllSounds = new stopSounds();
	Vector3 currentPosOfHand;
	Vector3 lastPosition = new Vector3(0f,0f,0f);
	Vector3 meanPositionHand = new Vector3(0f,0f,0f);

	public AudioClip soundHandDetected;
	public AudioClip soundscape;
	public AudioClip audioTooLongRoute;
	public AudioClip audioTooShortRoute;
	public AudioClip audioTooLongRouteAlmostThere;
	public AudioClip audioWellDone;
	public AudioClip audioRouteNotFinished;
	public AudioClip detectStart;
	private AudioSource voiceFile;

	private MovieTexture movie;
	private Renderer r;

	public Button skipButton {get; private set;}

	// When we have made the path (when pathDone=true), it should draw the path
	void OnDrawGizmos () {
		if (path.Count>1 && pathLargeMap.Count>1){
			iTween.DrawPath(path.ToArray(), Color.red);
			iTween.DrawPath(pathLargeMap.ToArray(), Color.red);
			if (pathDone){
				pathFinished = true;
				follower.position = new Vector3(0f, 0f, 30f);
				}
		}
	}

	void Start() {
		// We initialize our variables that we receive from other gameobjects/scripts:
		udpRec = GetComponent<UDPReceive>();
		start = GameObject.Find("Start").gameObject.GetComponent<SphereCollider>();
		end = GameObject.Find("End").gameObject.GetComponent<SphereCollider>();
		//startLarge = GameObject.Find("StartLarge").gameObject.GetComponent<SphereCollider>();
		//endLarge = GameObject.Find("EndLarge").gameObject.GetComponent<SphereCollider>();
		follower = GameObject.Find("followHandBike").gameObject.GetComponent<Transform>();
		pathPoints = GameObject.FindGameObjectsWithTag("Path");
		//pathPointsLarge = GameObject.FindGameObjectsWithTag("PathLarge");
		haveBeenAtPathPoint = new bool[pathPoints.Length];
		// get pathPoints GameObject.FindGameObjectsWithTag Gameobject so we can get transform.position as well
		voiceFile= GetComponent<AudioSource>();
		detectSoundHasPlayed = false;

		// Get movie
		r = GetComponent<Renderer>();
		movie = (MovieTexture)r.material.mainTexture;
		// Loops the movie.
		movie.loop = true;

		skipButton = GameObject.Find("Skip").gameObject.GetComponent<Button>();
	}

	void Update () {
		// As long as skip is not true, we will try and draw the route.
		// OBS: if (!skip) can maybe be deleted when skip is fully implmented (if it is made as a scene shift)?
		if (!skip){
			// When the path is finished and has been drawn, we will make the bike animation follow the line's position:
			if (pathFinished) {
				// if the bike animation is not out of the map collider then
					if (currentPosition < 0.1) {
					if (!movie.isPlaying){
							voiceFile.clip = movie.audioClip;
							movie.Play();
							//voiceFile.Play();
							//voiceFile.volume = 0.25f;
						}
						currentPosition += percentSec * Time.deltaTime;
						iTween.PutOnPath(this.gameObject, pathLargeMap.ToArray(), currentPosition);
					} else if (currentPosition > 1.0) {
						currentPosition = 1.1f;
						movie.Stop();
						//voiceFile.Stop();
					} else {
					if (!movie.isPlaying){
						voiceFile.clip = movie.audioClip;
						movie.Play();
						//voiceFile.Play();
						//voiceFile.volume = 0.25f;
					}
						currentPosition += percentSec * Time.deltaTime;
						iTween.PutOnPath(this.gameObject, pathLargeMap.ToArray(), currentPosition);
					}
				// if the bike animation is out of the map collider then:
				if(outOfMap){
					/*if (soundscapeHasStarted){
						currentPosition = 1.1f;
						movie.Stop();
						voiceFile.Stop();
					}*/
					if (!soundscapeHasStarted){
					// Tænker at det er fint nok bare at gøre det oneshot
						voiceFile.PlayOneShot(soundscape, 0.5f);
						soundscapeHasStarted = true;
					}
					if (!voiceFile.isPlaying){
						skipButton.onClick.Invoke();
						skip = true;
					}
				}


			}
			// Path finished, path done both ensures that we will only add hand position when the path is not finished
			// and that we will not start the animation until the path is done.
			// As long as path is not finished and done and that the hand is X amount close to the Kinect, this happens:
			else if (!pathFinished && !pathDone && udpRec.zVal < zValMax && udpRec.handPos.Count>1){
				length = udpRec.handPos.Count;
				currentPosOfHand = new Vector3(udpRec.handPos[length-1].x, udpRec.handPos[length-1].y, 0f);

				// To take the mean position of e.g. 10 positions given via UDP
				for (int i = 0; i<handPosMax+1; i++){
					meanPositionHand = new Vector3(meanPositionHand.x+udpRec.handPos[length-1].x, meanPositionHand.y+udpRec.handPos[length-1].y, 0f);
				}

				// When we have been through the for-loop we take the mean of the 10 entries of the position of the hand given by UDP
				meanPositionHand = meanPositionHand/handPosMax;
				follower.position = meanPositionHand;
				meanPositionHand = new Vector3(0f,0f,0f);
					
				//follower.position = currentPosOfHand;

				if (end.bounds.Contains(currentPosOfHand) && wereInsideStart){
					// Add end coordinates to path
					path.Add(end.transform.position);
					//pathLargeMap.Add(endLarge.transform.position);
					pathLargeMap.Add(new Vector3(end.transform.position.x*largeMapMultiplyX+largeMapAddX, end.transform.position.y*largeMapMultiplyY+largeMapAddY, 200f));
					pathDone = true;
					print("Farm reached");
					stopAllSounds.StopAllAudio();
				
					//If statements about how many of the pathPoints the user went through - determines the type of feedback:
					if (numberOfPathPoints < 2)
						voiceFile.PlayOneShot(audioTooShortRoute, 0.5f);
					else if (numberOfPathPoints > 1 && numberOfPathPoints < 5)
						voiceFile.PlayOneShot(audioWellDone, 0.5f);
					//else if (numberOfPathPoints > 4 && numberOfPathPoints < 4)
					//	voiceFile.PlayOneShot(audioTooLongRouteAlmostThere, 0.5f);
					else if (numberOfPathPoints > 4)
						voiceFile.PlayOneShot(audioTooLongRoute, 0.5f);
				}
				// When we reach the start-collider
				else if (start.bounds.Contains(currentPosOfHand) && !wereInsideStart){
					//print("Start reached");
					voiceFile.PlayOneShot(detectStart, 0.5f);
					//print("position: " + start.transform.position);
					wereInsideStart = true;
					//Add start coordinates to path
					path.Add(start.transform.position);
					//pathLargeMap.Add(startLarge.transform.position);
					pathLargeMap.Add(new Vector3(start.transform.position.x*largeMapMultiplyX+largeMapAddX, start.transform.position.y*largeMapMultiplyY+largeMapAddY, 200f));
				}
				//When we have reached Start, then we can start add things to the path:
				else if (wereInsideStart){
					//print("inside wereInsideStart");
					for (int i = 0; i<pathPoints.Length; i++){
						//print("length of pathPoints: " + pathPoints.Length);
						currentPathPoint = pathPoints[i].GetComponent<SphereCollider>();
						if (currentPathPoint.bounds.Contains(currentPosOfHand) && !haveBeenAtPathPoint[i]){
							print("path Point " + i + " reached");
							path.Add(currentPathPoint.transform.position);
							pathLargeMap.Add(new Vector3(currentPathPoint.transform.position.x*largeMapMultiplyX+largeMapAddX, currentPathPoint.transform.position.y*largeMapMultiplyY+largeMapAddY, 200f));
							haveBeenAtPathPoint[i] = true;
							numberOfPathPoints++;
						}
					}
				}
			}
		//}

			// If none of the above is true, but we do have something in the path, then path is done
			// OBS: Shouldn't we only stop the experience when path is completely done, then?
			else if (udpRec.zVal > zValMax){
				// If only want to stop the experience when the route is done, we can just outcomment these lines:
				if (1<path.Count)
					pathDone = true;
				stopAllSounds.StopAllAudio();
				voiceFile.PlayOneShot(audioRouteNotFinished, 0.5f);
				}
				
			//currentPosOfHand = new Vector3(currentPosOfHand.x, currentPosOfHand.y, udpRec.zVal);

			// Plays the sound when the hand is detected the first time:
			if (lastPosition != null && currentPosOfHand != lastPosition && !detectSoundHasPlayed){
				//print("1 if-sentence");
				detectSoundHasPlayed = true;
				voiceFile.PlayOneShot(soundHandDetected, 0.5f);
			}
			// Testing if it can say the sound the next times that a hand is detected. It does currently not work.
			//else if (lastPosition != null && currentPosOfHand==lastPosition && detectSoundHasPlayed){
			//	detectSoundHasPlayed = false;
				//print("2 if-sentence");
			//}
			//print("Last pos: " + lastPosition + "Current Pos: " + currentPosOfHand);

			lastPosition = currentPosOfHand;
			}
		}

	public void ButtonSkip () {
		skip = true;	
	}


	void OnCollisionExit(Collision col)
	{
		if(col.collider.name == "LargeMap" )
		{
			outOfMap = true;
			//print("collision with: " + col.collider.name);
		}
		print("collision exit with: " + col.collider.name);
	}

}
