using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {
	public SpriteRenderer decalPart;
	public SpriteRenderer headPart;
	public SpriteRenderer armPart;
	public SpriteRenderer handPart;
	public SpriteRenderer upperBodyPart;
	public SpriteRenderer legLPart;
	public SpriteRenderer legRPart;
	public SpriteRenderer footLPart;
	public SpriteRenderer footRPart;
	public List<Sprite> decal = new List<Sprite>();
	public List<Sprite> head = new List<Sprite>();
	public List<Sprite> arm = new List<Sprite>();
	public List<Sprite> upperBody = new List<Sprite>();
	public List<Sprite> legL = new List<Sprite>();
	public List<Sprite> legR = new List<Sprite>();

	public List<Color> SkinColor = new List<Color> ();

	// Use this for initialization
	void Start () {

		int rnd = Random.Range (0, head.Count);
		headPart.sprite = head [rnd];
		rnd = Random.Range (0, decal.Count);
		decalPart.sprite = decal [rnd];
		rnd = Random.Range (0, upperBody.Count);
		upperBodyPart.sprite = upperBody [rnd];
		rnd = Random.Range (0, legL.Count);
		legLPart.sprite = legL [rnd];
		legRPart.sprite = legR [rnd];

		rnd = Random.Range (0, arm.Count);
		armPart.sprite = arm [rnd];

	



		rnd = Random.Range (0, SkinColor.Count);
		headPart.color = SkinColor [rnd];
		armPart.color = SkinColor [rnd];
		handPart.color = SkinColor [rnd];
		footLPart.color = SkinColor [rnd];
		footRPart.color = SkinColor [rnd];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
