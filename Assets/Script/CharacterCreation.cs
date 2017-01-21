using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {
	public SpriteRenderer hairPart;
	public SpriteRenderer headPart;
	public SpriteRenderer upperBodyPart;
	public SpriteRenderer lowerBodyPart;

	public List<Sprite> hair = new List<Sprite>();
	public List<Sprite> head = new List<Sprite>();
	public List<Sprite> upperBody = new List<Sprite>();
	public List<Sprite> lowerBody = new List<Sprite>();

	// Use this for initialization
	void Start () {

		int rnd = Random.Range (0, head.Count);
		headPart.sprite = head [rnd];
		rnd = Random.Range (0, upperBody.Count);
		upperBodyPart.sprite = upperBody [rnd];
		rnd = Random.Range (0, lowerBody.Count);
		lowerBodyPart.sprite = lowerBody [rnd];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
