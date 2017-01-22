using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {

	public Color SkinColorChar;

	public SpriteRenderer decalPart;
	public Color decalColor;
	public SpriteRenderer headPart;
	public SpriteRenderer armPart;
	public SpriteRenderer handPart;
	public SpriteRenderer upperBodyPart;
	public Color upperColor;
	public SpriteRenderer legLPart;
	public SpriteRenderer legRPart;
	public Color legColor;
	public SpriteRenderer footLPart;
	public SpriteRenderer footRPart;
	public List<Sprite> decal = new List<Sprite>();
	public List<Sprite> head = new List<Sprite>();
	public List<Sprite> arm = new List<Sprite>();
	public List<Sprite> upperBody = new List<Sprite>();
	public List<Sprite> legL = new List<Sprite>();
	public List<Sprite> legR = new List<Sprite>();


	public List<Color> SkinColor = new List<Color> ();

	public Color turnDownColor;

	// Use this for initialization
	void Start () {
		int rnd = Random.Range (0, head.Count-1);
		headPart.sprite = head [rnd];
		rnd = Random.Range (0, decal.Count-1);
		decalPart.sprite = decal [rnd];
		rnd = Random.Range (0, upperBody.Count-1);
		upperBodyPart.sprite = upperBody [rnd];
		rnd = Random.Range (0, legL.Count-1);
		legLPart.sprite = legL [rnd];
		legRPart.sprite = legR [rnd];

		rnd = Random.Range (0, arm.Count-1);
		armPart.sprite = arm [rnd];

	



		rnd = Random.Range (0, SkinColor.Count-1);
		headPart.color = SkinColor [rnd];
		armPart.color = SkinColor [rnd];
		handPart.color = SkinColor [rnd];
		footLPart.color = SkinColor [rnd];
		footRPart.color = SkinColor [rnd];

		SkinColorChar = SkinColor [rnd];
		decalColor = decalPart.color;
		upperColor = upperBodyPart.color;
		legColor = legLPart.color;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void TurnOnColors(){
		decalPart.color = decalColor;
		headPart.color = SkinColorChar;
		armPart.color = SkinColorChar;
		handPart.color = SkinColorChar;
		upperBodyPart.color = upperColor;
		legLPart.color = legColor;
		legRPart.color = legColor;
		footLPart.color = SkinColorChar;
		footRPart.color = SkinColorChar;


	}
	public void TurnOffColors(){
		decalPart.color = decalColor - turnDownColor;
		headPart.color = SkinColorChar - turnDownColor;
		armPart.color =SkinColorChar - turnDownColor;
		handPart.color = SkinColorChar - turnDownColor;
		upperBodyPart.color =upperColor - turnDownColor;
		legLPart.color =legColor - turnDownColor;
		legRPart.color =legColor - turnDownColor;
		footLPart.color = SkinColorChar - turnDownColor;
		footRPart.color = SkinColorChar - turnDownColor;

	}
}
