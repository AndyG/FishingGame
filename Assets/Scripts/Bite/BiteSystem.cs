using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteSystem : MonoBehaviour
{

  [SerializeField]
  private float minBiteWaitTime = 2f;

  [SerializeField]
  private float maxBiteWaitTime = 5f;

  [SerializeField]
  private float biteDuration = 0.3f;

  private bool isCasted = false;

  private bool isBiteHappening = false;


  private IEnumerator currentCoroutine;

  public delegate void OnBiteStart();
  public event OnBiteStart OnBiteStartEvent;

  public delegate void OnBiteEnd(bool hooked);
  public event OnBiteEnd OnBiteEndEvent;

  public bool IsBiteHappening()
  {
    return this.isBiteHappening;
  }

  public void SetCasted(bool isCasted)
  {
    bool wasCasted = this.isCasted;
    this.isCasted = isCasted;

    if (!wasCasted && isCasted)
    {
      float timeTilBite = Random.Range(minBiteWaitTime, maxBiteWaitTime);
      this.currentCoroutine = WaitForBite(timeTilBite);
      StartCoroutine(currentCoroutine);
    }
    else if (!isCasted)
    {
      if (currentCoroutine != null)
      {
        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
      }
    }
  }

  public void OnHookSet()
  {
    if (currentCoroutine != null)
    {
      StopCoroutine(currentCoroutine);
      currentCoroutine = null;
    }
    SetBiteHappening(false, true);
  }

  private IEnumerator WaitForBite(float timeTilBite)
  {
    yield return new WaitForSeconds(timeTilBite);
    SetBiteHappening(true, false);
    yield return new WaitForSeconds(biteDuration);
    SetBiteHappening(false, false);
    this.currentCoroutine = null;
  }

  private void SetBiteHappening(bool isBiteHappening, bool hooked)
  {
    this.isBiteHappening = isBiteHappening;

    if (isBiteHappening)
    {
      OnBiteStartEvent();
    }
    else
    {
      OnBiteEndEvent(hooked);
    }
  }
}
