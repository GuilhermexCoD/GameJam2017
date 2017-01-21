/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public class LineController : MonoBehaviour {
	/// <summary>
	/// The characters list.
	/// </summary>
	[SerializeField]
	List<GameObject> Characters = new List<GameObject>();

	float frequency;
	float amplitude;

	float timer;
	[SerializeField]
	float waveDelay;
	int waveCount;
	bool everyoneStopped;

	public bool active;

	int[] Music = new int[10];

	void Awake(){

		frequency = 1;
		amplitude = 1;
		timer = 0;
		waveCount = 0;

		//colocar todos os filhos deste objeto em uma lista no inicio do jogo
		for (int i = 0; i < this.transform.childCount; i++) {

			if (this.transform.GetChild(i).GetComponent<CharacterCreation>() != null) {
				Characters.Add (this.transform.GetChild(i).gameObject);
			}
		}


	}

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.A)) {
			DoWave (3,1);
		}
		if (everyoneStopped) {
			timer += Time.fixedDeltaTime;
			if (timer >= waveDelay) {
				Characters [waveCount].GetComponent<TorcidaMove> ().ChangeFrequency (frequency);
				Characters [waveCount].GetComponent<TorcidaMove> ().ChangeAmplitude (amplitude);
				Characters [waveCount].GetComponent<TorcidaMove> ().StartMoving ();
				waveCount++;
				if (waveCount == Characters.Count) {
					waveCount = 0;
				}
				timer = 0;
			}
		} else {
			foreach (var item in Characters) {
				if (item.GetComponent<TorcidaMove> ().notMoving == false) {
					everyoneStopped = false;
					break;
				} else if(Characters.IndexOf(item) == Characters.Count-1){
					everyoneStopped = true;
				}
			}
		}



		
	}

	public void DoWave(float _frequency,float _amplitude){
		
		if (!everyoneStopped) {
			foreach (var item in Characters) {
				item.GetComponent<TorcidaMove> ().StopMoving ();
				frequency = _frequency;
				amplitude = _amplitude;
			}
		
		} 

	}
=======
public class LineController : MonoBehaviour
{
    public bool completedLine;
    public bool missedTiming;
    public bool isActive;
    public List<int> sequence = new List<int>();
    public List<GameObject> Characters = new List<GameObject>();

    void Awake()
    {
        completedLine = false;
        missedTiming = false;
        isActive = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Characters.Add(this.transform.GetChild(i).gameObject);
            sequence.Add(Random.Range(0, 3));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(isActive)
        {
            
        }
    }
>>>>>>> Stashed changes
}
*/