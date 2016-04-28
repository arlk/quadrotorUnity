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

	public float soundBubble;

	GameObject PlayerCamera;

    // Use this for initialization
    void Start()
    {
        //  Set up audio
		prop_1_sound = GameObject.Find("Prop1").GetComponent<AudioSource>();
        prop_2_sound = GameObject.Find("Prop2").GetComponent<AudioSource>();
        prop_3_sound = GameObject.Find("Prop3").GetComponent<AudioSource>();
        prop_4_sound = GameObject.Find("Prop4").GetComponent<AudioSource>();

		PlayerCamera = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update()
    {

        prop_1_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop1);
        prop_2_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop2);
        prop_3_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop3);
        prop_4_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop4);

        prop_1_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop1)));
        prop_2_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop2)));
        prop_3_sound.pitch = 0.8f + (float)(0.001 * Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop3)));
        prop_4_sound.pitch = 0.8f + (float)(0.001* Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop4)));

    }
}