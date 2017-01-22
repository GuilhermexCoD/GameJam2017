using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class LineController : MonoBehaviour
{
	public enum TorcedorState
	{
		idle,
		standing
	}

   public float frequency;
    float amplitude;
    public int myIndex;
    public bool completedLine;
    public bool missedTiming;
    public bool isActive;
    public List<KeyCode> sequence = new List<KeyCode>();
    public List<GameObject> Characters = new List<GameObject>();
    float missTimer,winTimer,decayTimer, holaTimer;
    public int activeCell;
    public float maxMissTime,maxWinTime,maxDecayTime;
	float maxMissTimeStart,maxWinTimeStart,maxDecayTimeStart;
	public bool pressed;
    bool vibrate;
    float vibrateTime;
    float waveDelay;
	bool offColor;

	float win=1,loss =1;
	float winLossRation;
    bool everyoneStopped;
    int waveCount;

    float testAmplitude;

    float testFrequency;
    int countStop;
    bool stopTheHola;
	public GameObject praticlePrefab;


    void Start()
    {
        stopTheHola = true;
        countStop = 0;
        testAmplitude = 1f;
        testFrequency = 3f;
        waveDelay = 0.1f;
        waveCount = 0;
        holaTimer = 0;
        everyoneStopped = false;
        frequency = 1;
        amplitude = 1;
        maxMissTimeStart = maxMissTime;
		maxWinTimeStart = maxWinTime;
		maxDecayTimeStart = maxDecayTime;
        vibrate = false;
        vibrateTime = 0;
        activeCell = 0;
        missTimer = 0;
        completedLine = false;
        missedTiming = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            
            Characters.Add(this.transform.GetChild(i).gameObject);
            sequence.Add(randomizeKey());
			Characters [i].GetComponent<TorcidaMove> ().ChooseKey (sequence[i]);
            if(Characters[i].GetComponent<TorcidaMove>().anim != null)
			Characters[i].GetComponent<TorcidaMove>().anim.SetInteger("State",(int)TorcedorState.idle);
        }



    }
    // Update is called once per frame
    void Update()
    {

		winLossRation = (float)(win / loss);
		if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
		//	print ("YYYYYYYYYYYYYY");
		}

        //If line is active
		if (isActive) {
			decayTimer = 0;
			if (offColor) {
				foreach (var item in Characters) {
					item.GetComponent<CharacterCreation> ().TurnOnColors ();
				}
				offColor = false;
			}

			//Mostrar teclas
	//		print ("Char to press: " + sequence [activeCell]);
			RhythmController.singleton.timer.text = sequence [activeCell].ToString ();
			//For each valid key
			for (int i = 0; i < RhythmController.singleton.validKeys.Count; i++) {
				if (Input.GetKeyDown (RhythmController.singleton.validKeys [i]) || Input.GetKeyDown (RhythmController.singleton.validKeysJoystick [i]) && (sequence [activeCell] == RhythmController.singleton.validKeys [i])) {
	//				print ("Apertei certo");
					if (RhythmController.singleton.action && !pressed) {
						//levantar
						Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.standing);
						Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.green;
						Characters [activeCell].GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [0];
						GameObject p = (GameObject)Instantiate (praticlePrefab,Characters[activeCell].transform.position +new Vector3(0,1.3f,0),Quaternion.identity);

						RhythmController.singleton.timer.text = "Acertou";
						vibrate = true;
						vibrateTime = 0;
						GamePad.SetVibration (PlayerIndex.One, 1, 1);

						if (activeCell < sequence.Count - 1)
							activeCell++;
						else {
							activeCell = 0;
							//nao pode resetar de imediato
//                            restartLine();
							completedLine = true;
						//	print ("GANHOU CARALHO");
							win++;
                            DoWave(10f, 2f);

                        }
						//Acertou, colocar pontos, levantar placa do proximo,etc..
			//			print ("Acertou");
						pressed = true;
						missedTiming = false;
					} else {
		//				print ("Errou");
						loss++;
						Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.idle);
						Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
						Characters [activeCell].GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [1];
						Miss ();
						missedTiming = true;
						//bloquear linha por tempo, resetar animacoes, etc...
						isActive = false;
					}
				} else if (Input.GetKeyDown (RhythmController.singleton.validKeys [i]) || Input.GetKeyDown (RhythmController.singleton.validKeysJoystick [i])) {
					Characters [activeCell].GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [1];
					Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.idle);
		//			print ("Errou");
					Miss ();
					loss++;
					Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
					missedTiming = true;
					isActive = false;
					//bloquear linha por tempo
				}
			}
		} else {
			if (!completedLine && ! missedTiming) {
				decayTimer += Time.fixedDeltaTime;
				if (decayTimer >= (maxDecayTime/2)) {
					if (activeCell!=0) {
						activeCell--;
					}
					maxDecayTime = maxDecayTimeStart * winLossRation;
					Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.idle);
					Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.white;
					Characters [activeCell].GetComponent<TorcidaMove> ().ChooseKey (sequence[activeCell]);


					decayTimer = 0;
				}
			}

		}
		if (!offColor && !isActive) {
			
			foreach (var item in Characters) {
				item.GetComponent<CharacterCreation> ().TurnOffColors ();
			}
			offColor = true;
		}

		if (completedLine) {
			winTimer += Time.fixedDeltaTime;
			if (winTimer >=maxWinTime) {
				maxWinTime = maxWinTimeStart * winLossRation;
                StopWave();
				restartLine ();
			}
		}

        if(missedTiming)
        {
            missTimer += Time.fixedDeltaTime;
            if(missTimer >= maxMissTime)
            {
				maxMissTime = maxMissTimeStart/ winLossRation;
                restartLine();
    //            print("Reset timer");
            }
        }
        
        if (vibrateTime >= 1 && vibrate)
        {
    //        print("ENTREI");
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
            vibrate = false;
        }
        vibrateTime += Time.fixedDeltaTime;

        if (Input.GetKeyDown(KeyCode.C))
        {
            DoWave(10f, 2f);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeWave(10f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            testFrequency += 0.5f;
            testAmplitude += 0.2f;
            ChangeWave(testFrequency, testAmplitude);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            StopWave();
        }

        if (everyoneStopped && !stopTheHola)
        {
            holaTimer += Time.fixedDeltaTime;
            if (holaTimer >= waveDelay)
            {
                Characters[waveCount].GetComponent<TorcidaMove>().ChangeFrequency(frequency);
                Characters[waveCount].GetComponent<TorcidaMove>().ChangeAmplitude(amplitude);
                Characters[waveCount].GetComponent<TorcidaMove>().StartMoving();
                waveCount++;
                if (waveCount == Characters.Count)
                {
                    waveCount = 0;
                }
                holaTimer = 0;
            }
        }
        else if (!everyoneStopped)
        {
            foreach (var item in Characters)
            {
                if (item.GetComponent<TorcidaMove>().notMoving == false)
                {
                    countStop = 0;
                    everyoneStopped = false;
                    break;
                }
                else if (Characters.IndexOf(item) == Characters.Count - 1)
                {
                    countStop++;
                }
            }
            if (countStop == Characters.Count) everyoneStopped = true;
            else everyoneStopped = false;
        }
    }

    public KeyCode randomizeKey()
    {
        return RhythmController.singleton.validKeys[Random.Range(0, RhythmController.singleton.validKeys.Count)];
    }

    public void restartLine()
    {
		Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
		if (RhythmController.singleton.activeLine == myIndex) {
			isActive = true;
		}
		decayTimer = 0;
        activeCell = 0;
		winTimer = 0;
        missTimer = 0;
        completedLine = false;
        missedTiming = false;
        sequence.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
			Characters [i].GetComponent<TorcidaMove> ().boardSprite.color = Color.white;
            sequence.Add(randomizeKey());
			Characters [i].GetComponent<TorcidaMove> ().ChooseKey (sequence[i]);
			Characters[i].GetComponent<TorcidaMove>().anim.SetInteger("State",(int)TorcedorState.idle);
        }
    }
	public void Miss(){

		foreach (var item in Characters) {
			
			item.GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.idle);
			item.GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
			item.GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [1];
		}
	}

    public void DoWave(float _frequency, float _amplitude)
    {
        stopTheHola = false;
        if (!everyoneStopped)
        {
            foreach (var item in Characters)
            {
                item.GetComponent<TorcidaMove>().StopMoving();
                frequency = _frequency;
                amplitude = _amplitude;
            }

        }

    }

    public void ChangeWave(float _frequency, float _amplitude)
    {
        foreach (var item in Characters)
        {
            frequency = _frequency;
            amplitude = _amplitude;
        }
    }

    public void StopWave()
    {
        stopTheHola = true;
        foreach (var item in Characters)
        {
            //item.GetComponent<TorcidaMove>().ChangeFrequency(0f);
            item.GetComponent<TorcidaMove>().ReturnToStartPos();
            item.GetComponent<TorcidaMove>().StopMoving();
            item.GetComponent<TorcidaMove>().stop = true;
            item.GetComponent<TorcidaMove>().timer = 0;
            waveCount = 0;
            //      item.GetComponent<TorcidaMove>().notMoving = true;

            everyoneStopped = false;
        }
    }
}
