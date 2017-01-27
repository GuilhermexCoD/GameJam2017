using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaJam : MonoBehaviour {

	public GameObject prefab;
	public int numberOfObjects;
	[HideInInspector]
	public float radius;
	public GameObject[] musicObject;
	public float min,max;
	public float spectrumMultiplier;
	public int indexMaxSpec;
	public bool active;
	public float winPercent;
	public float lerp;
	// Use this for initialization
	void Start () {
		radius = numberOfObjects * 0.2f;
		spectrumMultiplier = numberOfObjects * 2;
		musicObject = new GameObject[numberOfObjects];
		for (int i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			Vector3 pos = new Vector3 (Mathf.Cos(angle),0,Mathf.Sin(angle))*radius;
			GameObject g = (GameObject)Instantiate (prefab,pos,Quaternion.identity);
			musicObject [i] = g;
		}

		min = float.MaxValue;
		max = float.MinValue;
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = new float[1024];

		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.Hamming );

//		for( int i = 1; i < spectrum.Length-1; i++ )
//		{
//
//
//			Debug.DrawLine( new Vector3( i - 1, spectrum[i] + 10, 0 ), new Vector3( i, spectrum[i + 1] + 10, 0 ), Color.red );
//			Debug.DrawLine( new Vector3( i - 1, Mathf.Log( spectrum[i - 1] ) + 10, 2 ), new Vector3( i, Mathf.Log( spectrum[i] ) + 10, 2 ), Color.cyan );
//			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), spectrum[i - 1] - 10, 1 ), new Vector3( Mathf.Log( i ), spectrum[i] - 10, 1 ), Color.green );
//			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), Mathf.Log( spectrum[i - 1] ), 3 ), new Vector3( Mathf.Log( i ), Mathf.Log( spectrum[i] ), 3 ), Color.blue );
//		}

		for (int i = 0; i < numberOfObjects; i++) {
//			print (spectrum[i]);
			Vector3 previusScale = musicObject [i].transform.localScale;
			previusScale.y = spectrum [i] * spectrumMultiplier;
			musicObject [i].transform.localScale = previusScale;
			musicObject [i].GetComponent<Renderer> ().material.color = Color.Lerp (Color.green,Color.red,Mathf.Clamp01( (spectrum[i]/max)*2));
			if (spectrum[i] < min) {
				min = spectrum [i];
			}
			if (spectrum[i] > max) {
				max = spectrum [i];
				indexMaxSpec = i;
			}

		}
		lerp = Mathf.Clamp01 ((spectrum [indexMaxSpec] / max));
		if (spectrum [indexMaxSpec] > max * winPercent) {
			active = true;
		} else {
			active = false;
		}




		
	}
}
