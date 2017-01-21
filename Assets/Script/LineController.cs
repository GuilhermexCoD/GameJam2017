using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public bool completedLine;
    public bool missedTiming;
    public bool isActive;
    public List<KeyCode> sequence = new List<KeyCode>();
    public List<GameObject> Characters = new List<GameObject>();
    public float missTimer;
    public int activeCell;
    public float maxMissTime;


    void Awake()
    {
        maxMissTime = 0;
        activeCell = 0;
        missTimer = 0;
        completedLine = false;
        missedTiming = false;
        isActive = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            
            Characters.Add(this.transform.GetChild(i).gameObject);
            sequence.Add(randomizeKey());
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //If line is active
        if (isActive)
        {
            //Mostrar teclas
            print("Char to press: " + sequence[activeCell]);
            //For each valid key
            for(int i = 0; i<RhythmController.singleton.validKeys.Count-1; i++)
            {
                if (Input.GetKeyDown(RhythmController.singleton.validKeys[i]) && (sequence[activeCell] == RhythmController.singleton.validKeys[i]))
                {
                    if (RhythmController.singleton.action)
                    {
                        RhythmController.singleton.timer.text = "Acertou";
                        if(activeCell < sequence.Count) activeCell++;
                        //Acertou, colocar pontos
                        missedTiming = false;
                    }
                    else
                    {
                        restartLine();
                        missedTiming = true;
                        //bloquear linha por tempo
                    }
                }
            }
        }

        if(missedTiming)
        {
            missTimer += Time.fixedDeltaTime;
            if(missTimer >= maxMissTime)
            {
                restartLine();
            }
        }
    }

    public KeyCode randomizeKey()
    {
        return RhythmController.singleton.validKeys[Random.Range(0, RhythmController.singleton.validKeys.Count-1)];
    }

    public void restartLine()
    {
        activeCell = 0;
        missTimer = 0;
        completedLine = false;
        missedTiming = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            sequence.Add(randomizeKey());
        }
    }
}
