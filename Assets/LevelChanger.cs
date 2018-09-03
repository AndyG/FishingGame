using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	[SerializeField]
	private Animator animator;

	private int levelToLoad;

	public void FadeToLevel (int levelIndex) {
		levelToLoad = levelIndex;
		animator.SetTrigger ("FadeOutTrigger");
	}

	public void OnFadeComplete () {
		SceneManager.LoadScene (levelToLoad);
	}
}