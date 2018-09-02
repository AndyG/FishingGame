using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{

  private SpriteRenderer spriteRenderer;

  private PlayerController.State state;

  private int power = 1;

  private ChargeCycleColors currentChargeColorMod = ChargeCycleColors.RED;

  [SerializeField]
  private float chargeCycleRate = 0.05f;
  [SerializeField]
  [Range(0, 0.75f)]
  private float chargeCycleStep = 0.025f;
  private int chargeCycleDirection = -1;

  private float scaleMinAddition = 0f;

  [SerializeField]
  [Range(0f, 0.2f)]
  private float scaleMaxAddition = 0.2f;
  private float currentScaleAddition = 0f;

  [SerializeField]
  [Range(0f, 0.1f)]
  private float scaleStep = 0.1f;

  private int scaleDirection = 1;

  [SerializeField]
  [Range(0, 1f)]
  private float minAlpha = 0.1f;
  [Range(0, 0.1f)]
  private float alphaStep = 0.05f;


  [Header("Sprites")]
  [SerializeField]
  private Sprite idleSprite;

  [SerializeField]
  private Sprite chargingSprite;

  [SerializeField]
  private Sprite castingSprite;

  // Use this for initialization
  void Start()
  {
    this.spriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if (state == PlayerController.State.CHARGING)
    {
      if (this.power > 2)
      {
        OscillateSize();
      }
      else
      {
        this.transform.localScale = new Vector3(1, 1, 1);
      }

      if (this.power > 4)
      {
        CycleColor();
      }
      else
      {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
      }

      FadeAlphaIn();
      SetChargingSprite();
    }
    else if (state == PlayerController.State.CASTED)
    {
      FadeAlphaOut();
      spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a);
      this.transform.localScale = new Vector3(1, 1, 1);
      SetIdleSprite();
    }
    else if (state == PlayerController.State.CASTING)
    {
      FadeAlphaIn();
      spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a);
      this.transform.localScale = new Vector3(1, 1, 1);
      SetCastingSprite();
    }
    else // idle
    {
      spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a);
      FadeAlphaIn();
      this.transform.localScale = new Vector3(1, 1, 1);
      SetIdleSprite();
    }
  }

  private void FadeAlphaOut()
  {
    Color curColor = spriteRenderer.color;
    if (curColor.a > minAlpha)
    {
      float newAlpha = curColor.a - alphaStep;
      spriteRenderer.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
  }

  private void FadeAlphaIn()
  {
    Color curColor = spriteRenderer.color;
    if (curColor.a < 1f)
    {
      float newAlpha = curColor.a + alphaStep;
      spriteRenderer.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
  }

  public void SetState(PlayerController.State state)
  {
    this.state = state;
  }

  private void OscillateSize()
  {
    if (currentScaleAddition > scaleMaxAddition)
    {
      currentScaleAddition = scaleMaxAddition;
      scaleDirection = -1;
    }
    else if (currentScaleAddition < scaleMinAddition)
    {
      currentScaleAddition = scaleMinAddition;
      scaleDirection = 1;
    }

    Vector3 scaleAddition = new Vector3(currentScaleAddition, currentScaleAddition, 0f);
    this.transform.localScale = new Vector3(1f, 1f, 1f) + scaleAddition;
    currentScaleAddition += scaleStep * scaleDirection;
  }

  private void CycleColor()
  {
    Color currentColor = spriteRenderer.color;
    if (currentChargeColorMod == ChargeCycleColors.RED)
    {
      float redComponent = currentColor.r;
      float newRedComponent = redComponent + (chargeCycleDirection * chargeCycleStep);
      if (newRedComponent < 0f)
      {
        newRedComponent = 0f;
        chargeCycleDirection = 1;
        currentChargeColorMod = ChargeCycleColors.GREEN;
      }
      else if (newRedComponent > 1f)
      {
        newRedComponent = 1f;
        currentChargeColorMod = ChargeCycleColors.GREEN;
        chargeCycleDirection = -1;
      }
      Color newColor = new Color(newRedComponent, currentColor.g, currentColor.b, spriteRenderer.color.a);
      spriteRenderer.color = newColor;
    }
    else if (currentChargeColorMod == ChargeCycleColors.GREEN)
    {
      float greenComponent = currentColor.g;
      float newGreenComponent = greenComponent + (chargeCycleDirection * chargeCycleStep);
      if (newGreenComponent < 0f)
      {
        newGreenComponent = 0f;
        chargeCycleDirection = 1;
        currentChargeColorMod = ChargeCycleColors.BLUE;
      }
      else if (newGreenComponent > 1f)
      {
        newGreenComponent = 1f;
        currentChargeColorMod = ChargeCycleColors.BLUE;
        chargeCycleDirection = -1;
      }
      Color newColor = new Color(currentColor.r, newGreenComponent, currentColor.b, spriteRenderer.color.a);
      spriteRenderer.color = newColor;
    }
    else if (currentChargeColorMod == ChargeCycleColors.BLUE)
    {
      float blueComponent = currentColor.b;
      float newBlueComponent = blueComponent + (chargeCycleDirection * chargeCycleStep);
      if (newBlueComponent < 0f)
      {
        newBlueComponent = 0f;
        chargeCycleDirection = 1;
        currentChargeColorMod = ChargeCycleColors.RED;
      }
      else if (newBlueComponent > 1f)
      {
        newBlueComponent = 1f;
        currentChargeColorMod = ChargeCycleColors.RED;
        chargeCycleDirection = -1;
      }
      Color newColor = new Color(currentColor.r, currentColor.g, newBlueComponent, spriteRenderer.color.a);
      spriteRenderer.color = newColor;
    }
  }

  public void SetPower(int power)
  {
    this.power = power;
  }

  private void SetIdleSprite()
  {
    spriteRenderer.sprite = idleSprite;
  }

  private void SetCastingSprite()
  {
    spriteRenderer.sprite = castingSprite;
  }

  private void SetChargingSprite()
  {
    spriteRenderer.sprite = chargingSprite;
  }

  private enum ChargeCycleColors
  {
    RED,
    GREEN,
    BLUE,
  }

}