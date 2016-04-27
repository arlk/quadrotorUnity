using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
 * A simple class to manage drone properties (for multi-drone scenes
 */

public class Droneinfo : MonoBehaviour {
	public int droneId;  /// Should beunique id for each drone

	//public UnityEvent global_MyEvent;

	//CSVReader.configInfo[] theConfig;

	public int SendPort;
	public int RecvPort;
	private bool initdone = false;

	//Setup theSetup;
	// Use this for initialization
	void Start () {

		//theSetup = GetComponent<Setup>();

		//Debug.LogWarning ("found setup: "+Setup.something);
	}
	
	// Update is called once per frame
	void Update () {

	//	if (Input.anyKeyDown && global_MyEvent != null)
	//	{
	//		global_MyEvent.Invoke ();
	//	}


	
	}

	public bool getinitdone() {
		return initdone;
	}

	public void setetinitdone() {
		Debug.Log ("setting done "+SendPort+" "+RecvPort);
		initdone=true;
	}


	public int getdroneId() {
		return droneId;
	}
}
