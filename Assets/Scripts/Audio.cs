using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

public class Audio : MonoBehaviour
{
    
    public static AudioSource quad_sound; //new audiosource for quad's sounds.
    public static AudioSource prop_1_sound;
    public static AudioSource prop_2_sound;
    public static AudioSource prop_3_sound;
    public static AudioSource prop_4_sound;

    double velocity_squared; //will be summation of x,y,and z velocities

    //public static Kinematics ThrustSource;//receives thrust values from Kinematics script. Both Kinematics and Audio script willbe attached to same gameobject, in this case the quadrotor.
    //public static PropRotation_New PropAudio;
   


    // Use this for initialization
    void Start()
    {

        //  Set up audio
        quad_sound = gameObject.AddComponent<AudioSource>();

        prop_1_sound = GameObject.Find("Prop1").AddComponent<AudioSource>();
        prop_2_sound = GameObject.Find("Prop2").AddComponent<AudioSource>();
        prop_3_sound = GameObject.Find("Prop3").AddComponent<AudioSource>();
        prop_4_sound = GameObject.Find("Prop4").AddComponent<AudioSource>();

        AudioClip staticSound; //create an audio clip variable that will take the audioclip file we need for the quad
        staticSound = (AudioClip)Resources.Load("Quadrotor_MidThrottle_firstTake");

        

        //attach clip to audio sample
       quad_sound.clip = staticSound; //the file "heli_500_100%" is now a clip in the AudioSource quad_sound
        prop_1_sound.clip = staticSound;
        prop_2_sound.clip = staticSound;
        prop_3_sound.clip = staticSound;
        prop_4_sound.clip = staticSound;



        //set up sound settings
        quad_sound.loop = true; //enable looping of sound.
        quad_sound.minDistance = 1f;
        quad_sound.maxDistance = 17.8f;
        quad_sound.spatialBlend = 1f;
       quad_sound.ignoreListenerVolume = true;
       quad_sound.spread = 194;

        prop_1_sound.loop = true; //enable looping of sound.
        prop_1_sound.minDistance = 1f;
        prop_1_sound.maxDistance = 17.8f;
        prop_1_sound.spatialBlend = 1f;
        prop_1_sound.ignoreListenerVolume = true;
        prop_1_sound.spread = 194;

        prop_2_sound.loop = true; //enable looping of sound.
        prop_2_sound.minDistance = 1f;
        prop_2_sound.maxDistance = 17.8f;
        prop_2_sound.spatialBlend = 1f;
        prop_2_sound.ignoreListenerVolume = true;
        prop_2_sound.spread = 194;

        prop_3_sound.loop = true; //enable looping of sound.
        prop_3_sound.minDistance = 1f;
        prop_3_sound.maxDistance = 17.8f;
        prop_3_sound.spatialBlend = 1f;
        prop_3_sound.ignoreListenerVolume = true;
        prop_3_sound.spread = 194;

        prop_4_sound.loop = true; //enable looping of sound.
        prop_4_sound.minDistance = 1f;
        prop_4_sound.maxDistance = 17.8f;
        prop_4_sound.spatialBlend = 1f;
        prop_4_sound.ignoreListenerVolume = true;
        prop_4_sound.spread = 194;


        //quad_sound.mute = true;//INITIALLY SHOULD BE MUTE

        //quad_sound.dopplerLevel = 3;
      //  quad_sound.Play();
        prop_1_sound.Play();
        prop_2_sound.Play();
        prop_3_sound.Play();
        prop_4_sound.Play();

    }

    // Update is called once per frame
    void Update()
    {

        //if (gameObject.GetComponent<Kinematics>().thrust_prop1 <= 220000 || gameObject.GetComponent<Kinematics>().thrust_prop1 >= 190000)
        //{ 
        //keep reading thrust values 
        //quad_sound.volume = 0.2f + (float)(0.9 * ThrustSource.thrust_actual);
        prop_1_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop1);
        //}



        prop_2_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop2);
        prop_3_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop3);
        prop_4_sound.volume = 0.5f + (float)(0.0000008 * gameObject.GetComponent<Kinematics>().thrust_prop4);
        //  }


       // quad_sound.pitch = 0.5f + (float)(0.6 * Mathf.Sqrt(System.Math.Abs((float)ThrustSource.thrust_actual)));
        prop_1_sound.pitch = 0.2f +(float)(0.001 * Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop1)));
        Debug.Log(prop_1_sound.pitch);
        prop_2_sound.pitch = 0.2f + (float)(0.001 * Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop2)));
        prop_3_sound.pitch = 0.2f + (float)(0.001 * Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop3)));
        prop_4_sound.pitch = 0.2f + (float)(0.001* Mathf.Sqrt(System.Math.Abs((float)gameObject.GetComponent<Kinematics>().thrust_prop4)));

        //
        //Debug.Log(quad_sound.pitch);



        //Conditional statements on boundaries of sound and visibility.
        if (gameObject.transform.position.x >= 55 || gameObject.transform.position.x <= -11 || gameObject.transform.position.z >= 58.4 || gameObject.transform.position.z <= 39)
        {
            //quad_sound.mute = true;
            prop_1_sound.mute = true;
            prop_2_sound.mute = true;
            prop_3_sound.mute = true;
            prop_4_sound.mute = true;
            gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        }

        else
        {
            //quad_sound.mute = false;
            prop_1_sound.mute = false;
            prop_2_sound.mute = false;
            prop_3_sound.mute = false;
            prop_4_sound.mute = false;
            gameObject.transform.localScale = new Vector3(1f,1f,1f);
        }





    }
}