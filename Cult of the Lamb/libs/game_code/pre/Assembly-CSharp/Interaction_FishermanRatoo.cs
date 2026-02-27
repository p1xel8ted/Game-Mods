// Decompiled with JetBrains decompiler
// Type: Interaction_FishermanRatoo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections;
using System.Collections.Generic;
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
  public GameObject TraderInteraction;
  private int FishGotAwayCount;
  public Interaction_SimpleConversation CaughtFirstFishConversation;
  public Interaction_Fishing FishingSpot;
  private string sLabel;
  private Coroutine acceptFishRoutine;
  private bool Waiting;
  public Interaction_KeyPiece KeyPiecePrefab;

  private int Progress
  {
    get => DataManager.Instance.RatooFishingProgress;
    set => DataManager.Instance.RatooFishingProgress = value;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Give;
  }

  private void Start()
  {
    this.HasChanged = true;
    this.FishingSpot.OnCatchFish -= new System.Action(this.CaughtFish);
    this.FishingSpot.OnFishEscaped -= new System.Action(this.FishEscaped);
    this.StopAllCoroutines();
    if (this.GetRemainingFishCount() <= 0)
      this.Progress = 3;
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
        break;
      case 1:
        this.FishGotAwayCount = 0;
        this.FishingSpot.OnCatchFish += new System.Action(this.CaughtFish);
        this.FishingSpot.OnFishEscaped += new System.Action(this.FishEscaped);
        break;
      case 3:
        this.TraderInteraction.SetActive(true);
        break;
    }
    this.UpdateLocalisation();
  }

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

  private void CompleteObjective()
  {
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.CatchFish);
  }

  private void CaughtFish()
  {
    this.CaughtFirstFishConversation.CallOnConversationEnd = false;
    this.CaughtFirstFishConversation.gameObject.SetActive(true);
    this.CaughtFirstFishConversation.Callback.AddListener((UnityAction) (() =>
    {
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 6f);
      this.CompleteObjective();
      ++this.Progress;
      this.Start();
      this.StartCoroutine((IEnumerator) this.CaughtFishGiveTarot());
    }));
  }

  private IEnumerator CaughtFishGiveTarot()
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
      // ISSUE: reference to a compiler-generated method
      TarotCustomTarget.Create(interactionFishermanRatoo.transform.position + Vector3.back, PlayerFarming.Instance.transform.position, 1f, TarotCards.Card.NeptunesCurse, new System.Action(interactionFishermanRatoo.\u003CCaughtFishGiveTarot\u003Eb__21_0));
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

  private IEnumerator IGiveAllFishObjects()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchSquid));
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchOctopus));
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchLobster));
    ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/FishingQuest", Objectives.CustomQuestTypes.CatchCrab));
  }

  private void FishEscaped()
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

  private List<InventoryItem.ITEM_TYPE> GetRemainingFishList()
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

  private int GetRemainingFishCount()
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
    Debug.Log((object) ("Progress: " + (object) this.Progress));
    if (this.Progress != 2)
      return;
    base.OnInteract(state);
    this.Interactable = false;
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    HUD_Manager.Instance.Hide(false, 0);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    List<InventoryItem.ITEM_TYPE> remainingFishList = this.GetRemainingFishList();
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(remainingFishList, new ItemSelector.Params()
    {
      Key = "fisherman" + (object) remainingFishList.Count,
      Context = ItemSelector.Context.Give,
      Offset = new Vector2(0.0f, 100f),
      HideOnSelection = true,
      ShowEmpty = true
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
      state.CURRENT_STATE = StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
    });
  }

  private IEnumerator GiveItem(InventoryItem.ITEM_TYPE toGive)
  {
    Interaction_FishermanRatoo interactionFishermanRatoo = this;
    yield return (object) null;
    PlayerFarming.Instance.GoToAndStop(interactionFishermanRatoo.transform.position + new Vector3(-2.5f, 0.8f), interactionFishermanRatoo.gameObject);
    if (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 8f);
    GameManager.GetInstance().AddToCamera(interactionFishermanRatoo.gameObject);
    yield return (object) new WaitForSeconds(1f);
    interactionFishermanRatoo.Waiting = true;
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionFishermanRatoo.gameObject);
    // ISSUE: reference to a compiler-generated method
    ResourceCustomTarget.Create(interactionFishermanRatoo.gameObject, PlayerFarming.Instance.transform.position, toGive, new System.Action(interactionFishermanRatoo.\u003CGiveItem\u003Eb__28_0));
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
    Debug.Log((object) ("GetRemainingFishCount(): " + (object) interactionFishermanRatoo.GetRemainingFishCount()));
    if (interactionFishermanRatoo.GetRemainingFishCount() <= 0)
    {
      interactionFishermanRatoo.Progress++;
      interactionFishermanRatoo.QuestCompleteConversation.gameObject.SetActive(true);
      // ISSUE: reference to a compiler-generated method
      interactionFishermanRatoo.QuestCompleteConversation.Callback.AddListener(new UnityAction(interactionFishermanRatoo.\u003CGiveItem\u003Eb__28_1));
    }
    else
    {
      interactionFishermanRatoo.acceptFishRoutine = (Coroutine) null;
      interactionFishermanRatoo.Start();
    }
  }

  private IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_FishermanRatoo interactionFishermanRatoo = this;
    yield return (object) null;
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(interactionFishermanRatoo.KeyPiecePrefab, interactionFishermanRatoo.transform.position + Vector3.back * 0.75f, Quaternion.identity, interactionFishermanRatoo.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1f);
    KeyPiece.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) null;
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(PlayerFarming.Instance.state);
  }

  private void GetConversation(InventoryItem.ITEM_TYPE itemType)
  {
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    int num = -1;
    string str = $"Conversation_NPC/Fisherman/Caught_{itemType.ToString()}/";
    while (LocalizationManager.GetTermData(str + (object) ++num) != null)
      Entries.Add(new ConversationEntry(this.gameObject, str + num.ToString())
      {
        CharacterName = ScriptLocalization.NAMES.Fisherman,
        soundPath = "event:/dialogue/hallowed_shores/fisherman/standard_fisherman"
      });
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => this.Waiting = false)), false, SetPlayerIdleOnComplete: false, SnapLetterBox: true);
  }
}
