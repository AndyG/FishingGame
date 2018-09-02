using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSystem : MonoBehaviour
{

  public delegate void OnPowerChange(int power);
  public event OnPowerChange OnPowerChangeEvent;

  [SerializeField]
  private GameObject castObjectPrototype;

  private float timeSinceCastStart = 0f;

  private bool isCastStarted = false;

  private int maxPower = 3;

  // Update is called once per frame
  void Update()
  {
    if (isCastStarted)
    {
      timeSinceCastStart += Time.deltaTime;
      int power = GetPowerFromCast();
      OnPowerChangeEvent(power);
    }
  }

  public void StartCast()
  {
    if (!isCastStarted)
    {
      timeSinceCastStart = 0f;
      isCastStarted = true;
    }
  }

  public void EndCast()
  {
    if (isCastStarted)
    {
      isCastStarted = false;
      int power = GetPowerFromCast();

      if (power < 1 || power > 5)
      {
        Debug.LogError("bad power");
      }
      StartCoroutine(this.StartFallingBobber(power));
    }
  }

  public void UnlockRainbowCharge()
  {
    maxPower = 5;
  }

  private IEnumerator StartFallingBobber(int power)
  {
    yield return new WaitForSeconds(0.5f);

    float scale = 1f / power;
    GameObject go = GameObject.Instantiate(castObjectPrototype, this.transform.position, Quaternion.identity);
    go.transform.localScale = new Vector3(scale, scale, 1);

    go.GetComponent<FallingBobber>().SetTargetPosY(GetDistanceYFromPower(power));
  }

  private int GetPowerFromCast()
  {
    if (timeSinceCastStart > 2.8)
    {
      return Mathf.Min(maxPower, 5);
    }
    else if (timeSinceCastStart > 2.3)
    {
      return Mathf.Min(maxPower, 4);
    }
    else if (timeSinceCastStart > 1.4)
    {
      return 3;
    }
    else if (timeSinceCastStart > 0.7)
    {
      return 2;
    }
    else
    {
      return 1;
    }
  }

  private float GetDistanceYFromPower(int power)
  {
    switch (power)
    {
      case 1:
        return -0.756f;
      case 2:
        return -0.447f;
      case 3:
        return -0.143f;
      case 4:
        return 0.211f;
      case 5:
        return 0.509f;
    }

    return 0f;
  }
}