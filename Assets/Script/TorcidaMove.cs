using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorcidaMove : MonoBehaviour {
	/// <summary>
	/// The movement of the character in Y.
	/// </summary>
	float pingPongY;

	/// <summary>
	/// The frequency.
	/// </summary>
	[SerializeField]
	float frequency;

	/// <summary>
	/// The amplitude.
	/// </summary>
	[SerializeField]
	float amplitude;

	bool stop;
	public bool notMoving;

	Vector3 myStartTransform;

	float timer;


	// Use this for initialization
	void Start () {
		myStartTransform = this.transform.position;

		frequency = Random.Range (1,10);
		amplitude = Random.Range (0.5f,1.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		timer += Time.fixedDeltaTime;

		if (!stop) {
			this.transform.position = new Vector3 (myStartTransform.x, myStartTransform.y +pingPongY, myStartTransform.z);
		} else {
			timer = 0;
			ReturnToStartPos ();
		}

		pingPongY = Mathf.PingPong (timer*frequency,amplitude);

	}
	/// <summary>
	/// Changes the frequency.
	/// </summary>
	/// <param name="_frequency">Frequency.</param>
	public void ChangeFrequency (float _frequency ){
		frequency = _frequency;
	}
	/// <summary>
	/// Changes the amplitude.
	/// </summary>
	/// <param name="_amplitude">Amplitude.</param>
	public void ChangeAmplitude (float _amplitude ){
		amplitude = _amplitude;
	}

	/// <summary>
	/// Returns to start position.
	/// </summary>
	public void ReturnToStartPos(){

		float lerp = Mathf.Lerp (this.transform.position.y,myStartTransform.y,Time.fixedDeltaTime*frequency);
		


		if (this.transform.position.y >= myStartTransform.y + 0.1f) {
			this.transform.position = new Vector3 (this.transform.position.x, lerp, this.transform.position.z);
		} else {
			this.transform.position = myStartTransform;
			notMoving = true;
		}

	}

	public void StopMoving(){

		stop = true;

	}
	public void StartMoving(){
		stop = false;
	}
		
}
