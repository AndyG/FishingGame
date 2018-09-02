using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvasManager : MonoBehaviour
{

  private bool disappearing = false;
  private Animator animator;

  // Use this for initialization
  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    if (disappearing)
    {
      return;
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      disappearing = true;
      animator.SetBool("Disappearing", true);
    }
  }

  private void OnDisappeared()
  {
    FindObjectOfType<PlayerController>().SetReady();
  }
}
