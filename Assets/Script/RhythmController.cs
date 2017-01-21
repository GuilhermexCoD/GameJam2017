using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour {

	float timing, spaceTime, prevBeat;
	public float timeSet, actionTime;
	public bool action;

	public GameObject wave;
	public GameObject newWave;

	void Awake ()
	{
		timing = timeSet;
		prevBeat = timeSet - actionTime;
		spaceTime = prevBeat;
	}

	void FixedUpdate () 
	{
		Debug.Log (action);
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

	}
}
