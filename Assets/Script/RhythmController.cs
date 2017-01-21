using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmController : MonoBehaviour {

	public RhythmController singleton;

	public Text timer;

	float timing, spaceTime, prevBeat;
	public float timeSet, actionTime;
	public bool action;

	public GameObject wave;
	public GameObject newWave;

	public List<int> music = new List<int> ();
	public int musicSize;
	public int activeLine;

	public Vector3 targetPosition;

	/// <summary>
	/// The speed that the player change lines.
	/// </summary>
	public float speed;
	/// <summary>
	/// The waypoints of the lines.
	/// </summary>
	public List<GameObject> waypoints = new List<GameObject>();

	List<GameObject> lineList = new List<GameObject>();
	void Awake ()
	{
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
	}

	void FixedUpdate () 
	{

		timer.text = timing.ToString();
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

		if (Input.GetKeyDown(KeyCode.UpArrow) && activeLine != 0) {
			activeLine--;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && activeLine != 2) {
			activeLine++;
		}

		targetPosition = waypoints [activeLine].transform.position;

		foreach (var item in lineList) {
			if (lineList.IndexOf (item) == activeLine) {
				lineList [activeLine].GetComponent<LineController> ().active = true;
			} else {
				lineList [activeLine].GetComponent<LineController> ().active = false;
			}
		}

	



//		this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, Time.fixedDeltaTime * 2);

		this.transform.position =targetPosition;


	}
}
