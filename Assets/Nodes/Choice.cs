using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Choice", menuName = "DialogChoice")]
public class Choice : ScriptableObject
{

  [SerializeField]
  public string text;

  [SerializeField]
  public Node result;
}
