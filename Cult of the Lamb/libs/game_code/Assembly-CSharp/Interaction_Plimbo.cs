// Decompiled with JetBrains decompiler
// Type: Interaction_Plimbo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Plimbo : Interaction
{
  public float InteractionDistance = 2.5f;
  public GameObject Shrine;
  public Interaction_SimpleConversation IntroConversation;
  public Interaction_SimpleConversation EndConversation;
  public Interaction_SimpleConversation Layer2Conversation;
  public Interaction_SimpleConversation Layer2EndConversation;
  public Interaction_SimpleConversation WinterIntroConversation;
  public Interaction_SimpleConversation RotEyeConversation;
  public string sLabel;
  public bool ShowingResource;
  public bool Waiting;
  public Interaction_KeyPiece KeyPiecePrefab;

  public int StoryProgress
  {
    get => DataManager.Instance.PlimboStoryProgress;
    set => DataManager.Instance.PlimboStoryProgress = value;
  }

  public void Start()
  {
    this.ActivateDistance = this.InteractionDistance;
    this.HasChanged = true;
    this.Shrine.SetActive(false);
    TailorManager.GetClothingQuantity(FollowerClothingType.Normal_3);
    if (GameManager.Layer2 && this.StoryProgress == 5)
      this.StoryProgress = 6;
    this.WinterIntroConversation.Callback.AddListener(new UnityAction(this.UpdateConversations));
    this.Layer2Conversation.Callback.AddListener(new UnityAction(this.UpdateConversations));
    this.RotEyeConversation.Callback.AddListener(new UnityAction(this.UpdateConversations));
    this.UpdateConversations();
    if (this.StoryProgress < 5)
      this.Shrine.SetActive(false);
    if (Inventory.GetItemQuantity(101) <= 0 && ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.PlimboEye4) && DataManager.Instance.HasBeatenAllBeholderBosses())
      Inventory.AddItem(101, 1);
    this.UpdateLocalisation();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.WinterIntroConversation.Callback.RemoveListener(new UnityAction(this.UpdateConversations));
    this.Layer2Conversation.Callback.RemoveListener(new UnityAction(this.UpdateConversations));
    this.RotEyeConversation.Callback.RemoveListener(new UnityAction(this.UpdateConversations));
  }

  public string GetAffordColor()
  {
    return Inventory.GetItemQuantity(101) > 0 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = string.Join(" ", string.Format(ScriptLocalization.UI_ItemSelector_Context.Give, (object) ScriptLocalization.Inventory.BEHOLDER_EYE), CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BEHOLDER_EYE, 1));
  }

  public override void GetLabel()
  {
    if (this.StoryProgress > 0 && this.StoryProgress < 5 || this.StoryProgress > 6 && this.StoryProgress < 11)
      this.Label = this.sLabel;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (Inventory.GetItemQuantity(101) <= 0)
    {
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      base.OnInteract(state);
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.GiveBeholderEye());
    }
  }

  public IEnumerator AcceptBeholderEye()
  {
    Interaction_Plimbo interactionPlimbo = this;
    while ((UnityEngine.Object) interactionPlimbo.playerFarming == (UnityEngine.Object) null)
      yield return (object) null;
    while (InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
  }

  public IEnumerator GiveBeholderEye()
  {
    Interaction_Plimbo interactionPlimbo = this;
    interactionPlimbo.state.CURRENT_STATE = StateMachine.State.InActive;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlimboEye1);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlimboEye2);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlimboEye3);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlimboEye4);
    interactionPlimbo.playerFarming.GoToAndStop(interactionPlimbo.transform.position + new Vector3(-0.5f, -2f), interactionPlimbo.gameObject);
    if (interactionPlimbo.playerFarming.GoToAndStopping)
      yield return (object) null;
    interactionPlimbo.ShowingResource = false;
    Inventory.ChangeItemQuantity(101, -1);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPlimbo.playerFarming.gameObject, 8f);
    GameManager.GetInstance().AddToCamera(interactionPlimbo.gameObject);
    yield return (object) new WaitForSeconds(1f);
    interactionPlimbo.Waiting = true;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionPlimbo.gameObject);
    ResourceCustomTarget.Create(interactionPlimbo.gameObject, interactionPlimbo.playerFarming.transform.position, InventoryItem.ITEM_TYPE.BEHOLDER_EYE, new System.Action(interactionPlimbo.\u003CGiveBeholderEye\u003Eb__21_0));
    while (interactionPlimbo.Waiting)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionPlimbo.Waiting = true;
    interactionPlimbo.GetConversation();
    while (interactionPlimbo.Waiting)
      yield return (object) null;
    yield return (object) interactionPlimbo.StartCoroutine((IEnumerator) interactionPlimbo.GiveKeyPieceRoutine());
    while ((UnityEngine.Object) Interaction_KeyPiece.Instance != (UnityEngine.Object) null)
      yield return (object) null;
    ++interactionPlimbo.StoryProgress;
    if (interactionPlimbo.StoryProgress == 5)
      interactionPlimbo.EndConversation.gameObject.SetActive(true);
    else if (interactionPlimbo.StoryProgress == 11)
      interactionPlimbo.Layer2EndConversation.gameObject.SetActive(true);
    else
      interactionPlimbo.Start();
    yield return (object) new WaitForSeconds(2f);
    switch (interactionPlimbo.StoryProgress)
    {
      case 2:
      case 8:
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye2));
        break;
      case 3:
      case 9:
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye3));
        break;
      case 4:
      case 10:
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye4));
        break;
    }
  }

  public void GiveLoreNote() => this.StartCoroutine((IEnumerator) this.GiveLoreNoteIE());

  public IEnumerator GiveLoreNoteIE()
  {
    Interaction_Plimbo interactionPlimbo = this;
    bool waiting = true;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionPlimbo.gameObject);
    ResourceCustomTarget.Create(interactionPlimbo.gameObject, interactionPlimbo.playerFarming.transform.position, InventoryItem.ITEM_TYPE.LORE_STONE, (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
  }

  public void TestComplete()
  {
    this.StoryProgress = 5;
    this.EndConversation.gameObject.SetActive(true);
  }

  public void GetConversation()
  {
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int num = -1;
    string str = $"Conversation_NPC/DecorationShop/BeholderEye{this.StoryProgress.ToString()}/";
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
    {
      ConversationEntry conversationEntry = new ConversationEntry(this.gameObject, str + num.ToString());
      conversationEntry.CharacterName = ScriptLocalization.NAMES.DecorationShopSeller;
      conversationEntry.soundPath = "event:/dialogue/plimbo/standard_plimbo";
      conversationEntry.Animation = "talk";
      if (this.StoryProgress == 1)
      {
        switch (num)
        {
          case 0:
            conversationEntry.Animation = "talk-wink";
            break;
          case 1:
            conversationEntry.Animation = "talk";
            break;
          case 2:
            conversationEntry.Animation = "talk-laugh";
            break;
          default:
            conversationEntry.Animation = "talk";
            break;
        }
      }
      else if (this.StoryProgress == 2)
      {
        switch (num)
        {
          case 0:
            conversationEntry.Animation = "talk-excited";
            break;
          case 1:
            conversationEntry.Animation = "talk";
            break;
          case 2:
            conversationEntry.Animation = "talk";
            break;
          default:
            conversationEntry.Animation = "talk-wink";
            break;
        }
      }
      else if (this.StoryProgress == 3)
        conversationEntry.Animation = num != 0 ? "talk" : "talk-excited";
      else if (this.StoryProgress == 4)
        conversationEntry.Animation = num != 0 ? "talk-wink" : "talk-excited";
      Entries.Add(conversationEntry);
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.Waiting = false)), false, SetPlayerIdleOnComplete: false);
  }

  public IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_Plimbo interactionPlimbo = this;
    yield return (object) null;
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(interactionPlimbo.KeyPiecePrefab, interactionPlimbo.transform.position + Vector3.back * 1.2f, Quaternion.identity, interactionPlimbo.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1f);
    KeyPiece.transform.DOMove(interactionPlimbo.playerFarming.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) null;
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(interactionPlimbo.playerFarming.state);
  }

  public void RevealShrine() => this.StartCoroutine((IEnumerator) this.RevealShrineIE());

  public IEnumerator RevealShrineIE()
  {
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.Shrine.gameObject);
    this.Shrine.gameObject.SetActive(true);
    Interaction_RatauShrine r = this.Shrine.GetComponentInChildren<Interaction_RatauShrine>();
    this.Shrine.transform.localPosition = Vector3.forward;
    this.Shrine.transform.DOLocalMove(Vector3.zero, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      Vector3 localScale = r.transform.localScale;
      r.transform.localScale = Vector3.zero;
      r.transform.DOScale(localScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      CultFaithManager.AddThought(Thought.Cult_PledgedToYou);
    }));
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void UpdateConversations()
  {
    if (DataManager.Instance.CurrentSeason == SeasonsManager.Season.Winter && !DataManager.Instance.SpokenToPlimboWinter)
      this.WinterIntroConversation.gameObject.SetActive(true);
    else if (this.StoryProgress != 6 && !DataManager.Instance.PlimboRejectedRotEye && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT) > 0)
    {
      this.RotEyeConversation.gameObject.SetActive(true);
    }
    else
    {
      switch (this.StoryProgress)
      {
        case 0:
          this.IntroConversation.gameObject.SetActive(true);
          this.IntroConversation.Callback.AddListener((UnityAction) (() =>
          {
            this.StoryProgress = 1;
            ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye1));
            this.Start();
          }));
          break;
        case 5:
          this.Shrine.SetActive(true);
          break;
        case 6:
          this.Layer2Conversation.gameObject.SetActive(true);
          this.Layer2Conversation.Callback.AddListener((UnityAction) (() =>
          {
            this.StoryProgress = 7;
            ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye1));
            this.Start();
          }));
          break;
        default:
          this.StartCoroutine((IEnumerator) this.AcceptBeholderEye());
          break;
      }
    }
  }

  [CompilerGenerated]
  public void \u003CGiveBeholderEye\u003Eb__21_0() => this.Waiting = false;

  [CompilerGenerated]
  public void \u003CGetConversation\u003Eb__25_0() => this.Waiting = false;

  [CompilerGenerated]
  public void \u003CUpdateConversations\u003Eb__30_0()
  {
    this.StoryProgress = 1;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye1));
    this.Start();
  }

  [CompilerGenerated]
  public void \u003CUpdateConversations\u003Eb__30_1()
  {
    this.StoryProgress = 7;
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/VisitPlimbo", Objectives.CustomQuestTypes.PlimboEye1));
    this.Start();
  }
}
