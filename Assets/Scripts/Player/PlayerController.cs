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

  [SerializeField]
  private BiteSystem biteSystem;

  [SerializeField]
  private PlayerRenderer playerRenderer;

  private GameInput input;
  private Rigidbody2D parentRb;

  int castPower = 1;

  private State state = State.IDLE;

  // Use this for initialization
  void Start()
  {
    this.parentRb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    this.input = FindObjectOfType<GameInput>();
    this.biteSystem.OnBiteStartEvent += this.OnBiteStart;
    this.biteSystem.OnBiteEndEvent += this.OnBiteEnd;
  }

  // Update is called once per frame
  void Update()
  {
    if (state == State.IDLE || state == State.CHARGING)
    {
      ProcessMovement();
      ProcessCast();
    }
    else if (state == State.CASTED)
    {
      ProcessCastedInput();
    }
  }

  void ProcessMovement()
  {
    if (this.state == State.IDLE)
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
    else
    {
      SetVelocityX(0);
    }
  }

  void ProcessCast()
  {
    if (input.GetCastDown() && this.state == State.IDLE)
    {
      castSystem.StartCast();
      SetState(State.CHARGING);
    }
    if (input.GetCastUp() && this.state == State.CHARGING)
    {
      castSystem.EndCast();
      SetState(State.CASTING);
    }
  }

  void ProcessCastedInput()
  {
    if (input.GetCastDown())
    {
      if (biteSystem.IsBiteHappening())
      {
        biteSystem.OnHookSet();
      }
      else
      {
        // TODO: trigger game over
        SetState(State.IDLE);
      }
    }
  }

  /* A fish has been hooked! */
  private void OnHookSet()
  {
    // Debug.Log("OnHookSet!");
    // SetState(State.HOOKED);
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

  public void NotifyCasted()
  {
    SetState(State.CASTED);
  }

  private void OnBiteStart()
  {

  }

  private void OnBiteEnd(bool hooked)
  {
    if (!hooked)
    {
      SetState(State.IDLE);
    }
    else
    {
      SetState(State.HOOKED);
    }
  }

  private void SetState(State state)
  {
    Debug.Log("State change from " + this.state + " to " + state);
    this.state = state;
    biteSystem.SetCasted(state == State.CASTED);
    playerRenderer.SetState(state);
  }

  public enum State
  {
    IDLE,
    CHARGING,
    CASTING,
    CASTED,
    HOOKED,

  }
}
