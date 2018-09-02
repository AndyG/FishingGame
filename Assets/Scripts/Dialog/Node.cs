using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Node", menuName = "DialogNode")]
public class Node : ScriptableObject
{
  [SerializeField]
  public string characterName;

  [SerializeField]
  public Sprite image;

  [SerializeField]
  public List<string> text;

  public List<Choice> choices;

  public string successFishName;
  public string failureFishName;

  public Node nextNode;

  public bool HasChoices()
  {
    return choices != null && choices.Count > 0;
  }

  public int GetLineCount()
  {
    return text.Count;
  }

  public Fish.Fishes? GetSuccessFish()
  {
    if (successFishName != null)
    {
      if (successFishName == "DAD")
      {
        return Fish.Fishes.DAD;
      }
      else if (successFishName == "KAT")
      {
        return Fish.Fishes.KAT;
      }
      else if (successFishName == "JIM")
      {
        return Fish.Fishes.JIM;
      }
      else if (successFishName == "BOOT")
      {
        return Fish.Fishes.BOOT;
      }
    }

    return null;
  }

  public Fish.Fishes? GetFailureFish()
  {
    if (failureFishName != null)
    {
      if (failureFishName == "DAD")
      {
        return Fish.Fishes.DAD;
      }
      else if (failureFishName == "KAT")
      {
        return Fish.Fishes.KAT;
      }
      else if (failureFishName == "JIM")
      {
        return Fish.Fishes.JIM;
      }
      else if (failureFishName == "BOOT")
      {
        return Fish.Fishes.BOOT;
      }
    }

    return null;
  }
}
