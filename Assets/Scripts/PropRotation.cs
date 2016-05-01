using UnityEngine;
using System.Collections;

public class PropRotation : MonoBehaviour {

    GameObject thisprop1;
	GameObject thisprop2;
	GameObject thisprop3;
	GameObject thisprop4;

	float angle;

	// Use this for initialization
	void Start () {
		
		thisprop1 = GameObject.Find("Prop1");
		thisprop2 = GameObject.Find("Prop2");
		thisprop3 = GameObject.Find("Prop3");
		thisprop4 = GameObject.Find("Prop4");

		angle = 270f;

	}
	
	// Update is called once per frame
	void Update () {
		angle = angle + 80f;

		thisprop1.transform.eulerAngles = new Vector3(-90f ,-(float)angle,0f);
		thisprop2.transform.eulerAngles = new Vector3(-90f,(float)angle ,-0f);
		thisprop3.transform.eulerAngles = new Vector3(-90f,-(float)angle ,-0f);
		thisprop4.transform.eulerAngles = new Vector3(-90f,(float)angle ,-0f);
	}
}
