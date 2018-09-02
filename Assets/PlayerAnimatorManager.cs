using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnAnimatedIn()
  {
    // GetComponent<Animator>().SetBool("AnimateIn", false);
    GameObject.Destroy(GetComponent<Animator>());
    FindObjectOfType<PlayerController>().OnAnimatedIn();
  }
}
