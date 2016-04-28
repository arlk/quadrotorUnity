using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

public class AudioThrust : MonoBehaviour
{
    public static AudioSource prop_1_sound;
    public static AudioSource prop_2_sound;
    public static AudioSource prop_3_sound;
    public static AudioSource prop_4_sound;

	double thrust_prop1;
	double thrust_prop2;
	double thrust_prop3;
	double thrust_prop4;

	IPEndPoint inremoteEndPoint;
	UdpClient inclient;
	public int rport;

    // Use this for initialization
    void Start()
    {
        //  Set up audio
		prop_1_sound = GameObject.Find("Prop1").GetComponent<AudioSource>();
        prop_2_sound = GameObject.Find("Prop2").GetComponent<AudioSource>();
        prop_3_sound = GameObject.Find("Prop3").GetComponent<AudioSource>();
        prop_4_sound = GameObject.Find("Prop4").GetComponent<AudioSource>();

		// UDP Receive Port
		inremoteEndPoint = new IPEndPoint (IPAddress.Any, rport);
		inclient = new UdpClient (rport);
		inclient.Client.ReceiveBufferSize = 48;

		thrust_prop1 = 0;
		thrust_prop2 = 0;
		thrust_prop3 = 0;
		thrust_prop4 = 0;
    }

    // Update is called once per frame
    void Update()
    {
		if (inclient.Available > 0) {

			byte[] data = inclient.Receive (ref inremoteEndPoint);

			thrust_prop1 = System.BitConverter.ToDouble(data, 0);
			thrust_prop2 = System.BitConverter.ToDouble(data, 8);
			thrust_prop3 = System.BitConverter.ToDouble(data, 16);
			thrust_prop4 = System.BitConverter.ToDouble(data, 24);
		}

        prop_1_sound.volume = 0.5f + (float)(0.0000008 * thrust_prop1);
        prop_2_sound.volume = 0.5f + (float)(0.0000008 * thrust_prop2);
        prop_3_sound.volume = 0.5f + (float)(0.0000008 * thrust_prop3);
        prop_4_sound.volume = 0.5f + (float)(0.0000008 * thrust_prop4);

		prop_1_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt((float)thrust_prop1));
		prop_2_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt((float)thrust_prop2));
		prop_3_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt((float)thrust_prop3));
		prop_4_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt((float)thrust_prop4));
    }
}