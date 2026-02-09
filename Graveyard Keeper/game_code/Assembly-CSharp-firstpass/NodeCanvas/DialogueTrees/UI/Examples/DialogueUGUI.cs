// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.UI.Examples.DialogueUGUI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace NodeCanvas.DialogueTrees.UI.Examples;

public class DialogueUGUI : MonoBehaviour
{
  [Header("Input Options")]
  public bool skipOnInput;
  public bool waitForInput;
  [Header("Subtitles")]
  public RectTransform subtitlesGroup;
  public Text actorSpeech;
  public Text actorName;
  public Image actorPortrait;
  public RectTransform waitInputIndicator;
  public DialogueUGUI.SubtitleDelays subtitleDelays = new DialogueUGUI.SubtitleDelays();
  public List<AudioClip> typingSounds;
  [Header("Multiple Choice")]
  public RectTransform optionsGroup;
  public Button optionButton;
  public Dictionary<Button, int> cachedButtons;
  public Vector2 originalSubsPosition;
  public bool isWaitingChoice;
  public AudioSource _localSource;

  public AudioSource localSource
  {
    get
    {
      return !((UnityEngine.Object) this._localSource != (UnityEngine.Object) null) ? (this._localSource = this.gameObject.AddComponent<AudioSource>()) : this._localSource;
    }
  }

  public void OnEnable()
  {
    DialogueTree.OnDialogueStarted += new Action<DialogueTree>(this.OnDialogueStarted);
    DialogueTree.OnDialoguePaused += new Action<DialogueTree>(this.OnDialoguePaused);
    DialogueTree.OnDialogueFinished += new Action<DialogueTree>(this.OnDialogueFinished);
    DialogueTree.OnSubtitlesRequest += new Action<SubtitlesRequestInfo>(this.OnSubtitlesRequest);
    DialogueTree.OnMultipleChoiceRequest += new Action<MultipleChoiceRequestInfo>(this.OnMultipleChoiceRequest);
  }

  public void OnDisable()
  {
    DialogueTree.OnDialogueStarted -= new Action<DialogueTree>(this.OnDialogueStarted);
    DialogueTree.OnDialoguePaused -= new Action<DialogueTree>(this.OnDialoguePaused);
    DialogueTree.OnDialogueFinished -= new Action<DialogueTree>(this.OnDialogueFinished);
    DialogueTree.OnSubtitlesRequest -= new Action<SubtitlesRequestInfo>(this.OnSubtitlesRequest);
    DialogueTree.OnMultipleChoiceRequest -= new Action<MultipleChoiceRequestInfo>(this.OnMultipleChoiceRequest);
  }

  public void Start()
  {
    this.subtitlesGroup.gameObject.SetActive(false);
    this.optionsGroup.gameObject.SetActive(false);
    this.optionButton.gameObject.SetActive(false);
    this.waitInputIndicator.gameObject.SetActive(false);
    this.originalSubsPosition = (Vector2) this.subtitlesGroup.transform.position;
  }

  public void OnDialogueStarted(DialogueTree dlg)
  {
  }

  public void OnDialoguePaused(DialogueTree dlg)
  {
    this.subtitlesGroup.gameObject.SetActive(false);
    this.optionsGroup.gameObject.SetActive(false);
  }

