using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorcidaMove : MonoBehaviour {

	float pingPong;

	[Range(0,10)]
	public float frequency;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		pingPong = Mathf.PingPong (Time.time*frequency,2);


		this.transform.position = new Vector3 (pingPong,this.transform.position.y,this.transform.position.z);

	}
	public void (){

	}
}
