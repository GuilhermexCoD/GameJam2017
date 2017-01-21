using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorcidaMove : MonoBehaviour {

	float pingPong;

	public float frequency;

	public float amplitude;

	Transform myStartTransform;

	// Use this for initialization
	void Start () {
		myStartTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {

		pingPong = Mathf.PingPong (Time.time*frequency,amplitude);


		this.transform.position = new Vector3 (pingPong,this.transform.position.y,this.transform.position.z);

	}
	public void ChangeFrequency (float _frequency ){

	}
}
