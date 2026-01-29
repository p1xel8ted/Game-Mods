// Decompiled with JetBrains decompiler
// Type: Interaction_FishermanRatoo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_FishermanRatoo : Interaction
{
  public Interaction_SimpleConversation IntroConversation;
  public Interaction_SimpleConversation FishGotAwayConversation;
  public Interaction_SimpleConversation FishGotAwayConversation2;
  public Interaction_SimpleConversation FishGotAwayConversation3;
  public Interaction_SimpleConversation QuestCompleteConversation;
  public Interaction_SimpleConversation WinterCantFishConversation;
  public SkeletonAnimation fishermanSpine;
  public GameObject TraderInteraction;
  public GameObject WinterInteraction;
  public int FishGotAwayCount;
  public Interaction_SimpleConversation CaughtFirstFishConversation;
  public Interaction_Fishing FishingSpot;
  public string sLabel;
  public Coroutine acceptFishRoutine;
  public bool Waiting;
  public Interaction_KeyPiece KeyPiecePrefab;

  public int Progress
  {
    get => DataManager.Instance.RatooFishingProgress;
    set => DataManager.Instance.RatooFishingProgress = value;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Give;
  }

  public void Start()
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && !DataManager.Instance.WinterModeActive)
    {
      this.fishermanSpine.AnimationState.SetAnimation(0, "idle-nofishingrod", true);
      if (!DataManager.Instance.FishermanWinterConvo)
        this.WinterCantFishConversation.gameObject.SetActive(true);
      this.WinterInteraction.SetActive(true);
    }
    else
    {
      this.HasChanged = true;
      this.FishingSpot.OnCatchFish -= new System.Action(this.CaughtFish);
      this.FishingSpot.OnFishEscaped -= new System.Action(this.FishEscaped);
      this.StopAllCoroutines();
      this.CheckIfQuest2IsCompleted();
      switch (this.Progress)
      {
        case 0:
          this.IntroConversation.gameObject.SetActive(true);
          this.IntroConversation.Callback.AddListener((UnityAction) (() =>
          {
            this.GiveObjective();
            ++this.Progress;
            this.Start();
          }));
          goto case 2;
        case 1:
          this.FishGotAwayCount = 0;
          this.FishingSpot.OnCatchFish += new System.Action(this.CaughtFish);
          this.FishingSpot.OnFishEscaped += new System.Action(this.FishEscaped);
          goto case 2;
        case 2:
          this.UpdateLocalisation();
          break;
        default:
          if (ObjectiveManager.GroupExists("Objectives/GroupTitles/LegendaryDagger"))
          {
            this.WinterInteraction.SetActive(true);
            goto case 2;
          }
          this.TraderInteraction.SetActive(true);
          goto case 2;
      }
    }
  }

  public void CheckIfQuest2IsCompleted()
  {
    if (this.GetRemainingFishCount() <= 0)
      this.Progress = 3;
    if (this.Progress != 3)
      return;
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchSquid) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchSquid))
    {
      DataManager.Instance.RatooFishing_FISH_SQUID = false;
      this.Progress = 2;
    }
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchOctopus) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchOctopus))
    {
      DataManager.Instance.RatooFishing_FISH_OCTOPUS = false;
      this.Progress = 2;
    }
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchLobster) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchLobster))
    {
      DataManager.Instance.RatooFishing_FISH_LOBSTER = false;
      this.Progress = 2;
    }
    if (ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchCrab) && !ObjectiveManager.HasCompletedCustomObjectiveOfType(Objectives.CustomQuestTypes.CatchCrab))
    {
      DataManager.Instance.RatooFishing_FISH_CRAB = false;
      this.Progress = 2;
    }
    if (this.Progress != 2)
      return;
    int remainingFishCount = this.GetRemainingFishCount();
    for (int index = 1; index <= remainingFishCount; ++index)
      DataManager.Instance.GetItemSelectorCategory("fisherman" + index.ToString()).TrackedItems.Clear();
  }

  public void IncreaseProgressWinter() => DataManager.Instance.FishermanWinterConvo = true;

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.acceptFishRoutine = (Coroutine) null;
  }

  public override void GetLabel()
  {
    if (this.Progress == 2)
      this.Label = this.sLabel;
    else
      this.Label = "";
  }

  public void GiveObjective()
  {
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/CatchFish", Objectives.CustomQuestTypes.CatchFish), true);
  }

  public void CompleteObjective()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CatchFish);
  }

  public void CaughtFish()
  {
    this.CaughtFirstFishConversation.CallOnConversationEnd = false;
    this.CaughtFirstFishConversation.gameObject.SetActive(true);
    this.CaughtFirstFishConversation.Callback.RemoveListener(new UnityAction(this.CaughtFirstFishConversationCallback));
    this.CaughtFirstFishConversation.Callback.AddListener(new UnityAction(this.CaughtFirstFishConversationCallback));
  }

  public void CaughtFirstFishConversationCallback()
  {
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject, 6f);
    this.CompleteObjective();
    ++this.Progress;
    this.Start();
    this.StartCoroutine((IEnumerator) this.CaughtFishGiveTarot());
  }

  public IEnumerator CaughtFishGiveTarot()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_FishermanRatoo interactionFishermanRatoo = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      TarotCustomTarget.Create(interactionFishermanRatoo.transform.position + Vector3.back, interactionFishermanRatoo.playerFarming.transform.position, 1f, TarotCards.Card.NeptunesCurse, new System.Action(interactionFishermanRatoo.\u003CCaughtFishGiveTarot\u003Eb__27_0));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator IGiveAllFishObjects()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchSquid));
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchOctopus));
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchLobster));
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchCrab));
  }

  public void FishEscaped()
  {
    Interaction_SimpleConversation simpleConversation;
    switch (++this.FishGotAwayCount)
    {
      case 1:
        simpleConversation = this.FishGotAwayConversation;
        break;
      case 2:
        simpleConversation = this.FishGotAwayConversation2;
        break;
      case 3:
        simpleConversation = this.FishGotAwayConversation3;
        break;
      default:
        this.FishingSpot.OnFishEscaped -= new System.Action(this.FishEscaped);
        return;
    }
    simpleConversation.gameObject.SetActive(true);
    simpleConversation.Callback.AddListener((UnityAction) (() => { }));
  }

  public List<InventoryItem.ITEM_TYPE> GetRemainingFishList()
  {
    List<InventoryItem.ITEM_TYPE> remainingFishList = new List<InventoryItem.ITEM_TYPE>();
    if (!DataManager.Instance.RatooFishing_FISH_CRAB)
      remainingFishList.Add(InventoryItem.ITEM_TYPE.FISH_CRAB);
    if (!DataManager.Instance.RatooFishing_FISH_LOBSTER)
      remainingFishList.Add(InventoryItem.ITEM_TYPE.FISH_LOBSTER);
    if (!DataManager.Instance.RatooFishing_FISH_OCTOPUS)
      remainingFishList.Add(InventoryItem.ITEM_TYPE.FISH_OCTOPUS);
    if (!DataManager.Instance.RatooFishing_FISH_SQUID)
      remainingFishList.Add(InventoryItem.ITEM_TYPE.FISH_SQUID);
    return remainingFishList;
  }

  public int GetRemainingFishCount()
  {
    int remainingFishCount = 0;
    if (!DataManager.Instance.RatooFishing_FISH_CRAB)
      ++remainingFishCount;
    if (!DataManager.Instance.RatooFishing_FISH_LOBSTER)
      ++remainingFishCount;
    if (!DataManager.Instance.RatooFishing_FISH_OCTOPUS)
      ++remainingFishCount;
    if (!DataManager.Instance.RatooFishing_FISH_SQUID)
      ++remainingFishCount;
    return remainingFishCount;
  }

  public override void OnInteract(StateMachine state)
  {
    Debug.Log((object) ("Progress: " + this.Progress.ToString()));
    if (this.Progress != 2)
      return;
    base.OnInteract(state);
    this.Interactable = false;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    foreach (SimpleBarkRepeating bark in SimpleBarkRepeating.Barks)
      bark.Close();
    HUD_Manager.Instance.Hide(false, 0);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    List<InventoryItem.ITEM_TYPE> remainingFishList = this.GetRemainingFishList();
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, remainingFishList, new ItemSelector.Params()
    {
      Key = "fisherman" + remainingFishList.Count.ToString(),
      Context = ItemSelector.Context.Give,
      Offset = new Vector2(0.0f, 100f),
      HideOnSelection = true,
      ShowEmpty = true,
      DisableForceClose = true
    });
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      this.StopAllCoroutines();
      this.StartCoroutine((IEnumerator) this.GiveItem(chosenItem));
    });
    UIItemSelectorOverlayController overlayController1 = itemSelector;
    overlayController1.OnCancel = overlayController1.OnCancel + (System.Action) (() => HUD_Manager.Instance.Show(0));
    UIItemSelectorOverlayController overlayController2 = itemSelector;
    overlayController2.OnHidden = overlayController2.OnHidden + (System.Action) (() =>
    {
      cameraFollowTarget.SetOffset(Vector3.zero);
      cameraFollowTarget.RemoveTarget(this.gameObject);
      state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
    });
  }

  public IEnumerator GiveItem(InventoryItem.ITEM_TYPE toGive)
  {
    Interaction_FishermanRatoo interactionFishermanRatoo = this;
    yield return (object) null;
    interactionFishermanRatoo.playerFarming.GoToAndStop(interactionFishermanRatoo.transform.position + new Vector3(-2.5f, 0.8f), interactionFishermanRatoo.gameObject);
    if (interactionFishermanRatoo.playerFarming.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionFishermanRatoo.playerFarming.gameObject, 8f);
    GameManager.GetInstance().AddToCamera(interactionFishermanRatoo.gameObject);
    yield return (object) new WaitForSeconds(1f);
    interactionFishermanRatoo.Waiting = true;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionFishermanRatoo.gameObject);
    ResourceCustomTarget.Create(interactionFishermanRatoo.gameObject, interactionFishermanRatoo.playerFarming.transform.position, toGive, new System.Action(interactionFishermanRatoo.\u003CGiveItem\u003Eb__34_0));
    while (interactionFishermanRatoo.Waiting)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", interactionFishermanRatoo.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionFishermanRatoo.Waiting = true;
    interactionFishermanRatoo.GetConversation(toGive);
    while (interactionFishermanRatoo.Waiting)
      yield return (object) null;
    yield return (object) interactionFishermanRatoo.StartCoroutine((IEnumerator) interactionFishermanRatoo.GiveKeyPieceRoutine());
    while ((UnityEngine.Object) Interaction_KeyPiece.Instance != (UnityEngine.Object) null)
      yield return (object) null;
    Inventory.ChangeItemQuantity((int) toGive, -1);
    switch (toGive)
    {
      case InventoryItem.ITEM_TYPE.FISH_CRAB:
        DataManager.Instance.RatooFishing_FISH_CRAB = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CatchCrab);
        break;
      case InventoryItem.ITEM_TYPE.FISH_LOBSTER:
        DataManager.Instance.RatooFishing_FISH_LOBSTER = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CatchLobster);
        break;
      case InventoryItem.ITEM_TYPE.FISH_OCTOPUS:
        DataManager.Instance.RatooFishing_FISH_OCTOPUS = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CatchOctopus);
        break;
      case InventoryItem.ITEM_TYPE.FISH_SQUID:
        DataManager.Instance.RatooFishing_FISH_SQUID = true;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CatchSquid);
        break;
    }
    Debug.Log((object) ("GetRemainingFishCount(): " + interactionFishermanRatoo.GetRemainingFishCount().ToString()));
    if (interactionFishermanRatoo.GetRemainingFishCount() <= 0)
    {
      interactionFishermanRatoo.Progress++;
      interactionFishermanRatoo.QuestCompleteConversation.gameObject.SetActive(true);
      interactionFishermanRatoo.QuestCompleteConversation.Callback.AddListener(new UnityAction(interactionFishermanRatoo.\u003CGiveItem\u003Eb__34_1));
    }
    else
    {
      interactionFishermanRatoo.acceptFishRoutine = (Coroutine) null;
      interactionFishermanRatoo.Start();
    }
  }

  public IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_FishermanRatoo interactionFishermanRatoo = this;
    yield return (object) null;
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(interactionFishermanRatoo.KeyPiecePrefab, interactionFishermanRatoo.transform.position + Vector3.back * 0.75f, Quaternion.identity, interactionFishermanRatoo.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1f);
    KeyPiece.transform.DOMove(interactionFishermanRatoo.playerFarming.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) null;
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(interactionFishermanRatoo.playerFarming.state);
  }

  public void GetConversation(InventoryItem.ITEM_TYPE itemType)
  {
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int num = -1;
    string str = $"Conversation_NPC/Fisherman/Caught_{itemType.ToString()}/";
    while (LocalizationManager.GetTermData(str + (++num).ToString()) != null)
      Entries.Add(new ConversationEntry(this.gameObject, str + num.ToString())
      {
        CharacterName = ScriptLocalization.NAMES.Fisherman,
        soundPath = "event:/dialogue/hallowed_shores/fisherman/standard_fisherman"
      });
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.Waiting = false)), false, SetPlayerIdleOnComplete: false, SnapLetterBox: true);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__18_0()
  {
    this.GiveObjective();
    ++this.Progress;
    this.Start();
  }

  [CompilerGenerated]
  public void \u003CCaughtFishGiveTarot\u003Eb__27_0()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.StartCoroutine((IEnumerator) this.IGiveAllFishObjects());
  }

  [CompilerGenerated]
  public void \u003CGiveItem\u003Eb__34_0() => this.Waiting = false;

  [CompilerGenerated]
  public void \u003CGiveItem\u003Eb__34_1() => this.Start();

  [CompilerGenerated]
  public void \u003CGetConversation\u003Eb__37_0() => this.Waiting = false;
}
