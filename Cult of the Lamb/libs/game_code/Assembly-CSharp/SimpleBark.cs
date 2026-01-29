// Decompiled with JetBrains decompiler
// Type: SimpleBark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SimpleBark : BaseMonoBehaviour
{
  public static List<SimpleBark> Barks = new List<SimpleBark>();
  public float ActivateDistance = 4f;
  public Vector3 ActivateOffset = Vector3.zero;
  public bool Spoken;
  public bool DeleteIfConditionsMet = true;
  public List<SimpleBark.VariableAndCondition> DeleteConditions = new List<SimpleBark.VariableAndCondition>();
  public List<ConversationEntry> Entries;
  public bool DisableAfterBark;
  public bool HideAfterBark;
  public UnityEvent CallbackAfterBark;
  public bool useTimer;
  public float timer = 5f;
  public bool StopAudioAfter = true;
  [CompilerGenerated]
  public Renderer \u003CRenderer\u003Ek__BackingField;
  public int StartingIndex;
  public int RandomBark;
  public bool isSpeaking;
  public bool Translate = true;
  public bool played;
  public bool Closed;

  public Renderer Renderer
  {
    get => this.\u003CRenderer\u003Ek__BackingField;
    set => this.\u003CRenderer\u003Ek__BackingField = value;
  }

  public event SimpleBark.NormalEvent OnPlay;

  public event SimpleBark.NormalEvent OnClose;

  public void OnEnable() => SimpleBark.Barks.Add(this);

  public void Start()
  {
    this.RandomBark = UnityEngine.Random.Range(0, this.Entries.Count);
    if (this.DeleteConditions.Count <= 0)
      return;
    bool flag = true;
    foreach (SimpleBark.VariableAndCondition deleteCondition in this.DeleteConditions)
    {
      Debug.Log((object) $"{this.gameObject.name} {deleteCondition.Variable.ToString()} {deleteCondition.Condition.ToString()}  {DataManager.Instance.GetVariable(deleteCondition.Variable).ToString()}");
      if (DataManager.Instance.GetVariable(deleteCondition.Variable) != deleteCondition.Condition)
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    if (this.DeleteIfConditionsMet)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      this.enabled = false;
  }

  public void AddEntry()
  {
    if (this.Entries.Count <= 0)
      this.Entries.Add(new ConversationEntry((GameObject) null, ""));
    else
      this.Entries.Add(ConversationEntry.Clone(this.Entries[this.Entries.Count - 1]));
  }

  public void IncrementEntry()
  {
    if (this.Entries.Count <= 0)
      return;
    int startingIndex = this.StartingIndex;
    while (LocalizationManager.GetTermData(ConversationEntry.Clone(this.Entries[this.StartingIndex]).TermToSpeak.Replace(this.StartingIndex.ToString(), (++startingIndex).ToString())) != null)
    {
      ConversationEntry conversationEntry = ConversationEntry.Clone(this.Entries[this.StartingIndex]);
      conversationEntry.TermToSpeak = conversationEntry.TermToSpeak.Replace(this.StartingIndex.ToString(), startingIndex.ToString());
      this.Entries.Add(conversationEntry);
    }
  }

  public bool IsSpeaking => this.isSpeaking;

  public void Update()
  {
    if ((double) Time.timeScale == 0.0)
    {
      if (!this.isSpeaking)
        return;
      this.Close();
    }
    else
    {
      if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || this.DisableAfterBark && this.played && this.Closed)
        return;
      float closestPlayerDist = PlayerFarming.GetClosestPlayerDist(this.transform.position + this.ActivateOffset);
      if (!MMConversation.isPlaying && !LetterBox.IsPlaying && !this.isSpeaking && (double) closestPlayerDist < (double) this.ActivateDistance && UIItemSelectorOverlayController.SelectorOverlays.Count == 0)
        this.Show();
      if (this.isSpeaking && (UnityEngine.Object) this.Renderer != (UnityEngine.Object) null && !this.Renderer.isVisible)
        this.Close();
      if (this.isSpeaking && ((UnityEngine.Object) this.Renderer == (UnityEngine.Object) null && (double) closestPlayerDist > (double) this.ActivateDistance || LetterBox.IsPlaying))
      {
        Debug.Log((object) "CLOSE!");
        this.Close();
      }
      if (!this.isSpeaking || UIItemSelectorOverlayController.SelectorOverlays.Count <= 0)
        return;
      this.Close();
    }
  }

  public IEnumerator StartTimer()
  {
    yield return (object) new WaitForSeconds(this.timer);
    this.Close();
  }

  public void Show()
  {
    if (this.Entries.Count == 0)
    {
      this.Close();
    }
    else
    {
      Debug.Log((object) "START SPEAKING");
      this.isSpeaking = true;
      this.played = true;
      this.StopAllCoroutines();
      if (this.useTimer)
        this.StartCoroutine((IEnumerator) this.StartTimer());
      MMConversation.PlayBark(new ConversationObject(new List<ConversationEntry>()
      {
        ConversationEntry.Clone(this.Entries[this.RandomBark])
      }, (List<MMTools.Response>) null, (System.Action) null), this.Translate);
      if (++this.RandomBark >= this.Entries.Count)
        this.RandomBark = 0;
      SimpleBark.NormalEvent onPlay = this.OnPlay;
      if (onPlay == null)
        return;
      onPlay();
    }
  }

  public void Close()
  {
    this.Closed = true;
    Debug.Log((object) "CLOSE!!!");
    this.StopAllCoroutines();
    this.played = true;
    this.isSpeaking = false;
    if (this.Entries.Count > 0 && MMConversation.isBark && (UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
      MMConversation.mmConversation?.Close(stopAudio: this.StopAudioAfter);
    if (this.HideAfterBark)
    {
      this.gameObject.SetActive(false);
      this.played = false;
      this.isSpeaking = false;
      this.Closed = false;
    }
    SimpleBark.NormalEvent onClose = this.OnClose;
    if (onClose != null)
      onClose();
    this.CallbackAfterBark?.Invoke();
  }

  public static void CloseAllBarks(bool immediately)
  {
    for (int index = SimpleBark.Barks.Count - 1; index >= 0; --index)
    {
      if (immediately)
        SimpleBark.Barks[index].CloseImmediately();
      else
        SimpleBark.Barks[index].Close();
    }
  }

  public void CloseImmediately()
  {
    this.Closed = true;
    Debug.Log((object) "CLOSE!!!");
    this.StopAllCoroutines();
    this.played = true;
    this.isSpeaking = false;
    if (MMConversation.isBark && (UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
      MMConversation.mmConversation?.CloseImmediately(stopAudio: this.StopAudioAfter);
    if (this.HideAfterBark)
    {
      this.gameObject.SetActive(false);
      this.played = false;
      this.isSpeaking = false;
      this.Closed = false;
    }
    SimpleBark.NormalEvent onClose = this.OnClose;
    if (onClose == null)
      return;
    onClose();
  }

  public void OnDisable()
  {
    if (this.played)
      this.Close();
    SimpleBark.Barks.Remove(this);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.white);
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }

  public delegate void NormalEvent();
}
