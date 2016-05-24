using UnityEngine;
using System.Collections;



public class Apache : MonoBehaviour {
	
	public GameObject TopPropeller , BackPropeller;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		Propeller (); //this is to make the propeller rotate
	}
	
	public void Propeller()
	{
		TopPropeller.transform.Rotate(Vector3.forward, Time.deltaTime*3500);
		
		BackPropeller.transform.Rotate(Vector3.forward, Time.deltaTime*3500);
		
	}
}
