using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  [Header("Movement")]
  [SerializeField]
  private float speedHoriz = 1;

  [SerializeField]
  private float topSpeedX = 5;

  [Header("Subsystems")]
  [SerializeField]
  private CastSystem castSystem;

  private GameInput input;
  private Rigidbody2D parentRb;

  int castPower = 1;

  // Use this for initialization
  void Start()
  {
    this.parentRb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    this.input = FindObjectOfType<GameInput>();
  }

  // Update is called once per frame
  void Update()
  {
    ProcessMovement();
    ProcessCast();
  }

  void ProcessMovement()
  {
    float inputHoriz = input.GetHorizontalAxis();

    if (inputHoriz == 0)
    {
      SetVelocityX(0);
    }
    else
    {
      float force = (inputHoriz) * speedHoriz;
      parentRb.AddForce(Vector2.right * force, ForceMode2D.Impulse);
    }

    CapVelocity();
  }

  void ProcessCast()
  {
    if (input.GetCastDown())
    {
      castSystem.StartCast();
    }
    if (input.GetCastUp())
    {
      castSystem.EndCast();
    }
  }

  /**
  * Various helper methods for updating position or velocity.
  */
  private void CapVelocity()
  {
    parentRb.velocity = Vector2.ClampMagnitude(parentRb.velocity, topSpeedX);
  }

  private void SetVelocityX(float x)
  {
    setVelocity(x, parentRb.velocity.y);
  }

  private void setVelocity(float x, float y)
  {
    parentRb.velocity = new Vector2(x, y);
  }
}
