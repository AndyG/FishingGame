using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishHeadManager : MonoBehaviour
{

  private int index;
  public List<Image> hooks;
  public List<Sprite> sprites;

  // Use this for initialization
  void Start()
  {
    index = 0;
    DialogRenderer dialogRenderer = FindObjectOfType<DialogRenderer>();
    dialogRenderer.OnFishSuccessEvent += OnFishCaught;
  }

  void OnFishCaught(Fish.Fishes fish)
  {
    if (index < hooks.Count)
    {
      hooks[index].sprite = GetSpriteForFish(fish);
      index++;
    }
  }

  private Sprite GetSpriteForFish(Fish.Fishes fish)
  {
    switch (fish)
    {
      case Fish.Fishes.BOOT: return sprites[0];
      case Fish.Fishes.JIM: return sprites[1];
      case Fish.Fishes.KAT: return sprites[2];
      case Fish.Fishes.DAD: return sprites[3];
    }

    return null;
  }
}
