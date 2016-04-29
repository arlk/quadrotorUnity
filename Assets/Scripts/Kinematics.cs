/*
 * To change position
 * transform.localPosition = new Vector3(0, -5, 0);
 * 
 * To control Velocity
 * transform.Translate(moveSpeed*Input.GetAxis ("Horizontal")*Time.deltaTime,0f,moveSpeed*Input.GetAxis ("Vertical")*Time.deltaTime);
 *
 * To control body rates
 * transform.eulerAngles = new Vector3(-(float)p*rotSpeed,-(float)r*rotSpeed,-(float)q*rotSpeed);
 * */


using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;


public class Kinematics : MonoBehaviour {

	public float moveSpeed; 
	public float rotSpeed; 

	public bool move;

	CharacterController uav ;

	Vector3 Forward;
	Vector3 Side;
	Vector3 Up;

    double x;
	double y;
	double z;
	double p;
	double q;
	double r;

	Socket sendSocket;
	EndPoint sendEndPoint ;

	IPEndPoint inremoteEndPoint;
	UdpClient inclient;

	public int sport;
	public int rport;

	// Use this for initialization
	void Start () 
	{
		uav = GetComponent<CharacterController>();

        Forward = transform.TransformDirection(Vector3.forward);
		Side = transform.TransformDirection(Vector3.right);
		Up = transform.TransformDirection(Vector3.up);

		// UDP Send Port
		sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		IPAddress sendTo = IPAddress.Parse("127.0.0.1");
		sendEndPoint = new IPEndPoint(sendTo, sport);
		sendSocket.SendBufferSize = 0;

		// UDP Receive Port
		inremoteEndPoint = new IPEndPoint (IPAddress.Any, rport);
		inclient = new UdpClient (rport);
		inclient.Client.ReceiveBufferSize = 48;

		x = 0;
		y = 0;
		z = 0;

		p = 0;
		q = 0;
		r = 0;

		move = false;
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (inclient.Available > 0) {
			
			byte[] data = inclient.Receive (ref inremoteEndPoint);
		
			x = System.BitConverter.ToDouble (data, 0);
			y = System.BitConverter.ToDouble (data, 8);
			z = System.BitConverter.ToDouble (data, 16);

			p = System.BitConverter.ToDouble (data, 24);
			q = System.BitConverter.ToDouble (data, 32);
			r = System.BitConverter.ToDouble (data, 40);
        }

		float moveFloat = move ? 1.0f : 0.0f;
		uav.Move(moveSpeed * moveFloat * ((float)x * Forward + (float)y * Side + (float)z * Up) * Time.deltaTime);
		transform.eulerAngles = rotSpeed * moveFloat * new Vector3(-(float)p,-(float)r,-(float)q);

        byte[] outdata = new byte[12];
        byte[] xout = new byte[4];
        byte[] yout = new byte[4];
        byte[] zout = new byte[4];

        int xd;
        int yd;
        int zd;

		xd = (int)(10000.0f * (float) transform.localPosition.x);
		yd = (int)(10000.0f * (float) transform.localPosition.y);
		zd = (int)(10000.0f * (float) transform.localPosition.z);

		xout = System.BitConverter.GetBytes(xd);
		yout = System.BitConverter.GetBytes(yd);
		zout = System.BitConverter.GetBytes(zd);
		System.Buffer.BlockCopy(xout,0,outdata,0,4);
		System.Buffer.BlockCopy(yout,0,outdata,4,4);
		System.Buffer.BlockCopy(zout,0,outdata,8,4);

		sendSocket.SendTo(outdata, sendEndPoint);
    }
}
