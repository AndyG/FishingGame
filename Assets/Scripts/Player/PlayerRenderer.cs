using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{

  private SpriteRenderer renderer;

  private bool isCharging = false;
  private bool isCasted;

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

  // Use this for initialization
  void Start()
  {
    this.renderer = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if (isCharging)
    {
      CycleColor();
      OscillateSize();
      FadeAlphaIn();
    }
    else if (isCasted)
    {
      FadeAlphaOut();
      this.transform.localScale = new Vector3(1, 1, 1);
    }
    else
    {
      renderer.color = new Color(1f, 1f, 1f, renderer.color.a);
      FadeAlphaIn();
      this.transform.localScale = new Vector3(1, 1, 1);
    }
  }

  private void FadeAlphaOut()
  {
    Color curColor = renderer.color;
    if (curColor.a > minAlpha)
    {
      float newAlpha = curColor.a - alphaStep;
      renderer.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
  }

  private void FadeAlphaIn()
  {
    Color curColor = renderer.color;
    if (curColor.a < 1f)
    {
      float newAlpha = curColor.a + alphaStep;
      renderer.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
  }

  public void SetState(PlayerController.State state)
  {
    SetCharging(state == PlayerController.State.CHARGING);
    SetCasted(state == PlayerController.State.CASTED);
  }

  public void SetCharging(bool isCharging)
  {
    this.isCharging = isCharging;
  }

  private void SetCasted(bool isCasted)
  {
    this.isCasted = isCasted;
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
    Color currentColor = renderer.color;
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
      Debug.Log("new red component: " + newRedComponent);
      Color newColor = new Color(newRedComponent, currentColor.g, currentColor.b, renderer.color.a);
      renderer.color = newColor;
      Debug.Log("new color: " + renderer.color);
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
      Color newColor = new Color(currentColor.r, newGreenComponent, currentColor.b, renderer.color.a);
      renderer.color = newColor;
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
      Color newColor = new Color(currentColor.r, currentColor.g, newBlueComponent, renderer.color.a);
      renderer.color = newColor;
    }
  }

  private enum ChargeCycleColors
  {
    RED,
    GREEN,
    BLUE,
  }

}