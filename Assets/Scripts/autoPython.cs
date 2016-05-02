using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class autoPython : MonoBehaviour {

	Socket sendSocket;
	EndPoint sendEndPoint;

	IPEndPoint inremoteEndPoint;
	UdpClient inclient;

	public int sport;
	public int rport;

	double scenario;
	double prevScenario;

	public float droneScale;
	public float height;

	public Vector3 startPosnA;
	public Vector3 startPosnB;
	public Vector3 startPosnC;

	AudioThrust propellers;
	Kinematics movement;

	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		scenario = 0;
		prevScenario = -1;

		// UDP Send Port
		sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		IPAddress sendTo = IPAddress.Parse("127.0.0.1");
		sendEndPoint = new IPEndPoint(sendTo, sport);
		sendSocket.SendBufferSize = 0;

		// UDP Receive Port
		inremoteEndPoint = new IPEndPoint (IPAddress.Any, rport);
		inclient = new UdpClient (rport);
		inclient.Client.ReceiveBufferSize = 8;

		propellers = transform.GetComponent<AudioThrust> ();
		movement = transform.GetComponent<Kinematics> ();

		mainCamera = GameObject.Find ("Camera");
	}
	
	// Update is called once per frame
	void Update () {

		if (inclient.Available > 0) {

			byte[] data = inclient.Receive (ref inremoteEndPoint);

			scenario = System.BitConverter.ToDouble (data, 0);
		}

		Debug.Log (scenario);

		if (scenario != prevScenario) {
			prevScenario = scenario;
			if (scenario == 0) {
				//Switch off everything
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.zero;
				propellers.MuteAll ();
				movement.move = false;
			} 
			else {
				if (scenario == 1) {
					//Posn A
					transform.localPosition = startPosnA;
				} else if (scenario == 2) {
					//Posn B
					transform.localPosition = startPosnB;
				} else if (scenario == 3) {
					//Posn C
					transform.localPosition = startPosnC;
				} 

				transform.localScale = droneScale*Vector3.one;
				propellers.UnmuteAll ();
				movement.move = true;
			}
	
		}

		byte[] outdata = new byte[24];

		byte[] vxout = new byte[4];
		byte[] vyout = new byte[4];
		byte[] vzout = new byte[4];

		byte[] dxout = new byte[4];
		byte[] dyout = new byte[4];
		byte[] dzout = new byte[4];

		vxout = System.BitConverter.GetBytes(movement.vx);
		vyout = System.BitConverter.GetBytes(movement.vy);
		vzout = System.BitConverter.GetBytes(movement.vz);

		dxout = System.BitConverter.GetBytes(transform.localPosition.x - mainCamera.transform.localPosition.x);
		dyout = System.BitConverter.GetBytes(transform.localPosition.z - mainCamera.transform.localPosition.z);
		dzout = System.BitConverter.GetBytes(transform.localPosition.y - mainCamera.transform.localPosition.y);

		System.Buffer.BlockCopy(vxout,0,outdata,0,4);
		System.Buffer.BlockCopy(vyout,0,outdata,4,4);
		System.Buffer.BlockCopy(vzout,0,outdata,8,4);
		System.Buffer.BlockCopy(dxout,0,outdata,12,4);
		System.Buffer.BlockCopy(dyout,0,outdata,16,4);
		System.Buffer.BlockCopy(dzout,0,outdata,20,4);

		sendSocket.SendTo(outdata, sendEndPoint);

	}
}
