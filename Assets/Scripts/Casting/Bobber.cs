using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour {

  [SerializeField]
  private GameObject indicatorPrototype;

  private GameObject spawnedIndicator;

  private AudioSource bobberAudioSource;

  [SerializeField]
  private float offsetX = 0.25f;
  [SerializeField]
  private float offsetY = 0.25f;

  [Header ("Audio")]
  [SerializeField]
  private AudioClip splashAudioClip;
  [SerializeField]
  private AudioClip hookedAudioClip;

  void Start () {
    BiteSystem biteSystem = FindObjectOfType<BiteSystem> ();
    biteSystem.OnBiteStartEvent += OnBiteStart;
    biteSystem.OnBiteEndEvent += OnBiteEnd;
    this.bobberAudioSource = GetComponent<AudioSource> ();

    this.bobberAudioSource.PlayOneShot (this.splashAudioClip);

  }

  public void DestroyIt () {
    BiteSystem biteSystem = FindObjectOfType<BiteSystem> ();
    biteSystem.OnBiteStartEvent -= OnBiteStart;
    biteSystem.OnBiteEndEvent -= OnBiteEnd;
    GameObject.Destroy (this.gameObject);
  }

  private void OnBiteStart () {
    Vector3 offset = new Vector3 (offsetX, offsetY, 0f);
    Vector3 position = this.transform.position + offset;
    spawnedIndicator = GameObject.Instantiate (indicatorPrototype, position, Quaternion.identity);
    spawnedIndicator.transform.localScale = this.transform.localScale;
    this.bobberAudioSource.PlayOneShot (this.hookedAudioClip);
  }

  private void OnBiteEnd (bool hooked) {
    GameObject.Destroy (spawnedIndicator);
  }
}