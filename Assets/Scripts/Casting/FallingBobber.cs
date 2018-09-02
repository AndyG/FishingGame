using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBobber : MonoBehaviour {

  private AudioSource bobberAudioSource;

  [SerializeField]
  private GameObject endBobberPrototype;
  private float targetPosY = 0f;

  void Start () {
    this.bobberAudioSource = GetComponent<AudioSource> ();
  }

  // Update is called once per frame
  void Update () {
    if (this.transform.position.y <= targetPosY) {
      GameObject go = GameObject.Instantiate (endBobberPrototype, this.transform.position, Quaternion.identity);
      go.transform.localScale = this.transform.localScale;
      GameObject.Destroy (gameObject);
      NotifyPlayerCasted ();
    }
  }

  public void SetTargetPosY (float targetPosY) {
    this.targetPosY = targetPosY;
  }

  private void NotifyPlayerCasted () {
    FindObjectOfType<PlayerController> ().NotifyCasted ();
  }
}