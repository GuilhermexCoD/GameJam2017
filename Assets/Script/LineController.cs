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

	public int myIndex;
    public bool completedLine;
    public bool missedTiming;
    public bool isActive;
    public List<KeyCode> sequence = new List<KeyCode>();
    public List<GameObject> Characters = new List<GameObject>();
    float missTimer,winTimer,decayTimer;
    public int activeCell;
    public float maxMissTime,maxWinTime,maxDecayTime;
	float maxMissTimeStart,maxWinTimeStart,maxDecayTimeStart;
	public bool pressed;
    bool vibrate;
    float vibrateTime;

	bool offColor;

	float win=1,loss =1;
	float winLossRation;


    void Start()
    {
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
    void FixedUpdate()
    {

		winLossRation = (float)(win / loss);
		if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
			print ("YYYYYYYYYYYYYY");
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
			print ("Char to press: " + sequence [activeCell]);
			RhythmController.singleton.timer.text = sequence [activeCell].ToString ();
			//For each valid key
			for (int i = 0; i < RhythmController.singleton.validKeys.Count; i++) {
				if (Input.GetKeyDown (RhythmController.singleton.validKeys [i]) || Input.GetKeyDown (RhythmController.singleton.validKeysJoystick [i]) && (sequence [activeCell] == RhythmController.singleton.validKeys [i])) {
					print ("Apertei certo");
					if (RhythmController.singleton.action && !pressed) {
						//levantar
						Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.standing);
						Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.green;
						Characters [activeCell].GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [0];
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
							print ("GANHOU CARALHO");
							win++;
						}
						//Acertou, colocar pontos, levantar placa do proximo,etc..
						print ("Acertou");
						pressed = true;
						missedTiming = false;
					} else {
						print ("Errou");
						loss++;
						Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.idle);
						Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
						Characters [activeCell].GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [1];
						missedTiming = true;
						//bloquear linha por tempo, resetar animacoes, etc...
						isActive = false;
					}
				} else if (Input.GetKeyDown (RhythmController.singleton.validKeys [i]) || Input.GetKeyDown (RhythmController.singleton.validKeysJoystick [i])) {
					Characters [activeCell].GetComponent<TorcidaMove> ().keySprite.sprite = RhythmController.singleton.feedbackSprite [1];
					Characters [activeCell].GetComponent<TorcidaMove> ().anim.SetInteger ("State", (int)TorcedorState.idle);
					print ("Errou");
					loss++;
					Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
					missedTiming = true;
					isActive = false;
					//bloquear linha por tempo
				}
			}
		} else {
			if (!completedLine) {
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
                print("Reset timer");
            }
        }
        
        if (vibrateTime >= 1 && vibrate)
        {
            print("ENTREI");
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
            vibrate = false;
        }
        vibrateTime += Time.fixedDeltaTime;
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


}
