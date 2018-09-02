using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnarPanel : MonoBehaviour
{

  public void GoToMainMenu()
  {
    SceneManager.LoadScene(0);
  }

}