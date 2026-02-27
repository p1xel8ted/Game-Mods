// Decompiled with JetBrains decompiler
// Type: SimpleBark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleBark : BaseMonoBehaviour
{
  public float ActivateDistance = 4f;
  public Vector3 ActivateOffset = Vector3.zero;
  private bool Spoken;
  public bool DeleteIfConditionsMet = true;
  public List<SimpleBark.VariableAndCondition> DeleteConditions = new List<SimpleBark.VariableAndCondition>();
  public List<ConversationEntry> Entries;
  public bool DisableAfterBark;
  public bool HideAfterBark;
  public bool useTimer;
  public float timer = 5f;
  public bool StopAudioAfter = true;
  public int StartingIndex;
  private int RandomBark;
  private bool IsSpeaking;
  public bool Translate = true;
  private bool played;
  private bool Closed;

  public Renderer Renderer { get; set; }

  public event SimpleBark.NormalEvent OnClose;

  private void Start()
  {
    this.RandomBark = UnityEngine.Random.Range(0, this.Entries.Count);
    if (this.DeleteConditions.Count <= 0)
      return;
    bool flag = true;
    foreach (SimpleBark.VariableAndCondition deleteCondition in this.DeleteConditions)
    {
      Debug.Log((object) $"{this.gameObject.name} {(object) deleteCondition.Variable} {deleteCondition.Condition.ToString()}  {DataManager.Instance.GetVariable(deleteCondition.Variable).ToString()}");
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

  private void IncrementEntry()
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

  private void Update()
  {
    if ((double) Time.timeScale == 0.0)
    {
      if (!this.IsSpeaking)
        return;
      this.Close();
    }
    else
    {
      if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || this.DisableAfterBark && this.played && this.Closed)
        return;
      if (!MMConversation.isPlaying && !LetterBox.IsPlaying && !this.IsSpeaking && (double) Vector3.Distance(this.transform.position + this.ActivateOffset, PlayerFarming.Instance.transform.position) < (double) this.ActivateDistance)
        this.Show();
      if (this.IsSpeaking && (UnityEngine.Object) this.Renderer != (UnityEngine.Object) null && !this.Renderer.isVisible)
        this.Close();
      if (!this.IsSpeaking || (!((UnityEngine.Object) this.Renderer == (UnityEngine.Object) null) || (double) Vector3.Distance(this.transform.position + this.ActivateOffset, PlayerFarming.Instance.transform.position) <= (double) this.ActivateDistance) && !LetterBox.IsPlaying)
        return;
      Debug.Log((object) "CLOSE!");
      this.Close();
    }
  }

  private IEnumerator StartTimer()
  {
    yield return (object) new WaitForSeconds(this.timer);
    this.Close();
  }

  public void Show()
  {
    Debug.Log((object) "START SPEAKING");
    this.IsSpeaking = true;
    this.played = true;
    if (this.useTimer)
      this.StartCoroutine((IEnumerator) this.StartTimer());
    MMConversation.PlayBark(new ConversationObject(new List<ConversationEntry>()
    {
      ConversationEntry.Clone(this.Entries[this.RandomBark])
    }, (List<MMTools.Response>) null, (System.Action) null), this.Translate);
    if (++this.RandomBark < this.Entries.Count)
      return;
    this.RandomBark = 0;
  }

  public void Close()
  {
    this.Closed = true;
    Debug.Log((object) "CLOSE!!!");
    this.StopAllCoroutines();
    this.played = true;
    this.IsSpeaking = false;
    if (MMConversation.isBark && (UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
      MMConversation.mmConversation?.Close(stopAudio: this.StopAudioAfter);
    if (this.HideAfterBark)
    {
      this.gameObject.SetActive(false);
      this.played = false;
      this.IsSpeaking = false;
      this.Closed = false;
    }
    SimpleBark.NormalEvent onClose = this.OnClose;
    if (onClose == null)
      return;
    onClose();
  }

  private void OnDisable()
  {
    if (!this.played)
      return;
    this.Close();
  }

  private void OnDrawGizmos()
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
