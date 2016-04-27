/*
 * To change position
 * 
 * transform.localPosition = new Vector3(0, -5, 0);
 * 
 * To control Velocity
 * transform.Translate(Vx,Vy,Vz);
 * 
 * transform.Translate(moveSpeed*Input.GetAxis ("Horizontal")*Time.deltaTime,0f,moveSpeed*Input.GetAxis ("Vertical")*Time.deltaTime);
 *
 * Merged initial audio for rotors
 * */


using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;


public class Kinematics : MonoBehaviour {

	int droneId;
	public float moveSpeed; 
	public float rotSpeed; 
	CharacterController uav ;
	Droneinfo droneinfo;
	Vector3 Forward;
	Vector3 Side;
	Vector3 Up;
    int k;
    double x;
	double y;
	double z;
	double p;
	double q;
	double r;
	double thrust;
    public double thrust_prop1;
    public double thrust_prop2;
    public double thrust_prop3;
    public double thrust_prop4;

    Vector3 radiusRoom;
	// infos

	public string lastReceivedUDPPacket="";

	//UnityEvent m_MyEvent;
	IPEndPoint outremoteEndPoint;
	IPEndPoint inremoteEndPoint;
	EndPoint sendEndPoint ;
	UdpClient outclient;
	UdpClient inclient; //UDP Client Port
	UDPSend sendobj;
	Socket sendSocket;
	public int port; // base port
	public int myPort;
	public int sport;
	public int rport;
    public int scale;

	//public static AudioSource quad_sound; //new audiosource for quad's sounds.
	//double velocity_squared; //will be summation of x,y,and z velocities

    Vector3 DroneScale;
	bool firsttime;

