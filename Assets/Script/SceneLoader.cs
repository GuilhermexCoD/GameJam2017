using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public GameObject pauseMenu;

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
