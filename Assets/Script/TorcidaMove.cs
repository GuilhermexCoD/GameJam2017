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

	public bool stop;
	public bool notMoving;

	Vector3 myStartTransform;

	float timer;

	public int countMovement;

	public SpriteRenderer boardSprite;
	public SpriteRenderer keySprite;

	public Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
//		myStartTransform = this.transform.position+ new Vector3(0,-amplitude,0);
		myStartTransform = this.transform.position;
		amplitude = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		timer += Time.fixedDeltaTime;

		if (!stop) {
			this.transform.position = new Vector3 (myStartTransform.x, myStartTransform.y + Mathf.Abs(Mathf.Sin(timer*frequency/2))/amplitude, myStartTransform.z);
			countMovement = (int)Mathf.Floor((timer * frequency / 2)/Mathf.PI);
		} else {
			timer = 0;
			ReturnToStartPos ();
		}



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

	public void ChooseKey(KeyCode key){
		for (int i = 0; i < RhythmController.singleton.validKeysInt.Count; i++) {
			if (RhythmController.singleton.ConvertKey2Int(key) == RhythmController.singleton.validKeysInt[i]) {
				int x = RhythmController.singleton.validKeysInt.IndexOf(RhythmController.singleton.validKeysInt[i]);
				keySprite.sprite = RhythmController.singleton.validKeysSprite[x];
			}
		}

	}
		
}
