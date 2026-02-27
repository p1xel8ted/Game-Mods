// Decompiled with JetBrains decompiler
// Type: SimpleBarkRepeating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleBarkRepeating : BaseMonoBehaviour
{
  private static List<SimpleBarkRepeating> Barks = new List<SimpleBarkRepeating>();
  public float ActivateDistance = 4f;
  public Vector3 ActivateOffset = Vector3.zero;
  private bool Spoken;
  public List<ConversationEntry> Entries;
  public int StartingIndex;
  private int RandomBark;
  public bool IsSpeaking;
  private GameObject Player;
  public bool Translate = true;
  private static SimpleBarkRepeating Closest;
  private float ClosestDist;
  private static int Frame = -1;
  private ConversationObject conversationObject;

  private void Start()
  {
    this.RandomBark = UnityEngine.Random.Range(0, this.Entries.Count);
    this.CreateConversationObject();
  }

  private void OnEnable() => SimpleBarkRepeating.Barks.Add(this);

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
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
      this.Player = PlayerFarming.Instance.gameObject;
    if (this.IsSpeaking && LetterBox.IsPlaying && (UnityEngine.Object) SimpleBarkRepeating.Closest == (UnityEngine.Object) this)
    {
      this.Close();
    }
    else
    {
      if (SimpleBarkRepeating.Frame != Time.frameCount)
      {
        SimpleBarkRepeating.Closest = (SimpleBarkRepeating) null;
        this.ClosestDist = float.MaxValue;
        foreach (SimpleBarkRepeating bark in SimpleBarkRepeating.Barks)
        {
          if (!((UnityEngine.Object) bark == (UnityEngine.Object) null))
          {
            float num = Vector3.Distance(this.Player.transform.position, bark.transform.position);
            if ((double) num < (double) this.ClosestDist && (double) num < (double) this.ActivateDistance)
            {
              this.ClosestDist = num;
              SimpleBarkRepeating.Closest = bark;
            }
          }
        }
        SimpleBarkRepeating.Frame = Time.frameCount;
      }
      if (!this.IsSpeaking && (UnityEngine.Object) SimpleBarkRepeating.Closest == (UnityEngine.Object) this && !MMConversation.isPlaying && !LetterBox.IsPlaying)
      {
        this.IsSpeaking = true;
        MMConversation.PlayBark(this.conversationObject, this.Translate);
      }
      if (!this.IsSpeaking || !((UnityEngine.Object) SimpleBarkRepeating.Closest != (UnityEngine.Object) this))
        return;
      this.Close();
    }
  }

  public void Close()
  {
    this.IsSpeaking = false;
    if (MMConversation.isBark)
      MMConversation.mmConversation?.Close();
    this.CreateConversationObject();
  }

  private void CreateConversationObject()
  {
    this.conversationObject = new ConversationObject(new List<ConversationEntry>()
    {
      ConversationEntry.Clone(this.Entries[this.RandomBark])
    }, (List<MMTools.Response>) null, (System.Action) null);
    this.RandomBark += UnityEngine.Random.Range(1, 3);
    if (this.RandomBark < this.Entries.Count)
      return;
    this.RandomBark = 0;
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) SimpleBarkRepeating.Closest == (UnityEngine.Object) this)
      SimpleBarkRepeating.Closest = (SimpleBarkRepeating) null;
    this.Close();
    SimpleBarkRepeating.Barks.Remove(this);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.white);
  }
}
