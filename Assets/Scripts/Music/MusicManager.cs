using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

  [SerializeField]
  AudioSource musicSource;

  [SerializeField]
  AudioClip calmMusic;

  [SerializeField]
  AudioClip excitedMusic;

  int music = -1;

  int CALM = 1;
  int EXCITED = 2;


  // Use this for initialization
  void Start()
  {
    PlayCalm();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void PlayCalm()
  {
    if (music != CALM)
    {
      musicSource.clip = calmMusic;
      musicSource.Play();
      musicSource.volume = 0.25f;
      music = CALM;
    }
  }

  public void PlayExcited()
  {
    if (music != EXCITED)
    {
      musicSource.clip = excitedMusic;
      musicSource.volume = 0.4f;
      musicSource.Play();
      music = EXCITED;
    }
  }

  public void StopMusic()
  {
    musicSource.Stop();
  }
}
