using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

  private List<Fishes> CATCHABLE_FISH;

  void Start()
  {
    CATCHABLE_FISH = GetInitialFish();
  }

  private List<Fishes> GetInitialFish()
  {
    List<Fishes> fish = new List<Fishes>();
    fish.Add(Fishes.BOOT);
    fish.Add(Fishes.JIM);
    fish.Add(Fishes.KAT);
    return fish;
  }

  public void OnFishCaught(Fishes fish)
  {
    Debug.Log("OnFishCaught: " + fish);
    CATCHABLE_FISH.Remove(fish);
    if (CATCHABLE_FISH.Count == 0)
    {
      CATCHABLE_FISH.Add(Fishes.DAD);
    }
  }

  public Fishes GetRandomFish()
  {
    Debug.Log("count: " + CATCHABLE_FISH.Count);
    int index = Random.Range(0, CATCHABLE_FISH.Count);
    // Debug.Log("index: " + index);
    return CATCHABLE_FISH[index];
  }

  public List<Node> rootConversations;

  public Node GetConversation(Fishes fish)
  {
    switch (fish)
    {
      case Fishes.BOOT: return rootConversations[0];
      case Fishes.JIM: return rootConversations[1];
      case Fishes.KAT: return rootConversations[2];
      case Fishes.DAD: return rootConversations[3];
    }

    return null;
  }

  public enum Fishes
  {
    BOOT,
    JIM,
    KAT,
    DAD
  }
}
