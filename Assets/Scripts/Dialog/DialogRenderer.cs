using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogRenderer : MonoBehaviour {

  private AudioSource dialogAudioSource;

  [SerializeField]
  private List<Node> nodes;

  [SerializeField]
  private TextMeshProUGUI nameText;

  [SerializeField]
  private TextMeshProUGUI contentText;

  [SerializeField]
  private Image image;

  [SerializeField]
  private GameObject dialogCanvas;

  [SerializeField]
  private GameObject choicesCanvas;

  [SerializeField]
  private TextMeshProUGUI choiceText1;

  [SerializeField]
  private TextMeshProUGUI choiceText2;

  [Range (0.01f, 0.2f)]
  [SerializeField]
  private float textDelay = 0.1f;

  private int nodeIndex = 0;

  private int nodeLineIndex = 0;

  private bool isDialogShowing = false;

  private IEnumerator slowTextCoroutine;

  [Header ("Audio")]
  [SerializeField]
  private List<AudioClip> audioClips;

  public delegate void OnFishSuccess (Fish.Fishes fish);
  public event OnFishSuccess OnFishSuccessEvent;

  public delegate void OnFishFailure (Fish.Fishes fish);
  public event OnFishFailure OnFishFailureEvent;

  public delegate void OnDialogEnded ();
  public event OnDialogEnded OnDialogEndedEvent;

  private void RenderNodes (List<Node> nodes) {
    this.nodes = nodes;
    nodeIndex = 0;
    RenderNode (nodes[nodeIndex]);
  }

  public void RenderNode (Node node) {
    nodeLineIndex = 0;
    dialogCanvas.SetActive (true);
    isDialogShowing = true;
    nameText.text = node.characterName;

    // Play test nice and slow..
    slowTextCoroutine = this.GetSlowText (node.text[0]);
    StartCoroutine (slowTextCoroutine);

    image.sprite = node.image;

    if (node.HasChoices ()) {
      choicesCanvas.SetActive (true);
      choiceText1.text = node.choices[0].text;
      choiceText2.text = node.choices[1].text;
    } else {
      choicesCanvas.SetActive (false);
    }
  }

  private IEnumerator GetSlowText (string s) {
    int strLen = s.Length;
    int index = 1;

    while (index <= strLen) {
      this.PlayRandomAudioClip ();
      string newStr = s.Substring (0, index);
      index++;

      this.contentText.text = newStr;
      yield return new WaitForSeconds (this.textDelay);
    }

    this.slowTextCoroutine = null;
  }

  public void Next () {
    Node currentNode = nodes[nodeIndex];
    if (currentNode.HasChoices ()) {
      return;
    }

    // if still populating slow text, fill immediately
    if (this.slowTextCoroutine != null) {
      StopCoroutine (this.slowTextCoroutine);
      this.slowTextCoroutine = null;
      this.contentText.text = currentNode.text[nodeLineIndex];
      return;
    }

    nodeLineIndex++;
    if (nodeLineIndex < currentNode.GetLineCount ()) {
      slowTextCoroutine = this.GetSlowText (currentNode.text[nodeLineIndex]);
      StartCoroutine (slowTextCoroutine);
    } else {
      NextNode ();
    }
  }

  private void PlayRandomAudioClip () {
    int clipIndex = Random.Range (0, this.audioClips.Count);
    this.dialogAudioSource.PlayOneShot (this.audioClips[clipIndex]);
  }

  private void NextNode () {
    Node currentNode = nodes[nodeIndex];

    nodeIndex++;
    nodeLineIndex = 0;
    if (nodeIndex < nodes.Count) {
      RenderNode (nodes[nodeIndex]);
    } else {
      EndConversation ();
      if (currentNode.GetSuccessFish ().HasValue) {
        _OnFishSuccess (currentNode.GetSuccessFish ().Value);
      } else if (currentNode.GetFailureFish ().HasValue) {
        _OnFishFailure (currentNode.GetFailureFish ().Value);
      } else if (currentNode.nextNode != null) {
        List<Node> newNodes = new List<Node> ();
        newNodes.Add (currentNode.nextNode);
        RenderNodes (newNodes);
      } {
        OnDialogEndedEvent ();
      }
    }
  }

  private void _OnFishSuccess (Fish.Fishes successFish) {
    OnFishSuccessEvent (successFish);
    Fish fishManager = FindObjectOfType<Fish> ();
    fishManager.OnFishCaught (successFish);
  }

  private void _OnFishFailure (Fish.Fishes failureFish) {
    OnFishFailureEvent (failureFish);
  }

  // Use this for initialization
  void Start () {
    dialogAudioSource = GetComponent<AudioSource> ();
  }

  // Update is called once per frame
  void Update () {
    if (Input.GetKeyDown (KeyCode.Space) && isDialogShowing) {
      Next ();
    }
  }

  public void OnChoice1 () {
    Choice choice = nodes[nodeIndex].choices[0];
    OnChoice (choice);
  }

  public void OnChoice2 () {
    Choice choice = nodes[nodeIndex].choices[1];
    OnChoice (choice);
  }

  private void OnChoice (Choice choice) {
    List<Node> newNodes = new List<Node> ();
    newNodes.Add (choice.result);
    RenderNodes (newNodes);
  }

  private void EndConversation () {
    isDialogShowing = false;
    dialogCanvas.SetActive (false);
  }

  public bool IsDialogShowing () {
    return isDialogShowing;
  }

  public void StartConversation (Node node) {
    List<Node> conversation = new List<Node> ();
    conversation.Add (node);
    RenderNodes (conversation);
  }
}