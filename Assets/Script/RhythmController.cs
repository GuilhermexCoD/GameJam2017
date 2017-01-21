using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmController : MonoBehaviour {

	public static RhythmController singleton;

	public Text timer;
	public Text activeLineText;
	public Text activeLineText1;
	public Text activeLineText2;
	float timing, spaceTime, prevBeat;
	public float timeSet, actionTime;
	public bool action;

	public GameObject wave;
	public GameObject newWave;

	public List<int> music = new List<int> ();
	public int musicSize;
	public int activeLine;
    public List<KeyCode> validKeys = new List<KeyCode>();
	public List<KeyCode> validKeysJoystick = new List<KeyCode>();
	public List<int> validKeysInt = new List<int>();
	public List<Sprite> validKeysSprite = new List<Sprite> ();
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
	void Awake ()
	{

		for (int i = 0; i < validKeys.Count; i++) {
			validKeysInt.Add( ConvertKey2Int (validKeys [i]));
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

		//timer.text = timing.ToString();
		if (action) {
			timer.text += " Go";
		}


//		Debug.Log (action);
		if (Time.fixedTime >= spaceTime)					//Marca o inicio do espaco pra acao do jogador...
		{
			action = true;
		}
		
		if (Time.fixedTime >= timing) 						//Tempo final do hitmo pra chamada de funcoes...
		{
			timing = Time.fixedTime + timeSet;
			spaceTime = Time.fixedTime + prevBeat;
			action = false;
			newWave = Instantiate (wave) as GameObject;
			Destroy (newWave, 0.50f);
		}
		//comandos setas
		if (Input.GetKeyDown(KeyCode.UpArrow) && activeLine != 0) {
			foreach (var item in lineList) {
				lineList[activeLine].GetComponent<LineController>().isActive = false;
			}
			activeLine--;

			if (!lineList[activeLine].GetComponent<LineController>().missedTiming) {
				lineList[activeLine].GetComponent<LineController>().isActive = true;
			}
        }
		if (Input.GetKeyDown(KeyCode.DownArrow) && activeLine != lineList.Count-1) {
			foreach (var item in lineList) {
				lineList[activeLine].GetComponent<LineController>().isActive = false;
			}


			activeLine++;
			if (!lineList[activeLine].GetComponent<LineController>().missedTiming) {
				lineList[activeLine].GetComponent<LineController>().isActive = true;
			}

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
