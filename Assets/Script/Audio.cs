using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Audio : MonoBehaviour {

	public AudioSource source;
	public AudioClip BarulhoBotao;


	public void OnHover()

	{
		source.PlayOneShot (BarulhoBotao);

	}

}