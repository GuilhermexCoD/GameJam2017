using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public static SceneLoader singleton;

	public GameObject pauseMenu;

	public bool pausedGame;

	void Awake(){
		
		if (singleton != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			singleton = this;
		}
//		DontDestroyOnLoad(this.gameObject);
	}

	void Update ()
	{
//		float timeShow = Time.time;
//		Debug.Log (timeShow);
	}

	public void PlayGame ()
	{
		SceneLoad (1);
	}

	public void PauseGame ()
	{
		pauseMenu.SetActive (true);
		pausedGame = true;
		Time.timeScale = 0;
	}

	public void ResumeGame ()
	{
		Time.timeScale = 1;

		pauseMenu.SetActive (false);
	}

	public void RestartGame ()
	{
		Time.timeScale = 1;
		SceneLoad (1);
	}

	public void QuitGame ()
	{
		Time.timeScale = 1;
		SceneLoad (0);
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}

	public void Credits ()
	{
		SceneLoad (2);
	}

	void SceneLoad (int index)
	{
		SceneManager.LoadScene (index);
	}
}
