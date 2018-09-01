using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSystem : MonoBehaviour
{

  [SerializeField]
  private GameObject castObjectPrototype;

  // Update is called once per frame
  void Update()
  {

  }

  public void Cast(int power)
  {
    Debug.Log("cast power: " + power);
    if (power < 1 || power > 5)
    {
      Debug.LogError("bad power");
    }

    float scale = 1f / power;
    GameObject go = GameObject.Instantiate(castObjectPrototype, this.transform.position, Quaternion.identity);
    go.transform.localScale = new Vector3(scale, scale, 1);

    go.GetComponent<FallingBobber>().SetTargetPosY(GetDistanceYFromPower(power));
  }

  private float GetDistanceYFromPower(int power)
  {
    switch (power)
    {
      case 1: return -0.756f;
      case 2: return -0.447f;
      case 3: return -0.143f;
      case 4: return 0.211f;
      case 5: return 0.509f;
    }

    return 0f;
  }
}
