// Decompiled with JetBrains decompiler
// Type: Interaction_ShrinePleasure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using MMTools;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ShrinePleasure : Interaction
{
  public static Interaction_ShrinePleasure Instance;
  [SerializeField]
  public GameObject pointAvailableObject;
  [SerializeField]
  public GameObject rewardPrefab;
  public string sLabel;
  public bool GivenOutfit;

  public int PointsAvailable
  {
    get
    {
      int pointsAvailable = 0;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.CurrentTaskType == FollowerTaskType.Floating && allBrain.CurrentTask.State == FollowerTaskState.Idle)
          ++pointsAvailable;
      }
      return pointsAvailable;
    }
  }

  public void Start()
  {
    Interaction_ShrinePleasure.Instance = this;
    this.gameObject.SetActive(false);
    this.UpdateBar();
  }

  public override void OnDestroy() => base.OnDestroy();

  public void OnBrainAssigned() => this.UpdateBar();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Collect;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = this.PointsAvailable > 0;
    string str;
    if (!this.Interactable)
      str = ScriptLocalization.UI.AwaitingSin;
    else
      str = $"{this.sLabel} {ScriptLocalization.Inventory.PLEASURE_POINT} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.PLEASURE_POINT)}";
    this.Label = str;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.GiveRewardIE());
  }

  public void UpdateBar()
  {
    this.pointAvailableObject.gameObject.SetActive(this.PointsAvailable > 0);
  }

  public IEnumerator GiveRewardIE()
  {
    Interaction_ShrinePleasure interactionShrinePleasure = this;
    GameManager.GetInstance().OnConversationNew();
    interactionShrinePleasure.playerFarming.GoToAndStop(interactionShrinePleasure.transform.position + Vector3.down / 2f);
    List<FollowerBrain> followerBrainList = new List<FollowerBrain>();
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CurrentTaskType == FollowerTaskType.Floating)
        followerBrainList.Add(allBrain);
    }
    foreach (FollowerBrain follower in followerBrainList)
    {
      Follower f = FollowerManager.FindFollowerByID(follower.Info.ID);
      yield return (object) f.Spine.YieldForAnimation("sins");
      ++DataManager.Instance.pleasurePointsRedeemed;
      GameObject godTear = (GameObject) null;
      godTear = UnityEngine.Object.Instantiate<GameObject>(interactionShrinePleasure.rewardPrefab, f.Spine.transform.position, Quaternion.identity, interactionShrinePleasure.transform.parent);
      godTear.transform.localScale = Vector3.zero;
      godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      GameManager.GetInstance().OnConversationNext(godTear, 6f);
      AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up", interactionShrinePleasure.gameObject);
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", interactionShrinePleasure.gameObject);
      AudioManager.Instance.PlayOneShot("event:/player/float_follower", interactionShrinePleasure.gameObject);
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
      yield return (object) new WaitForSeconds(1.5f);
      PlayerSimpleInventory component = interactionShrinePleasure.state.gameObject.GetComponent<PlayerSimpleInventory>();
      godTear.transform.DOMove(new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      yield return (object) new WaitForSeconds(0.25f);
      follower.Info.Pleasure = 0;
      f.Spine.transform.DOKill();
      f.Spine.transform.DOLocalMoveZ(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(UnityEngine.Random.Range(0.0f, 1f)).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => f.Brain.CheckChangeState()));
      interactionShrinePleasure.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionShrinePleasure.transform.position);
      Inventory.AddItem(154, 1);
      yield return (object) new WaitForSeconds(1.25f);
      UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject);
      if (DataManager.Instance.pleasurePointsRedeemed == 1)
      {
        interactionShrinePleasure.playerFarming.GoToAndStop(f.transform.position + Vector3.right * 1.5f, f.gameObject);
        yield return (object) new WaitForSeconds(1f);
        List<ConversationEntry> Entries = new List<ConversationEntry>()
        {
          new ConversationEntry(interactionShrinePleasure.gameObject, "FollowerInteractions/FollowerFirstSin/0"),
          new ConversationEntry(interactionShrinePleasure.gameObject, "FollowerInteractions/FollowerFirstSin/1")
        };
        foreach (ConversationEntry conversationEntry in Entries)
          conversationEntry.CharacterName = follower.Info.Name;
        MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
        yield return (object) null;
        f.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) f.SetBodyAnimation("Conversations/talk-nice1", true);
        if (!ObjectiveManager.GroupExists("Objectives/GroupTitles/Pleasure") && DataManager.Instance.TempleLevel == -1)
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/Pleasure", Objectives.CustomQuestTypes.SpendSinInTemple), true);
        while (MMConversation.isPlaying)
          yield return (object) null;
      }
      f.Brain.CompleteCurrentTask();
      godTear = (GameObject) null;
    }
    GameManager.GetInstance().OnConversationEnd();
    if (DataManager.Instance.pleasurePointsRedeemed >= 5 && !interactionShrinePleasure.GivenOutfit && !DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_5))
      interactionShrinePleasure.StartCoroutine((IEnumerator) interactionShrinePleasure.GiveOutfitIE());
    NotificationCentreScreen.Play(ScriptLocalization.Notifications_Sin.SinAcquired);
    interactionShrinePleasure.UpdateBar();
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    int num = e.Data.Name == "PoopPop" ? 1 : 0;
  }

  public IEnumerator GiveOutfitIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_ShrinePleasure interactionShrinePleasure = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      GameManager.GetInstance().OnConversationEnd();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, interactionShrinePleasure.transform.position).GetComponent<FoundItemPickUp>();
    component.clothingType = FollowerClothingType.Special_5;
    interactionShrinePleasure.GivenOutfit = true;
    GameManager.GetInstance().OnConversationNext(component.gameObject, 6f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(1.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
