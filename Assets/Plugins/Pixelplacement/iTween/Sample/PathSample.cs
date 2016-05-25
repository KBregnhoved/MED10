// OBS: Remember to place besides the UDPReceive Script for it to work!

/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathSample : MonoBehaviour {
	// All variables:
	private List<Vector3> path = new List<Vector3>();
	float currentPosition = 0.0f;
	float percentSec = 0.01f;
	bool skip = false;
	bool pathFinished = false;
	bool pathDone = false;
	Vector3 prevPoint = new Vector3(0,0,0);
	int amount = 0;
	int amountMax = 99;
	public UDPReceive udpRec;
	private SphereCollider start;
	private SphereCollider end;
	private bool wereInsideStart = false;
	private Vector3 currentPosOfHand;
	private Transform follower;

	// When we have made the path (when pathDone=true), it should draw the path
	void OnDrawGizmos () {
		if (pathDone){
			iTween.DrawPath(path.ToArray(), Color.red);
			pathFinished = true;
		}
	}

	void Start() {
		// We initialize our variables that we receive from other gameobjects/scripts:
		udpRec = GetComponent<UDPReceive>();
		start = GameObject.Find("Start").gameObject.GetComponent<SphereCollider>();
		end = GameObject.Find("End").gameObject.GetComponent<SphereCollider>();
		follower = GameObject.Find("followHand").gameObject.GetComponent<Transform>();
	}

	void Update () {
		// As long as skip is not true, we will try and draw the route.
		// OBS: if (!skip) can maybe be deleted when skip is fully implmented (if it is made as a scene shift)?
		if (!skip){
			// When the path is finished and has been drawn, we will make the cube follow the line's position:
			if (pathFinished) {
				if (currentPosition < 0.1) {
					currentPosition += percentSec * Time.deltaTime;
				} else {
					currentPosition += percentSec * Time.deltaTime;
				}
				if (currentPosition > 1.0) {
					currentPosition = 0.0f;
				}		
				iTween.PutOnPath (gameObject, path.ToArray(), currentPosition);
			}
			// Path finished, path done both ensures that we will only add hand position when the path is not finished
			// and that we will not start the animation until the path is done.
			// As long as path is not finished and done and that the hand is X amount close to the Kinect, this happens:
			else if (!pathFinished && !pathDone && udpRec.zVal < 600){
				for (int i = 0; i<udpRec.handPos.Count; i++){
					currentPosOfHand = new Vector3(udpRec.handPos[i].x, udpRec.handPos[i].y, 0);
					follower.position = currentPosOfHand;
					if (end.GetComponent<SphereCollider>().bounds.Contains(currentPosOfHand) && wereInsideStart){
						pathDone = true;
						print("Farm reached");
					}
					else if (start.GetComponent<SphereCollider>().bounds.Contains(currentPosOfHand) || wereInsideStart){
						wereInsideStart = true;
						print("Start reached");
						if(prevPoint != udpRec.handPos[i]){
							if (amount > amountMax){
								path.Add(new Vector3(udpRec.handPos[i].x, udpRec.handPos[i].y, 0));
								prevPoint = udpRec.handPos[i];
								amount = 0;
							}
							else {
								amount++;
							}
						}
					}
				}
			}
			else if (1<path.Count){
				pathDone = true;
			}
		}
	}

	public void ButtonSkip () {
		skip = true;	
	}
}*/