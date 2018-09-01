using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{

  public float GetHorizontalAxis()
  {
    float axis = Input.GetAxis("Horizontal");
    if (axis == 0)
    {
      return 0;
    }
    else
    {
      return Mathf.Sign(axis);
    }
  }

  public float GetVerticalAxis()
  {
    return Input.GetAxis("Vertical");
  }

  public bool GetCastDown()
  {
    return Input.GetKeyDown(KeyCode.Space);
  }

  public bool GetCastUp()
  {
    return Input.GetKeyUp(KeyCode.Space);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
