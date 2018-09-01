using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject pauseMenuUI;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (GameIsPaused) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}

	public void Resume () {
		GameIsPaused = false;
		// Set pausemenu game object to active
		pauseMenuUI.SetActive (false);
		// Freeze game time
		Time.timeScale = 1f;
	}

	void Pause () {
		GameIsPaused = true;
		// Set pausemenu game object to active
		pauseMenuUI.SetActive (true);
		// Freeze game time
		Time.timeScale = 0f;
	}

	public void LoadMenu () {
		Debug.Log ("loading menu..");
		// TODO use const
		// TODO build Main Menu Scene
		// EditorSceneManager.LoadScene ("Menu");
		// Makes sure timeScale is back to 1f etc
		Resume ();
	}

	public void QuitGame () {
		Debug.Log ("Quitting Game - does nothing in editor, hence this log");
		Application.Quit ();
	}
}