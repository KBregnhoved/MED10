using UnityEngine;
using System.Collections;

public class followHand : MonoBehaviour {

	public UDPReceive udpRec2;
	int amount;

	// Use this for initialization
	void Start () {
		udpRec2 = GetComponent<UDPReceive>();
	}
	
	// Update is called once per frame
	void Update () {
		//if (udpRec2.handPos.Count != 0){
		//amount = udpRec2.handPos.Count;
		//if (amount != 0)
		//this.transform.position = new Vector3(udpRec2.handPos[amount].x, udpRec2.handPos[amount].y, 0);
		//this.transform.position = new Vector3(200, 80, 0);
		//}
	}
}
