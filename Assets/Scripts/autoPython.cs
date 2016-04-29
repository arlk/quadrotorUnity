using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class autoPython : MonoBehaviour {

	public int rport;
	IPEndPoint inremoteEndPoint;
	UdpClient inclient;

	double scenario;
	double prevScenario;

	public float droneScale;
	public float height;

	public Vector3 startPosnA;
	public Vector3 startPosnB;
	public Vector3 startPosnC;

	AudioThrust propellers;
	Kinematics movement;

	// Use this for initialization
	void Start () {
		scenario = 0;
		prevScenario = -1;

		inremoteEndPoint = new IPEndPoint (IPAddress.Any, rport);
		inclient = new UdpClient (rport);
		inclient.Client.ReceiveBufferSize = 8;

		propellers = transform.GetComponent<AudioThrust> ();
		movement = transform.GetComponent<Kinematics> ();
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
	}
}
