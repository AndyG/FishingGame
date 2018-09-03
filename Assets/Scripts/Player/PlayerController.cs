using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  public Animator animator;

  private int lastCastPower = 0;

  private int caughtFishCount = 0;

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

  private State state = State.IDLE;

  private DialogRenderer dialogRenderer;
  private AudioSource playerAudioSource;

  [Header("Audio")]
  [SerializeField]
  private AudioClip castAudioClip;
  [SerializeField]
  private AudioClip chargeAudioClip;
  [SerializeField]
  private AudioClip caughtAudioClip;

  [SerializeField]
  private float dadVolume;

  [SerializeField]
  private AudioClip dadAudioClip;

  [SerializeField]
  private Node innerPowerDialog;

  [SerializeField]
  private Node introDialog;

  [SerializeField]
  private Node castFurtherConversation;

  public bool isReady = false;

  // Use this for initialization
  void Start()
  {
    this.parentRb = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    this.input = FindObjectOfType<GameInput>();
    this.dialogRenderer = FindObjectOfType<DialogRenderer>();
    this.biteSystem.OnBiteStartEvent += this.OnBiteStart;
    this.biteSystem.OnBiteEndEvent += this.OnBiteEnd;
    this.castSystem.OnPowerChangeEvent += this.OnPowerChange;
    this.dialogRenderer.OnFishSuccessEvent += this.OnFishSuccess;
    this.dialogRenderer.OnFishFailureEvent += this.OnFishFailure;
    this.dialogRenderer.OnDialogEndedEvent += this.OnDialogEnded;
    this.playerAudioSource = GetComponent<AudioSource>();
  }

  public void SetReady()
  {
    animator.SetBool("AnimateIn", true);
  }

  public void OnAnimatedIn()
  {
    StartCoroutine(PlayIntroDialog());
  }

  private void OnFishSuccess(Fish.Fishes fish)
  {
    this.SetState(State.IDLE);
    DestroyAllBobbers();
    caughtFishCount++;
    if (caughtFishCount == 3 || (caughtFishCount == 1 && FindObjectOfType<Fish>().justBoot))
    {
      PlayInnerPowerDialog();
    }

    if (fish == Fish.Fishes.DAD)
    {
      FindObjectOfType<LevelChanger>().FadeToLevel(1);
    }
  }

  private void OnFishFailure(Fish.Fishes fish)
  {
    this.SetState(State.IDLE);
    DestroyAllBobbers();
  }

  private void DestroyAllBobbers()
  {
    Bobber[] bobbers = FindObjectsOfType<Bobber>();
    for (int i = 0; i < bobbers.Length; i++)
    {
      bobbers[i].DestroyIt();
    }
  }

  private void OnDialogEnded()
  {
    this.SetState(State.IDLE);
  }
  private void PlayInnerPowerDialog()
  {
    dialogRenderer.StartConversation(innerPowerDialog);
    castSystem.UnlockRainbowCharge();
  }

  private IEnumerator PlayIntroDialog()
  {
    // fuckit
    yield return null;
    dialogRenderer.StartConversation(introDialog);
    isReady = true;
  }

  // Update is called once per frame
  void Update()
  {
    if (!isReady)
    {
      return;
    }
    if (dialogRenderer.IsDialogShowing())
    {
      return;
    }

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
      this.playerAudioSource.clip = this.chargeAudioClip;
      this.playerAudioSource.Play();
      this.playerAudioSource.loop = true;
    }
    if (input.GetCastUp() && this.state == State.CHARGING)
    {
      castSystem.EndCast();
      SetState(State.CASTING);
      this.playerAudioSource.Stop();
      this.playerAudioSource.loop = false;
      this.playerAudioSource.PlayOneShot(this.castAudioClip);
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
      Fish fishManager = FindObjectOfType<Fish>();
      if (caughtFishCount < 3 || lastCastPower > 4)
      {
        Fish.Fishes fish = fishManager.GetRandomFish();
        Node conversation = fishManager.GetConversation(fish);
        dialogRenderer.StartConversation(conversation);
        if (fish != Fish.Fishes.DAD)
        {
          this.playerAudioSource.PlayOneShot(this.caughtAudioClip);
        }
        else
        {
          this.playerAudioSource.volume = dadVolume;
          this.playerAudioSource.PlayOneShot(this.dadAudioClip);
        }
      }
      else
      {
        dialogRenderer.StartConversation(castFurtherConversation);
      }
    }
  }

  private void OnPowerChange(int power)
  {
    lastCastPower = power;
    playerRenderer.SetPower(power);
  }

  private void SetState(State state)
  {
    this.state = state;
    biteSystem.SetCasted(state == State.CASTED);
    playerRenderer.SetState(state);
    if (state != State.CASTED && state != State.HOOKED)
    {
      DestroyAllBobbers();
    }
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