  public void OnDialogueFinished(DialogueTree dlg)
  {
    this.subtitlesGroup.gameObject.SetActive(false);
    this.optionsGroup.gameObject.SetActive(false);
    if (this.cachedButtons == null)
      return;
    foreach (Button key in this.cachedButtons.Keys)
    {
      if ((UnityEngine.Object) key != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) key.gameObject);
    }
    this.cachedButtons = (Dictionary<Button, int>) null;
  }

  public void OnSubtitlesRequest(SubtitlesRequestInfo info)
  {
    this.StartCoroutine(this.Internal_OnSubtitlesRequestInfo(info));
  }

  public IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info)
  {
    DialogueUGUI dialogueUgui = this;
    string text = info.statement.text;
    AudioClip audio = info.statement.audio;
    IDialogueActor actor = info.actor;
    dialogueUgui.subtitlesGroup.gameObject.SetActive(true);
    dialogueUgui.actorSpeech.text = "";
    dialogueUgui.actorName.text = actor.name;
    dialogueUgui.actorSpeech.color = actor.dialogueColor;
    dialogueUgui.actorPortrait.gameObject.SetActive((UnityEngine.Object) actor.portraitSprite != (UnityEngine.Object) null);
    dialogueUgui.actorPortrait.sprite = actor.portraitSprite;
    if ((UnityEngine.Object) audio != (UnityEngine.Object) null)
    {
      AudioSource component = (UnityEngine.Object) actor.transform != (UnityEngine.Object) null ? actor.transform.GetComponent<AudioSource>() : (AudioSource) null;
      AudioSource playSource = (UnityEngine.Object) component != (UnityEngine.Object) null ? component : dialogueUgui.localSource;
      playSource.clip = audio;
      playSource.Play();
      dialogueUgui.actorSpeech.text = text;
      float timer = 0.0f;
      while ((double) timer < (double) audio.length)
      {
        if (dialogueUgui.skipOnInput && Input.anyKeyDown)
        {
          playSource.Stop();
          break;
        }
        timer += Time.deltaTime;
        yield return (object) null;
      }
      playSource = (AudioSource) null;
    }
    if ((UnityEngine.Object) audio == (UnityEngine.Object) null)
    {
      string tempText = "";
      bool inputDown = false;
      if (dialogueUgui.skipOnInput)
        dialogueUgui.StartCoroutine(dialogueUgui.CheckInput((Action) (() => inputDown = true)));
      for (int i = 0; i < text.Length; ++i)
      {
        if (dialogueUgui.skipOnInput & inputDown)
        {
          dialogueUgui.actorSpeech.text = text;
          yield return (object) null;
          break;
        }
        if (!dialogueUgui.subtitlesGroup.gameObject.activeSelf)
          yield break;
        char c = text[i];
        tempText += c.ToString();
        yield return (object) dialogueUgui.StartCoroutine(dialogueUgui.DelayPrint(dialogueUgui.subtitleDelays.characterDelay));
        dialogueUgui.PlayTypeSound();
        if (c == '.' || c == '!' || c == '?')
        {
          yield return (object) dialogueUgui.StartCoroutine(dialogueUgui.DelayPrint(dialogueUgui.subtitleDelays.sentenceDelay));
          dialogueUgui.PlayTypeSound();
        }
        if (c == ',')
        {
          yield return (object) dialogueUgui.StartCoroutine(dialogueUgui.DelayPrint(dialogueUgui.subtitleDelays.commaDelay));
          dialogueUgui.PlayTypeSound();
        }
        dialogueUgui.actorSpeech.text = tempText;
      }
      if (!dialogueUgui.waitForInput)
        yield return (object) dialogueUgui.StartCoroutine(dialogueUgui.DelayPrint(dialogueUgui.subtitleDelays.finalDelay));
      tempText = (string) null;
    }
    if (dialogueUgui.waitForInput)
    {
      dialogueUgui.waitInputIndicator.gameObject.SetActive(true);
      while (!Input.anyKeyDown)
        yield return (object) null;
      dialogueUgui.waitInputIndicator.gameObject.SetActive(false);
    }
    yield return (object) null;
    dialogueUgui.subtitlesGroup.gameObject.SetActive(false);
    info.Continue();
  }

  public void PlayTypeSound()
  {
    if (this.typingSounds.Count <= 0)
      return;
    AudioClip typingSound = this.typingSounds[UnityEngine.Random.Range(0, this.typingSounds.Count)];
    if (!((UnityEngine.Object) typingSound != (UnityEngine.Object) null))
      return;
    this.localSource.PlayOneShot(typingSound, UnityEngine.Random.Range(0.6f, 1f));
  }

  public IEnumerator CheckInput(Action Do)
  {
    while (!Input.anyKeyDown)
      yield return (object) null;
    Do();
  }

  public IEnumerator DelayPrint(float time)
  {
    float timer = 0.0f;
    while ((double) timer < (double) time)
    {
      timer += Time.deltaTime;
      yield return (object) null;
    }
  }

  public void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
  {
    this.optionsGroup.gameObject.SetActive(true);
    float height = this.optionButton.GetComponent<RectTransform>().rect.height;
    this.optionsGroup.sizeDelta = new Vector2(this.optionsGroup.sizeDelta.x, (float) ((double) info.options.Values.Count * (double) height + 20.0));
    this.cachedButtons = new Dictionary<Button, int>();
    int num = 0;
    foreach (KeyValuePair<IStatement, int> option in info.options)
    {
      Button btn = UnityEngine.Object.Instantiate<Button>(this.optionButton);
      btn.gameObject.SetActive(true);
      btn.transform.SetParent(this.optionsGroup.transform, false);
      btn.transform.localPosition = (Vector3) ((Vector2) this.optionButton.transform.localPosition - new Vector2(0.0f, height * (float) num));
      btn.GetComponentInChildren<Text>().text = option.Key.text;
      this.cachedButtons.Add(btn, option.Value);
      btn.onClick.AddListener((UnityAction) (() => this.Finalize(info, this.cachedButtons[btn])));
      ++num;
    }
    if (info.showLastStatement)
    {
      this.subtitlesGroup.gameObject.SetActive(true);
      this.subtitlesGroup.position = (Vector3) new Vector2(this.subtitlesGroup.position.x, (float) ((double) this.optionsGroup.position.y + (double) this.optionsGroup.sizeDelta.y + 1.0));
    }
    if ((double) info.availableTime <= 0.0)
      return;
    this.StartCoroutine(this.CountDown(info));
  }

  public IEnumerator CountDown(MultipleChoiceRequestInfo info)
  {
    this.isWaitingChoice = true;
    float timer = 0.0f;
    while ((double) timer < (double) info.availableTime)
    {
      if (!this.isWaitingChoice)
        yield break;
      timer += Time.deltaTime;
      this.SetMassAlpha(this.optionsGroup, Mathf.Lerp(1f, 0.0f, timer / info.availableTime));
      yield return (object) null;
    }
    if (this.isWaitingChoice)
      this.Finalize(info, info.options.Values.Last<int>());
  }

  public void Finalize(MultipleChoiceRequestInfo info, int index)
  {
    this.isWaitingChoice = false;
    this.SetMassAlpha(this.optionsGroup, 1f);
    this.optionsGroup.gameObject.SetActive(false);
    if (info.showLastStatement)
    {
      this.subtitlesGroup.gameObject.SetActive(false);
      this.subtitlesGroup.transform.position = (Vector3) this.originalSubsPosition;
    }
    foreach (Component key in this.cachedButtons.Keys)
      UnityEngine.Object.Destroy((UnityEngine.Object) key.gameObject);
    info.SelectOption(index);
  }

  public void SetMassAlpha(RectTransform root, float alpha)
  {
    foreach (CanvasRenderer componentsInChild in root.GetComponentsInChildren<CanvasRenderer>())
      componentsInChild.SetAlpha(alpha);
  }

  [Serializable]
  public class SubtitleDelays
  {
    public float characterDelay = 0.05f;
    public float sentenceDelay = 0.5f;
    public float commaDelay = 0.1f;
    public float finalDelay = 1.2f;
  }
}
