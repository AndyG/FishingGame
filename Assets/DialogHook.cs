using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHook : MonoBehaviour
{

  public bool isDialogShowing;

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnDialogShowing()
  {
    this.isDialogShowing = true;
  }

  public void OnDialogGone()
  {
    this.isDialogShowing = false;
  }
}
