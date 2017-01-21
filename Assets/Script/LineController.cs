using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
	public int myIndex;
    public bool completedLine;
    public bool missedTiming;
    public bool isActive;
    public List<KeyCode> sequence = new List<KeyCode>();
    public List<GameObject> Characters = new List<GameObject>();
    float missTimer;
    public int activeCell;
    public float maxMissTime;





    void Start()
    {
		
        activeCell = 0;
        missTimer = 0;
        completedLine = false;
        missedTiming = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            
            Characters.Add(this.transform.GetChild(i).gameObject);
            sequence.Add(randomizeKey());
			Characters [i].GetComponent<TorcidaMove> ().ChooseKey (sequence[i]);
        }



    }
    // Update is called once per frame
    void FixedUpdate()
    {
		if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
			print ("YYYYYYYYYYYYYY");
		}

        //If line is active
        if (isActive)
        {
            //Mostrar teclas
            print("Char to press: " + sequence[activeCell]);
            RhythmController.singleton.timer.text = sequence[activeCell].ToString();
            //For each valid key
            for (int i = 0; i<RhythmController.singleton.validKeys.Count; i++)
            {
				if (Input.GetKeyDown(RhythmController.singleton.validKeys[i]) || Input.GetKeyDown(RhythmController.singleton.validKeysJoystick[i])  && (sequence[activeCell] == RhythmController.singleton.validKeys[i]))
                {
                    print("Apertei certo");
                    if (RhythmController.singleton.action)
                    {
						Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.green;
                        RhythmController.singleton.timer.text = "Acertou";
                        if (activeCell < sequence.Count-1) activeCell++;
                        else
                        {
                            activeCell = 0;
                            restartLine();
                            
                            print("GANHOU CARALHO");
                        }
                        //Acertou, colocar pontos, levantar placa do proximo,etc..
                        print("Acertou");

                        missedTiming = false;
                    }
                    else
                    {
                        print("Errou");
						Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
                        restartLine();
                        missedTiming = true;
                        //bloquear linha por tempo, resetar animacoes, etc...
                        isActive = false;
                    }
                }
				else if(Input.GetKeyDown(RhythmController.singleton.validKeys[i]) || Input.GetKeyDown(RhythmController.singleton.validKeysJoystick[i]))
                {
                    print("Errou");
					Characters [activeCell].GetComponent<TorcidaMove> ().boardSprite.color = Color.red;
                    restartLine();
                    missedTiming = true;
                    isActive = false;
                    //bloquear linha por tempo
                }
            }
        }

        if(missedTiming)
        {
            missTimer += Time.fixedDeltaTime;
            if(missTimer >= maxMissTime)
            {
                restartLine();
                print("Reset timer");
            }
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
        activeCell = 0;
        missTimer = 0;
        completedLine = false;
        missedTiming = false;
        sequence.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            sequence.Add(randomizeKey());
        }
    }
}
