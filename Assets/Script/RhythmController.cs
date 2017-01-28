using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RhythmController : MonoBehaviour {

	public enum PlayerState
	{
		tambor,
		batuque,
		pandeiro
	}
	public int streakCount;
	public float score;
	public float scoreLineComplete =1000;
	public float scoreSingleHit = 100;
	public float scoreTimeDecrement = 1;
	public float comboDecres;
	public float timerScore;
	public int countNoStreak;
	public List<Color> ComboColor = new List<Color> ();

	public Image Splash;
	public Image Splash1;

	public GameObject pauseScreen;

	public Animator anim;

	public Camera mainCamera;

	public static RhythmController singleton;

	public List<SpriteRenderer> PlayerModelSkin;
	public List<Color> skinColors;

	public Text timer;
	public Text activeLineText;
	public Text activeLineText1;
	public Text activeLineText2;

	public Text inicialCounter;
	public Text scoreText;
	public Text comboText;
	public Text comboScore;
	public float timerComboScore;
	public bool streak;
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

	public AudioSource audioS;
	public AudioSource audioSHitWin;
	public AudioSource audioSStreak3;
	public AudioSource audioSWin;
	public List<AudioSource> audioList= new List<AudioSource>() ;
	public bool pressedVerticalAxis;


	public bool teste;
	public bool win;
	public bool lost;
	public float timerWin;

	public bool ReadyToPlay;
	public float TimerReady;
	int count = 0;
	public int CountCompleteLine;
	void Awake ()
	{
		scoreText.gameObject.SetActive (false);
		comboText.gameObject.SetActive (false);
		comboScore.gameObject.SetActive (false);
		audioList.Add (audioS);
		audioList.Add (audioSHitWin);
		audioList.Add (audioSStreak3);
		audioList.Add (audioSWin);
		win = false;
		ReadyToPlay = false;
		anim =this.gameObject.GetComponentInChildren<Animator> ();

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
		

		if (ReadyToPlay) {
			timerScore += Time.fixedDeltaTime;
			scoreText.gameObject.SetActive (true);
			comboText.gameObject.SetActive (streak);
			if (!win && !lost) {
				score -= scoreTimeDecrement+comboDecres;
			}

			scoreText.text = score.ToString();
			comboText.text ="Combo "+ streakCount.ToString()+"x";
			if (action && Input.anyKeyDown) {
				

			} else if (!action) {
				if (count ==0) {
					countNoStreak++;
				}
				count++;
				if (countNoStreak==2) {
//					streak = false;
					incrementStreakScore();
					streakCount = 0;
					countNoStreak = 0;
				}

			}
			comboScore.gameObject.SetActive (streak);
			comboScore.text = (scoreSingleHit * streakCount * (1 + ((float)streakCount / 10))).ToString();



		
	

			timerSine += Time.fixedDeltaTime;
//		Mathf.Abs(Mathf.Sin(timerSine*frequency/2))/amplitude;
			countMovement = (int)Mathf.Floor ((timerSine * frequency / 2) / Mathf.PI);

			//timer.text = timing.ToString();
//		if (action) {
//			timer.text += " Go";
//		}
			if (action) {
				count = 0;
				pressButton.image.color = Color.green;
			} else {
				pressButton.image.color = Color.red;
			}
			float y = Mathf.Sin (Mathf.PI / 2);


//			CheckSinAction ();
			CheckForMusicSpectrum ();
			LoseCondition ();



//		mainCamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration> ().intensity =Mathf.Abs(y)*(3/2);
//		print (((timerSine * frequency / 2)/Mathf.PI)-countMovement);

		
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


			//fim comandos seta

			if (lineList [activeLine].GetComponent<LineController> ().isActive == false) {
				timer.text = lineList [activeLine].GetComponent<LineController> ().sequence [lineList [activeLine].GetComponent<LineController> ().activeCell].ToString ();
			}
			targetPosition = waypoints [activeLine].transform.position;


			anim.SetInteger ("Situate", activeLine);
			
			activeLineText.text = " 1 " + lineList [0].GetComponent<LineController> ().isActive.ToString ();
			activeLineText1.text = " 2 " + lineList [1].GetComponent<LineController> ().isActive.ToString ();
			activeLineText2.text = " 3 " + lineList [2].GetComponent<LineController> ().isActive.ToString ();


//		this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, Time.fixedDeltaTime * 2);

			this.transform.position = targetPosition;



		} else {
			StartCounter ();
		}
	}
	public void CheckSinAction(){
		if ((Mathf.Abs (Mathf.Sin (timerSine * frequency / 2))) > 0.4f) {
			mainCamera.GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration> ().intensity = Mathf.Clamp (Mathf.Abs (Mathf.Sin (timerSine * frequency / 2)) - 0.6f, 0, 0.2f);
			Splash.color = new Color(Splash.color.r,Splash.color.g,Splash.color.b, Mathf.Clamp (Mathf.Abs (Mathf.Sin (timerSine * frequency / 2)) - 0.6f, 0, 0.2f));
			Splash1.color =new Color(Splash1.color.r,Splash1.color.g,Splash1.color.b, Mathf.Clamp (Mathf.Abs (Mathf.Sin (timerSine * frequency / 2)) - 0.6f, 0, 0.2f));
			action = true;
		} else {
			action = false;
			lineList [activeLine].GetComponent<LineController> ().pressed = false;
		}
	}

	public void CheckForMusicSpectrum(){
		Splash.color = new Color (Splash.color.r, Splash.color.g, Splash.color.b, Mathf.Clamp (mainCamera.GetComponent<MusicaJam> ().lerp, 0, 1));
		Splash1.color = new Color (Splash1.color.r, Splash1.color.g, Splash1.color.b, Mathf.Clamp (mainCamera.GetComponent<MusicaJam> ().lerp, 0, 1));
		if (mainCamera.GetComponent<MusicaJam> ().active) {
			
			action = true;
		} else {
			action = false;
			lineList [activeLine].GetComponent<LineController> ().pressed = false;
		}
	}

	public void LoseCondition(){
		int countLose = 0;
		foreach (var item in lineList) {
			if (item.GetComponent<LineController> ().wrong) {
				countLose++;
			}



		}
		if (countLose == lineList.Count) {
			lost = true;
		}

		if (lost) {
			print("lost the game Restart");
			inicialCounter.text = "You Lose!!!";
			inicialCounter.gameObject.SetActive (true);
			foreach (var item in audioList) {
				item.Stop ();
				//tocar musica de perda
				if (Input.anyKeyDown) {
					print ("restart Level");
					SceneLoader.singleton.RestartGame ();
				}
			}
		}


	}


	void Update(){
		if (ReadyToPlay) {
			
		
			if (CheckWin () && !win) {
			
				win = true;
				inicialCounter.text = "You Win!!!";
				inicialCounter.gameObject.SetActive (true);
				audioSWin.Play ();
			}


			if (win) {
				float ping = Mathf.Sin (Time.fixedTime * 5);
				mainCamera.transform.RotateAround (mainCamera.transform.position, Vector3.up, ping / 20);

				audioS.Stop ();
				audioSStreak3.Stop ();

				timerWin += Time.fixedDeltaTime;
				if (timerWin > Random.Range (0.5f, 1)) {
					for (int i = 0; i < 40; i++) {
						float x = Random.Range (-7, 2);
						float y = Random.Range (-4, 4);
						float z = Random.Range (-4, 4);
						GameObject p = (GameObject)Instantiate (lineList [0].GetComponent<LineController> ().praticlePrefab, this.transform.position + new Vector3 (x, y, z), Quaternion.identity);
					}
					timerWin = 0;
				}
		
			}

			if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.JoystickButton7)) {
				print ("Pause");
				if (!SceneLoader.singleton.pausedGame) {
					SceneLoader.singleton.PauseGame ();
				} else {
					SceneLoader.singleton.ResumeGame ();
					SceneLoader.singleton.pausedGame = false;
				}

			}

			if (Input.GetKeyDown (KeyCode.UpArrow) && activeLine != 0) {
				foreach (var item in lineList) {
					lineList [activeLine].GetComponent<LineController> ().isActive = false;
				}

				activeLine--;

				if (!lineList [activeLine].GetComponent<LineController> ().missedTiming) {
					lineList [activeLine].GetComponent<LineController> ().isActive = true;
				}
			}
			if (Input.GetKeyDown (KeyCode.DownArrow) && activeLine != lineList.Count - 1) {
				foreach (var item in lineList) {
					lineList [activeLine].GetComponent<LineController> ().isActive = false;
				}

				activeLine++;


				if (!lineList [activeLine].GetComponent<LineController> ().missedTiming) {
					lineList [activeLine].GetComponent<LineController> ().isActive = true;
				}

			}
			//pause

			if (Input.GetAxis ("Vertical") > 0.5f && !pressedVerticalAxis && activeLine != 0) {
				foreach (var item in lineList) {
					lineList [activeLine].GetComponent<LineController> ().isActive = false;
				}

				if (!pressedVerticalAxis) {
					activeLine--;
				}
				pressedVerticalAxis = true;

				if (!lineList [activeLine].GetComponent<LineController> ().missedTiming) {
					lineList [activeLine].GetComponent<LineController> ().isActive = true;
				}
			}
			if (Input.GetAxis ("Vertical") < -0.5f && !pressedVerticalAxis && activeLine != lineList.Count - 1) {
				foreach (var item in lineList) {
					lineList [activeLine].GetComponent<LineController> ().isActive = false;
				}

				if (!pressedVerticalAxis) {
					activeLine++;
				}
				pressedVerticalAxis = true;
				if (!lineList [activeLine].GetComponent<LineController> ().missedTiming) {
					lineList [activeLine].GetComponent<LineController> ().isActive = true;
				}
			}

			if (Input.GetAxis ("Vertical") == 0) {
				pressedVerticalAxis = false;
			}
		} 

	}
	public void incrementStreakScore(){
		float combo = 1 + ((float)streakCount / 10);
		streak = false;
		print (scoreSingleHit * streakCount*combo);
		score += scoreSingleHit * streakCount*combo;
	}
	public bool CheckWin(){

		
		int count = 0;
		foreach (var item in lineList) {
			
			if (item.GetComponent<LineController> ().completedLine) {
				count++;

			}

		}
		CountCompleteLine = count;
		if (count == lineList.Count) {
			return true;
		} else {
			return false;
		}
	}
	public int ConvertKey2Int(KeyCode key){
		return (int)key;
	}

	public void StartCounter(){
		pauseScreen = SceneLoader.singleton.pauseMenu;
		TimerReady += Time.fixedDeltaTime;
		inicialCounter.text = Mathf.Floor(4-TimerReady).ToString();
		if (TimerReady >=3) {
			inicialCounter.gameObject.SetActive (false);
			ReadyToPlay = true;
		}
	}


}
