using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RhythmController : MonoBehaviour {

	public Camera mainCamera;

	public static RhythmController singleton;

	public List<SpriteRenderer> PlayerModelSkin;
	public List<Color> skinColors;

	public Text timer;
	public Text activeLineText;
	public Text activeLineText1;
	public Text activeLineText2;
	public Button pressButton;

	float timing, spaceTime, prevBeat;
	public float timeSet, actionTime;
	public bool action;

	public GameObject wave;
	public GameObject newWave;

	public float frequency, amplitude;
	public float timerSine;
	public int countMovement;

	public List<int> music = new List<int> ();
	public int musicSize;
	public int activeLine;
    public List<KeyCode> validKeys = new List<KeyCode>();
	public List<KeyCode> validKeysJoystick = new List<KeyCode>();
	public List<int> validKeysInt = new List<int>();
	public List<Sprite> validKeysSprite = new List<Sprite> ();
	public List<Sprite> feedbackSprite = new List<Sprite> ();
	public Vector3 targetPosition;

	/// <summary>
	/// The speed that the player change lines.
	/// </summary>
	public float speed;
	/// <summary>
	/// The waypoints of the lines.
	/// </summary>
	public List<GameObject> waypoints = new List<GameObject>();
	public List<GameObject> lineList = new List<GameObject>();


	public bool pressedVerticalAxis;
	void Awake ()
	{
		targetPosition = this.transform.position;

		for (int i = 0; i < validKeys.Count; i++) {
			validKeysInt.Add( ConvertKey2Int (validKeys [i]));
		}
		int rnd = Random.Range (0, skinColors.Count-1);
		foreach (var item in PlayerModelSkin) {
			item.color = skinColors [rnd];
		}

        lineList[activeLine].GetComponent<LineController>().isActive = true;

        if (singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            singleton = this;
        }
        DontDestroyOnLoad(this.gameObject);

        activeLine = 0;
		if (waypoints.Count !=0) {
			targetPosition = waypoints [0].transform.position;
		}

		timing = timeSet;
		prevBeat = timeSet - actionTime;
		spaceTime = prevBeat;

		for (int i = 0; i < musicSize; i++) {
			music.Add (Random.Range(0,3));
		}

		foreach (var item in lineList) {
			item.GetComponent<LineController> ().myIndex = lineList.IndexOf (item);
		}
	}

	void FixedUpdate () 
	{
		timerSine += Time.fixedDeltaTime;
//		Mathf.Abs(Mathf.Sin(timerSine*frequency/2))/amplitude;
		countMovement = (int)Mathf.Floor((timerSine * frequency / 2)/Mathf.PI);

		//timer.text = timing.ToString();
//		if (action) {
//			timer.text += " Go";
//		}
		if (action) {
			pressButton.image.color = Color.green;
		} else {
			pressButton.image.color  = Color.red;
		}
		float y = Mathf.Sin (Mathf.PI/2);

//		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration> ().intensity =Mathf.Abs(y)*(3/2);
//		print (((timerSine * frequency / 2)/Mathf.PI)-countMovement);

		if ((Mathf.Abs (Mathf.Sin (timerSine * frequency / 2))) > 0.5f) {
			mainCamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration> ().intensity = Mathf.Clamp( Mathf.Abs(Mathf.Sin(timerSine*frequency/2)) - 0.6f,0,0.2f);
			action = true;
		} else {
			action = false;
			lineList [activeLine].GetComponent<LineController> ().pressed = false;
		}
//
//		if (((timerSine * frequency / 2)/Mathf.PI/2)-0.5f >){
//
//			print ("pico");
//			
//		}
//		Debug.Log (action);
//		if (Time.fixedTime >= spaceTime)					//Marca o inicio do espaco pra acao do jogador...
//		{
////			action = true;
//
//		}
//		
//		if (Time.fixedTime >= timing) 						//Tempo final do hitmo pra chamada de funcoes...
//		{
//			timing = Time.fixedTime + timeSet;
//			spaceTime = Time.fixedTime + prevBeat;
////			action = false;
//			lineList [activeLine].GetComponent<LineController> ().pressed = false;
//			newWave = Instantiate (wave) as GameObject;
//			Destroy (newWave, 0.50f);
//		}
		//comandos setas
		if (Input.GetKeyDown(KeyCode.UpArrow) && activeLine != 0 ) {
			foreach (var item in lineList) {
				lineList[activeLine].GetComponent<LineController>().isActive = false;
			}

			activeLine--;

			if (!lineList[activeLine].GetComponent<LineController>().missedTiming) {
				lineList[activeLine].GetComponent<LineController>().isActive = true;
			}
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) && activeLine != lineList.Count - 1 ) {
			foreach (var item in lineList) {
				lineList [activeLine].GetComponent<LineController> ().isActive = false;
			}

			activeLine++;
			

			if (!lineList [activeLine].GetComponent<LineController> ().missedTiming) {
				lineList [activeLine].GetComponent<LineController> ().isActive = true;
			}

		}

		if (Input.GetAxis("Vertical")> 0.5f && !pressedVerticalAxis && activeLine != 0) {
			foreach (var item in lineList) {
				lineList[activeLine].GetComponent<LineController>().isActive = false;
			}

			if (!pressedVerticalAxis) {
				activeLine--;
			}
			pressedVerticalAxis = true;

			if (!lineList[activeLine].GetComponent<LineController>().missedTiming) {
				lineList[activeLine].GetComponent<LineController>().isActive = true;
			}
		}
		if (Input.GetAxis ("Vertical") < -0.5f && !pressedVerticalAxis && activeLine != lineList.Count - 1) {
			foreach (var item in lineList) {
				lineList[activeLine].GetComponent<LineController>().isActive = false;
			}

			if (!pressedVerticalAxis) {
				activeLine++;
			}
			pressedVerticalAxis = true;
			if (!lineList[activeLine].GetComponent<LineController>().missedTiming) {
				lineList[activeLine].GetComponent<LineController>().isActive = true;
			}
		}

		if (Input.GetAxis ("Vertical") == 0 ) {
			pressedVerticalAxis = false;
		}

		//fim comandos seta

        if(lineList[activeLine].GetComponent<LineController>().isActive == false)
        {
            timer.text = lineList[activeLine].GetComponent<LineController>().sequence[lineList[activeLine].GetComponent<LineController>().activeCell].ToString();
        }
		targetPosition = waypoints [activeLine].transform.position;



			
		activeLineText.text = " 1 "+lineList[0].GetComponent<LineController>().isActive.ToString();
		activeLineText1.text = " 2 "+lineList[1].GetComponent<LineController>().isActive.ToString();
		activeLineText2.text = " 3 "+lineList[2].GetComponent<LineController>().isActive.ToString();


//		this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, Time.fixedDeltaTime * 2);

		this.transform.position =targetPosition;


	}
	public int ConvertKey2Int(KeyCode key){
		return (int)key;
	}


}
