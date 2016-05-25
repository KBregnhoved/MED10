/*
 
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
   
    // > receive
    // 127.0.0.1 : 8051
   
    // send
    // nc -u 127.0.0.1 8051

UDP part of the code retrieved from: http://forum.unity3d.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/
 
*/
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UDPReceive : MonoBehaviour {
	// All our variables:

	// receiving Thread
	Thread receiveThread;

	// udpclient object
	public UdpClient client;

	// public string IP = "127.0.0.1"; default local
	public int port; // define > init

	// infos
	public string lastReceivedUDPPacket="";
	//public string allReceivedUDPPackets=""; // clean up this from time to time!

	public List<Vector3> handPos = new List<Vector3>();
	private Vector3 secondTempHandPos;
	private string[] tempHandPos;
	public float zVal;



	// start from shell
	private static void Main()
	{
		UDPReceive receiveObj=new UDPReceive();
		receiveObj.init();

		string text="";
		do
		{
			text = Console.ReadLine();
		}
		while(!text.Equals("exit"));
	}

	// start from unity3d
	public void Start()
	{
		// Figure out where to place SoundActivate so it works as it should later.
		//SoundActivate();
		handPos = new List<Vector3>();
		init();
	}

	// OnGUI
	// OBS: should be outcommented when we have the challenge working:
	/*void OnGUI()
	{
		Rect rectObj=new Rect(40,10,200,400);
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.UpperLeft;
		GUI.Box(rectObj,"# UDPReceive\n127.0.0.1 "+port+" #\n"
			+ "shell> nc -u 127.0.0.1 : "+port+" \n"
			+ "\nLast Packet: \n"+ lastReceivedUDPPacket
			,style);
	}*/

	// init, called from Start();
	private void init()
	{
		// Prints that we initialize UDP
		print("UDPSend.init()");

		// define port
		port = 6100;

		// status
		print("Sending to 127.0.0.1 : "+port);
		print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");

		// Creates a new thread such that we can receive stuff:
		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}

	// receive thread
	private  void ReceiveData()
	{
		client = new UdpClient(port); //Creates a new client at the specified port (6100):
		// continous while-loop for us to receive the UDP stuff:
		while (true)
		{
			try
			{
				// Received Bytes
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
				byte[] data = client.Receive(ref anyIP);

				// Bytes encode the UTF8 encoding in the text format.
				string text = Encoding.UTF8.GetString(data);

				// latest UDPpacket
				lastReceivedUDPPacket=text;

				//Splitting the received UDP into an array, to be put into a Vector3 afterwards
				tempHandPos = lastReceivedUDPPacket.Split(';'); // I would like to say that it can max split into 3, but I can't make it work

				//For checking if we split them correctly
				//for (int a = 0; a<3;a++){
 				//print(">> " + tempHandPos[a]);
				//}

				//Adding the last received UDP-packet to a Vector3
				var fmt = new NumberFormatInfo();
				fmt.NegativeSign = "-";
				secondTempHandPos.x = (float)double.Parse(tempHandPos[0], fmt)*(-1);
				secondTempHandPos.y = (float)double.Parse(tempHandPos[1], fmt);
				secondTempHandPos.z = (float)double.Parse(tempHandPos[2], fmt);
				zVal = secondTempHandPos.z; // Storing the z-value of the received package

				// Adds the Vector3, which contains the latest received UDP package, into a Vector3 List:
				handPos.Add(new Vector3(secondTempHandPos.x, secondTempHandPos.y, 0));

			}
			catch (Exception err)
			{
				print(err.ToString());
			}
		}
	}
		
	/*public void SoundActivate () {
		soundFileHandDetected.PlayOneShot(soundHandDetected, 0.5f);
	}*/
	
	void OnDisable() 
	{ 
		if (receiveThread!= null) 
			receiveThread.Abort(); 
		try {
		client.Close();
		}
		catch(System.Exception e){
			Debug.Log("error " + e);
		}
	}

	/*void OnApplicationQuit () {

		if (receiveThread != null)
			receiveThread.Abort();

		client.Close();
	}*/
		
}