	// Use this for initialization
	void Start () 
	{
        k=0;
        firsttime = true;
        DroneScale = scale * Vector3.one;
		moveSpeed = 1f;
		rotSpeed = 1f*180/3.1415f;
		uav = GetComponent<CharacterController>();
		droneinfo = GetComponent<Droneinfo>();
        //radiusRoom= new Vector3(, 3, 3);
        Forward = transform.TransformDirection(Vector3.forward);
		Side = transform.TransformDirection(Vector3.right);
		Up = transform.TransformDirection(Vector3.up);
	
        /*

		//  Set up audio
		quad_sound = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip staticSound; //create an audio clip variable that will take the audioclip file we need for the quad

		staticSound = (AudioClip)Resources.Load("Quadrotor_MidThrottle_firstTake");
		quad_sound.clip = staticSound; //the file "heli_500_100%" is now a clip in the AudioSource quad_sound
		quad_sound.loop = true; //enable looping of sound.
        quad_sound.minDistance = 1f;
        quad_sound.maxDistance = 17.8f;
        quad_sound.spatialBlend = 1f;
        quad_sound.ignoreListenerVolume=true;
        quad_sound.spread = 194;
        //quad_sound.mute = true;//INITIALLY SHOULD BE MUTE
       
		//quad_sound.dopplerLevel = 3;
        quad_sound.Play();
        */
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (firsttime) {
			droneinfo = GetComponent<Droneinfo>();
			droneId = droneinfo.droneId;
			sport = droneinfo.SendPort;
			rport = droneinfo.RecvPort;
            rport = 25001;
			Debug.Log (droneId+" PORTS "+sport+" "+rport);

			inremoteEndPoint = new IPEndPoint (IPAddress.Any, /*port*/ rport);
			inclient = new UdpClient (rport); // input port
            //transform.position = new Vector3(174.0f, 1.5f, 304.0f);  OLD coordinates
            transform.position = new Vector3(63f, 2.33f, 48.737f); // UNITY's XYZ
            //transform.localScale = new Vector3(1.5f,1.5f, 1.5f);
            //transform.localScale = DroneScale;
            //
            const int ProtocolPort = 3000;
			sendSocket = new Socket(AddressFamily.InterNetwork, 
			                               SocketType.Dgram, ProtocolType.Udp);
			IPAddress sendTo = IPAddress.Parse("127.0.0.1");
			sendEndPoint = new IPEndPoint(sendTo, ProtocolPort);
            sendSocket.SendBufferSize = 0;
            inclient.Client.ReceiveBufferSize = 48;
            firsttime = false;

			x = 0;
			y = 0;
			z = 0;
			p = 0;
			q = 0;
			r = 0;
			thrust = 25;
		}



        if (inclient.Available > 0) {
	
	
			byte[] data = inclient.Receive (ref inremoteEndPoint);
		
	
			
	
			x = 10f*System.BitConverter.ToDouble (data, 0);
			y = 10f*System.BitConverter.ToDouble (data, 8);
			z = 10f*System.BitConverter.ToDouble (data, 16);
			p = System.BitConverter.ToDouble (data, 24);
			q = System.BitConverter.ToDouble (data, 32);
			r = System.BitConverter.ToDouble (data, 40);
			thrust=System.BitConverter.ToDouble (data,48);
            thrust_prop1= System.BitConverter.ToDouble(data, 56);
            thrust_prop2 = System.BitConverter.ToDouble(data, 64);
            thrust_prop3 = System.BitConverter.ToDouble(data, 72);
            thrust_prop4 = System.BitConverter.ToDouble(data, 80);
            //double.TryParse(text, out x);
            //print(">> x=" + x.ToString());
            //} else {
            //	Debug.Log (myPort+" client not available??");
        }







        // uav.Move(moveSpeed*((float)x*Forward + (float)y*Side + (float)z*Up)*Time.deltaTime);
        uav.Move(moveSpeed * (+(float)x * Forward + (float)y * Side + (float)z * Up) * Time.deltaTime);
		//Debug.Log (z);
        // rotor audio
 //       velocity_squared = (x * x) + (y * y) + (z * z); //find summ of velocities squared
                                                        //totalThrust = (t1 + t2 + t3 + t4)/4; //plain  summation for now
        //quad_sound.volume = 0.7f + (float)(0.1 * velocity_squared);
		//quad_sound.pitch = 0.7f + (float)(0.6 * Mathf.Sqrt((float)velocity_squared));
       //quad_sound.pitch = 0.7f + (float)(0.2 * Mathf.Sqrt((float)velocity_squared));
		if (thrust < 25) {
			thrust = 25;
		}
		thrust -= 25;
        //
      //  if (quad_sound.volume <= 0.6)
      //  {
      //      quad_sound.volume = 0.6f;
      //  }
      //  else
      //  {
      //  quad_sound.volume = 0.2f + (float)(0.9 * thrust);
      //  }
		
        //Debug.Log(gameObject.transform.position.z);
		//quad_sound.pitch = 0.5f + (float)(0.6 * Mathf.Sqrt(System.Math.Abs((float)thrust)));
		//
		//Debug.Log (quad_sound.pitch);
		//Debug.Log (thrust);
      //  transform.eulerAngles = new Vector3(-(float)p*rotSpeed,-(float)r*rotSpeed,-(float)q*rotSpeed);

/*

        //Conditional statements on boundaries of sound and visibility.
        if (gameObject.transform.position.x >= 40 || gameObject.transform.position.x <= -15 || gameObject.transform.position.z >= 58.4 || gameObject.transform.position.z <= 39)
        {
            quad_sound.mute = true;
           // gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        }

        else
        {
            quad_sound.mute = false;
          //  gameObject.transform.localScale = DroneScale;
        }
        //    

        */

        byte[] outdata = new byte[12];
        byte[] xout = new byte[4];
        byte[] yout = new byte[4];
        byte[] zout = new byte[4];

        int xd;// = new System.Double ();
        int yd;// = new System.Double ();
        int zd;// = new System.Double ();
        float xf;
        xf = (float)transform.localPosition.x;
        k++;
        xd = (int)(10000f *xf);
        yd = (int)(10000 * transform.localPosition.y);
        zd = (int)(10000 * transform.localPosition.z);
       // xd = 6000+k;
       // yd = 2000;
       // zd = 2;
       // xd = (int)(10000f*uav.transform.position.x);
       // print(xd);
		xout = System.BitConverter.GetBytes(xd);
		yout = System.BitConverter.GetBytes(yd);
		zout = System.BitConverter.GetBytes(zd);
		System.Buffer.BlockCopy(xout,0,outdata,0,4);
		System.Buffer.BlockCopy(yout,0,outdata,4,4);
		System.Buffer.BlockCopy(zout,0,outdata,8,4);

			int b = sendSocket.SendTo(outdata, 
			                  sendEndPoint);

        //print(BitConverter.ToInt32(yout,0));
        //print(yd);

        //Debug.Log("snet: "+b);

        //	}


        //FOR BUTTON PRESSES
        //if (Input.GetKeyDown("2") || Input.GetKeyDown("6"))
        //   quad_sound.mute = true;
     



    }
	/*
	// Model for a function that will, e.g., reset parameters 
	void Ping() {
		Debug.LogWarning ("Ping "+droneId);
	}*/

}
