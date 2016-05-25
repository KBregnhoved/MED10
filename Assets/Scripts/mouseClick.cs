using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class mouseClick : MonoBehaviour {

	public Button MyButton {get; private set;}
	public BoxCollider buttonCollider;
	public GameObject bike;
	//UDPReceive udpRec;
	int length = 0;
	Vector3 currentPosOfHand;

	void Awake() {
		MyButton = GetComponent<Button>();
		if (bike == null)
			bike = GameObject.Find("BikeAnim");
		//udpRec = GetComponent<UDPReceive>();
		if (buttonCollider == null){
			buttonCollider = GetComponent<BoxCollider>();
		}
	}

	// Update is called once per frame
	void Update () {
		length = bike.GetComponent<UDPReceive>().handPos.Count;//udpRec.handPos.Count;
		if (length > 0){
			currentPosOfHand = new Vector3(bike.GetComponent<UDPReceive>().handPos[length-1].x, bike.GetComponent<UDPReceive>().handPos[length-1].y, 0);
			if (buttonCollider.bounds.Contains(currentPosOfHand)) {
				MyButton.onClick.Invoke();
			}
		}
	}
}