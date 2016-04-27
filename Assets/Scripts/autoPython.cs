using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class autoPython : MonoBehaviour {

	public int rport;
	double scenario;
	IPEndPoint inremoteEndPoint;
	UdpClient inclient;
	double prevScenario;
	float DroneSize;
	public float height;
	// Use this for initialization
	void Start () {
		DroneSize = 1f;
		rport = 25003;
		scenario = 0;
		inremoteEndPoint = new IPEndPoint (IPAddress.Any, /*port*/ rport);
		inclient = new UdpClient (rport);
		inclient.Client.ReceiveBufferSize = 8;
		prevScenario = -1;
		height = 2f;
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
				transform.localPosition = new Vector3 (0f, 0f, 0f);
				transform.localScale = new Vector3 (0f, 0f, 0f);
				Audio.quad_sound.mute = true;
			} else {
				if (scenario % 4 == 1) {
					//Posn A
					transform.localPosition = new Vector3 (-17.9f, height, 63.94f);
				} else if (scenario % 4 == 2) {
					//Posn B
					transform.localPosition = new Vector3 (16.7f, height, 30.67f);
				} else if (scenario % 4 == 3) {
					//Posn C
					transform.localPosition = new Vector3 (15.85f, height, 64.04f);
				} else if (scenario % 4 == 0) {
					//Posn D
					transform.localPosition = new Vector3 (-15.32f, height, 30.67f);
				}

				if (scenario <= 4) {
					//Visual+Audio
					transform.localScale = DroneSize*Vector3.one;
					Audio.quad_sound.mute = false;
				} else if (scenario <= 8) {
					//Visual
					transform.localScale = DroneSize*Vector3.one;
					Audio.quad_sound.mute = true;
				} else if (scenario <= 12) {
					//Audio
					transform.localScale = new Vector3 (0f, 0f, 0f);
					Audio.quad_sound.mute = false;
				}

			}
	
		}
	}
}
