// Decompiled with JetBrains decompiler
// Type: SimpleBarkRepeating
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleBarkRepeating : BaseMonoBehaviour
{
  public static List<SimpleBarkRepeating> Barks = new List<SimpleBarkRepeating>();
  [Header("Activation")]
  public float ActivateDistance = 4f;
  public Vector3 ActivateOffset = Vector3.zero;
  [Header("Idle Play Threshold")]
  [Tooltip("If true, the bark only opens when the player has remained still for IdleSeconds while within range.")]
  public bool RequireIdleToShow;
  public float IdleSeconds = 1f;
  public bool Spoken;
  public List<ConversationEntry> Entries;
  public int StartingIndex;
  public int RandomBark;
  public bool IsSpeaking;
  public GameObject Player;
  public bool Translate = true;
  public static SimpleBarkRepeating Closest;
  public float ClosestDist;
  public static int Frame = -1;
  public Vector3 _lastPlayerPos;
  public float _lastMoveTime;
  public float _lastSpeed;
  public float idleTimer;
  public ConversationObject conversationObject;

  public void Start()
  {
    this.RandomBark = UnityEngine.Random.Range(0, this.Entries.Count);
    this.CreateConversationObject();
  }

  public void OnEnable()
  {
    SimpleBarkRepeating.Barks.Add(this);
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    GameObject playerGameObject = PlayerFarming.FindClosestPlayerGameObject(this.transform.position);
    if (!((UnityEngine.Object) playerGameObject != (UnityEngine.Object) null))
      return;
    this._lastPlayerPos = playerGameObject.transform.position;
    this._lastMoveTime = Time.time;
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

  public void Update()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    this.Player = PlayerFarming.FindClosestPlayerGameObject(this.transform.position);
    if ((UnityEngine.Object) this.Player == (UnityEngine.Object) null)
      return;
    if (PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.Idle)
      this.idleTimer += Time.deltaTime;
    else
      this.idleTimer = 0.0f;
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
            float num = Vector3.Distance(this.Player.transform.position, bark.transform.position + bark.ActivateOffset);
            if ((double) num < (double) bark.ActivateDistance && (double) num < (double) this.ClosestDist)
            {
              this.ClosestDist = num;
              SimpleBarkRepeating.Closest = bark;
            }
          }
        }
        SimpleBarkRepeating.Frame = Time.frameCount;
      }
      if (PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.Dodging)
      {
        if (!this.IsSpeaking)
          return;
        this.Close();
      }
      else
      {
        bool flag = (double) Time.time - (double) this._lastMoveTime >= (double) this.IdleSeconds || !this.RequireIdleToShow;
        if (((this.IsSpeaking ? 0 : ((UnityEngine.Object) SimpleBarkRepeating.Closest == (UnityEngine.Object) this ? 1 : 0)) & (flag ? 1 : 0)) != 0 && !MMConversation.isPlaying && !LetterBox.IsPlaying && UIItemSelectorOverlayController.SelectorOverlays.Count == 0 && (double) this.idleTimer > 0.10000000149011612)
        {
          this.IsSpeaking = true;
          MMConversation.PlayBark(this.conversationObject, this.Translate);
        }
        if (!this.IsSpeaking || !((UnityEngine.Object) SimpleBarkRepeating.Closest != (UnityEngine.Object) this) && UIItemSelectorOverlayController.SelectorOverlays.Count <= 0)
          return;
        this.Close();
      }
    }
  }

  public void Close()
  {
    this.IsSpeaking = false;
    if (MMConversation.isBark)
      MMConversation.mmConversation?.Close();
    this.CreateConversationObject();
  }

  public static void CloseAllBarks(bool immediately)
  {
    for (int index = SimpleBarkRepeating.Barks.Count - 1; index >= 0; --index)
    {
      if (!((UnityEngine.Object) SimpleBarkRepeating.Barks[index] == (UnityEngine.Object) null))
      {
        if (immediately)
          SimpleBarkRepeating.Barks[index].CloseImmediately();
        else
          SimpleBarkRepeating.Barks[index].Close();
      }
    }
  }

  public void CloseImmediately()
  {
    this.IsSpeaking = false;
    if (MMConversation.isBark)
      MMConversation.mmConversation?.CloseImmediately();
    this.CreateConversationObject();
  }

  public void CreateConversationObject()
  {
    if (this.RandomBark >= this.Entries.Count)
      this.RandomBark = 0;
    if (this.Entries.Count <= 0)
      this.conversationObject = new ConversationObject(new List<ConversationEntry>(), (List<MMTools.Response>) null, (System.Action) null);
    else
      this.conversationObject = new ConversationObject(new List<ConversationEntry>()
      {
        ConversationEntry.Clone(this.Entries[this.RandomBark])
      }, (List<MMTools.Response>) null, (System.Action) null);
    this.RandomBark += UnityEngine.Random.Range(1, 3);
    if (this.RandomBark < this.Entries.Count)
      return;
    this.RandomBark = 0;
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) SimpleBarkRepeating.Closest == (UnityEngine.Object) this)
      SimpleBarkRepeating.Closest = (SimpleBarkRepeating) null;
    this.Close();
    SimpleBarkRepeating.Barks.Remove(this);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivateOffset, this.ActivateDistance, Color.white);
  }
}
