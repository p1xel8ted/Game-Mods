// Decompiled with JetBrains decompiler
// Type: Interaction_FireHealingRoomNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_FireHealingRoomNPC : Interaction
{
  [Header("Conversations")]
  [SerializeField]
  public Interaction_SimpleConversation letter1Conversation;
  [SerializeField]
  public Interaction_SimpleConversation beforeBringingRodConversation;
  [SerializeField]
  public Interaction_SimpleConversation receiveRodConversation;
  [Header("Barks")]
  [SerializeField]
  public SimpleBarkRepeating normalBarks;
  [Space]
  [SerializeField]
  public Vector3 listenPosition;
  [SerializeField]
  public GameObject npc;
  [SerializeField]
  public SkeletonAnimation scyllaSpine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "scyllaSpine")]
  public string sadIdleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "scyllaSpine")]
  public string happyIdleAnimation;

  public bool canReceiveLetter
  {
    get
    {
      return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS) > 0 && !DataManager.Instance.DeliveredCharybisLetter;
    }
  }

  public bool canGiveFishingRod
  {
    get
    {
      return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD) > 0 && DataManager.Instance.DeliveredCharybisLetter;
    }
  }

  public bool canShowBeforeBringingRod
  {
    get
    {
      return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD) == 0 && DataManager.Instance.DeliveredCharybisLetter && !DataManager.Instance.BroughtFishingRod && !this.beforeBringingRodConversation.Spoken;
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.letter1Conversation.Callback.AddListener(new UnityAction(this.ReceiveLetterCallback));
    this.beforeBringingRodConversation.Callback.AddListener(new UnityAction(this.BeforeRodCallback));
    this.receiveRodConversation.Callback.AddListener(new UnityAction(this.ReceiveRodConversationCallback));
    if (DataManager.Instance.BroughtFishingRod)
      this.scyllaSpine.AnimationState.SetAnimation(0, this.happyIdleAnimation, true);
    else
      this.scyllaSpine.AnimationState.SetAnimation(0, this.sadIdleAnimation, true);
    this.CheckBarks();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.letter1Conversation.Callback.RemoveListener(new UnityAction(this.ReceiveLetterCallback));
    this.beforeBringingRodConversation.Callback.RemoveListener(new UnityAction(this.BeforeRodCallback));
    this.receiveRodConversation.Callback.RemoveListener(new UnityAction(this.ReceiveRodConversationCallback));
  }

  public override void GetLabel()
  {
    if (this.canReceiveLetter)
    {
      this.Label = $"{ScriptLocalization.Interactions.Give} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS)}";
      this.AutomaticallyInteract = true;
    }
    else if (this.canGiveFishingRod)
    {
      this.Label = $"{ScriptLocalization.Interactions.Give} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FISHING_ROD)}";
      this.AutomaticallyInteract = true;
    }
    else if (this.canShowBeforeBringingRod)
    {
      this.Label = ScriptLocalization.Interactions.Talk;
      this.AutomaticallyInteract = true;
    }
    else
    {
      this.Label = "";
      this.AutomaticallyInteract = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.canReceiveLetter)
      this.GoToInteractPosition((System.Action) (() => this.ReceiveLetter()));
    else if (this.canGiveFishingRod)
    {
      this.GoToInteractPosition((System.Action) (() => this.ReceiveFishingRod()));
    }
    else
    {
      if (!this.canShowBeforeBringingRod)
        return;
      this.beforeBringingRodConversation.gameObject.SetActive(true);
      this.beforeBringingRodConversation.Play(this.playerFarming.gameObject);
    }
  }

  public void ReceiveLetter()
  {
    GameManager.GetInstance().OnConversationNew();
    ResourceCustomTarget.Create(this.npc, this.playerFarming.transform.position, InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, (System.Action) (() =>
    {
      ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS);
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, -1);
      this.letter1Conversation.gameObject.SetActive(true);
      this.letter1Conversation.Play(this.playerFarming.gameObject);
    }));
  }

  public void ReceiveLetterCallback()
  {
    DataManager.Instance.DeliveredCharybisLetter = true;
    ObjectiveManager.Add((ObjectivesData) new Objectives_CollectItem("Objectives/GroupTitles/LegendaryDagger", InventoryItem.ITEM_TYPE.FISHING_ROD, 1), true, true);
    this.CheckBarks();
  }

  public void ReceiveFishingRod()
  {
    GameManager.GetInstance().OnConversationNew();
    ResourceCustomTarget.Create(this.npc, this.playerFarming.transform.position, InventoryItem.ITEM_TYPE.FISHING_ROD, (System.Action) (() =>
    {
      ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.FISHING_ROD);
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD, -1);
      this.receiveRodConversation.gameObject.SetActive(true);
      this.receiveRodConversation.Play(this.playerFarming.gameObject);
    }));
  }

  public void BeforeRodCallback() => this.CheckBarks();

  public void ReceiveRodConversationCallback()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      BiomeConstants.Instance.EmitHeartPickUpVFX(player.transform.position, 0.0f, "black", "burst_big");
      BiomeConstants.Instance.EmitBloodImpact(player.transform.position, 0.0f, "black", "BloodImpact_Large_0");
      player.health.FireHearts += (float) (4 * TrinketManager.GetHealthAmountMultiplier(this.playerFarming));
    }
    DataManager.Instance.BroughtFishingRod = true;
    this.PlayGiveDaggerSequence();
    this.CheckBarks();
  }

  public void GoToInteractPosition(System.Action callback)
  {
    this.playerFarming.GoToAndStop(this.npc.transform.position + this.listenPosition, this.gameObject, GoToCallback: (System.Action) (() => GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
          player.AbortGoTo();
      }
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      System.Action action = callback;
      if (action == null)
        return;
      action();
    })))), forcePositionOnTimeout: true, groupAction: true);
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void PlayGiveDaggerSequence()
  {
    this.StartCoroutine((IEnumerator) this.PlayerPickUpWeapon());
  }

  public IEnumerator PlayerPickUpWeapon()
  {
    Interaction_FireHealingRoomNPC fireHealingRoomNpc = this;
    while (fireHealingRoomNpc.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    PickUp pickup = (PickUp) null;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, fireHealingRoomNpc.npc.transform.position, result: (System.Action<PickUp>) (p =>
    {
      pickup = p;
      pickup.GetComponent<Interaction_BrokenWeapon>().SetWeapon(EquipmentType.Dagger_Legendary);
    }));
    while ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      yield return (object) null;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one;
    GameManager.GetInstance().OnConversationNext(pickup.gameObject, 5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", fireHealingRoomNpc.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    bool isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = pickup.transform.DOMove(BookTargetPosition, 1f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    pickup.transform.position = BookTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1.5f);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Dagger_Legendary);
    DataManager.Instance.FoundLegendaryDagger = true;
    pickup.GetComponent<Interaction_BrokenWeapon>().StartBringWeaponToBlacksmithObjective();
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
    fireHealingRoomNpc.enabled = false;
  }

  public void CheckBarks()
  {
    Debug.Log((object) $"Checking the Normal Bark condition: canReceiveLetter:{this.canReceiveLetter}, canGiveFishingRod: {this.canGiveFishingRod}, canShowBeforeBringingRod: {this.canShowBeforeBringingRod}");
    this.normalBarks?.gameObject.SetActive(!this.canReceiveLetter && !this.canGiveFishingRod && !this.canShowBeforeBringingRod);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__18_0() => this.ReceiveLetter();

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__18_1() => this.ReceiveFishingRod();

  [CompilerGenerated]
  public void \u003CReceiveLetter\u003Eb__19_0()
  {
    ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS, -1);
    this.letter1Conversation.gameObject.SetActive(true);
    this.letter1Conversation.Play(this.playerFarming.gameObject);
  }

  [CompilerGenerated]
  public void \u003CReceiveFishingRod\u003Eb__21_0()
  {
    ObjectiveManager.GiveItem(InventoryItem.ITEM_TYPE.FISHING_ROD);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.FISHING_ROD, -1);
    this.receiveRodConversation.gameObject.SetActive(true);
    this.receiveRodConversation.Play(this.playerFarming.gameObject);
  }
}
