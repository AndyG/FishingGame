using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YerDeadPanel : MonoBehaviour {

	public void GoToMainMenu () {

		SceneManager.LoadScene (0);
	}

	public void QuitGame () {
		Application.Quit ();
	}
}