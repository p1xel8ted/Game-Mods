// Decompiled with JetBrains decompiler
// Type: Interaction_GofernonRotburn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_GofernonRotburn : Interaction
{
  [Header("Settings")]
  public bool Interaction_Active = true;
  public BaseGoopDoor BaseGoopDoor;
  [SerializeField]
  public SkeletonAnimation Spine;
  [Header("Covnersations and Barks")]
  [SerializeField]
  public Interaction_SimpleConversation introConvo;
  public int barkCount;
  [SerializeField]
  public SimpleBark bark1;
  [SerializeField]
  public SimpleBark bark2;
  [Header("Revealing + Hiding")]
  [SerializeField]
  public Vector3 revealOffset;
  public bool isRevealed;
  [SerializeField]
  public float revealDistance = 7f;
  public bool GetFreshData = true;
  public string InteractionLabel;
  public string SoldOutLabel;
  public bool activated;
  public int currentFuel;
  public TraderTrackerItems _gofernonRotburnTradeItem;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    int num = DataManager.Instance.Followers.Count >= this.BaseGoopDoor.woolhavenFollowerCountRequirement ? 1 : (this.BaseGoopDoor.woolhavenFollowerCountRequirement <= 0 ? 1 : 0);
    this.Spine.gameObject.SetActive(false);
    this.GetFreshData = true;
    this.bark1.gameObject.SetActive(false);
    this.bark2.gameObject.SetActive(false);
    this.introConvo.gameObject.SetActive(false);
    if (num != 0)
    {
      this.Interaction_Active = false;
    }
    else
    {
      this.Interaction_Active = true;
      this.introConvo.gameObject.SetActive(DataManager.Instance.GofernonRotburnProgress == 0);
      if (DataManager.Instance.GofernonRotburnProgress == 0)
        this.Interactable = false;
      this.introConvo.Callback.AddListener((UnityAction) (() =>
      {
        this.Interactable = true;
        if (++this.barkCount % 2 == 0)
          this.bark1.gameObject.SetActive(true);
        else
          this.bark1.gameObject.SetActive(false);
        this.bark2.gameObject.SetActive(!this.bark1.gameObject.activeSelf);
        this.introConvo.Callback.RemoveAllListeners();
      }));
      if (DataManager.Instance.GofernonRotburnProgress > 0)
      {
        if (++this.barkCount % 2 == 0)
          this.bark1.gameObject.SetActive(true);
        else
          this.bark1.gameObject.SetActive(false);
        this.bark2.gameObject.SetActive(!this.bark1.gameObject.activeSelf);
      }
    }
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.InteractionLabel = string.Format(LocalizationManager.GetTranslation("Interactions/Buy"), (object) "<sprite name=\"icon_MagmaStone\">");
    this.Label = LocalizationManager.GetTranslation("Interaction/SoldOut");
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.Interaction_Active && this.Interactable)
      this.Label = this.InteractionLabel;
    else
      this.Label = "";
  }

  public override void Update()
  {
    base.Update();
    if (!this.Interaction_Active)
      return;
    bool flag = false;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) Vector3.Distance(player.transform.position, this.transform.position + this.revealOffset) < (double) this.revealDistance || (double) player.transform.position.y > 30.0)
        flag = true;
    }
    if (flag && !this.isRevealed)
    {
      this.Spine.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_in", this.Spine.transform.position);
      this.Spine.AnimationState.SetAnimation(0, "dig_up", false);
      this.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      if (DataManager.Instance.GofernonRotburnProgress > 0)
      {
        if (++this.barkCount % 2 == 0)
          this.bark1.gameObject.SetActive(true);
        else
          this.bark1.gameObject.SetActive(false);
        this.bark2.gameObject.SetActive(!this.bark1.gameObject.activeSelf);
      }
      this.isRevealed = true;
    }
    else
    {
      if (flag || !(this.Spine.AnimationState.GetCurrent(0).Animation.Name != "dig_down"))
        return;
      if ((UnityEngine.Object) MMConversation.mmConversation != (UnityEngine.Object) null)
        MMConversation.mmConversation.Close(false);
      this.Spine.AnimationState.SetAnimation(0, "dig_down", false);
      this.isRevealed = false;
    }
  }

  public void OnFirstConversationComplete() => ++DataManager.Instance.GofernonRotburnProgress;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.activated)
      return;
    this.activated = true;
    this.StartCoroutine((IEnumerator) this.InteractRoutine());
  }

  public List<InventoryItem> GetItemsForSale()
  {
    bool flag = (UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_DLCFurnace.Instance.Structure != (UnityEngine.Object) null && Interaction_DLCFurnace.Instance.Structure.Brain != null;
    if (this.GetFreshData)
      this.currentFuel = flag ? Interaction_DLCFurnace.Instance.Structure.Brain.Data.Fuel : int.MaxValue;
    int num = ((flag ? Interaction_DLCFurnace.Instance.Structure.Brain.Data.MaxFuel : int.MaxValue) - this.currentFuel) / InventoryItem.FuelWeight(InventoryItem.ITEM_TYPE.MAGMA_STONE);
    int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE);
    for (int index = 0; index < PickUp.PickUps.Count; ++index)
    {
      if (PickUp.PickUps[index].type == InventoryItem.ITEM_TYPE.MAGMA_STONE)
        ++itemQuantity;
    }
    int Quantity = Mathf.Max(0, num - itemQuantity);
    List<InventoryItem> itemsForSale = new List<InventoryItem>();
    itemsForSale.Add(new InventoryItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, Quantity));
    this.GetFreshData = false;
    return itemsForSale;
  }

  public IEnumerator InteractRoutine()
  {
    Interaction_GofernonRotburn interactionGofernonRotburn = this;
    bool Waiting = true;
    interactionGofernonRotburn.playerFarming.GoToAndStop(interactionGofernonRotburn.transform.position + new Vector3(-0.5f, -2f), interactionGofernonRotburn.gameObject, GoToCallback: (System.Action) (() => Waiting = false));
    while (Waiting)
      yield return (object) null;
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(interactionGofernonRotburn.gameObject, 1f);
    HUD_Manager.Instance.Hide(false, 0);
    List<InventoryItem> itemsForSale = interactionGofernonRotburn.GetItemsForSale();
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(interactionGofernonRotburn.Spine.transform, interactionGofernonRotburn.playerFarming, itemsForSale, new ItemSelector.Params()
    {
      Key = "Gofernon_Rotburn",
      Context = ItemSelector.Context.Buy,
      Offset = new Vector2(0.0f, 200f),
      ShowEmpty = true,
      RequiresDiscovery = false,
      AllowInputOnlyFromPlayer = interactionGofernonRotburn.playerFarming
    });
    if (interactionGofernonRotburn.InputOnlyFromInteractingPlayer)
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = interactionGofernonRotburn.playerFarming;
    itemSelector.CostProvider = new Func<InventoryItem.ITEM_TYPE, TraderTrackerItems>(interactionGofernonRotburn.GetTradeItem);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      int sellPrice = this.GetTradeItem(chosenItem).SellPrice;
      if (itemSelector.GetItemQuantity(chosenItem) <= 0)
        AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback");
      else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) >= sellPrice)
      {
        Inventory.ChangeItemQuantity(20, -sellPrice);
        ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => InventoryItem.Spawn(chosenItem, 1, this.transform.position + new Vector3(0.0f, -0.3f))));
        itemSelector.SetItemQuantity(chosenItem, -1);
      }
      else
        AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback");
    });
    UIItemSelectorOverlayController overlayController1 = itemSelector;
    overlayController1.OnCancel = overlayController1.OnCancel + (System.Action) (() =>
    {
      HUD_Manager.Instance.Show(0);
      this.activated = false;
    });
    UIItemSelectorOverlayController overlayController2 = itemSelector;
    overlayController2.OnHidden = overlayController2.OnHidden + (System.Action) (() =>
    {
      cameraFollowTarget.SetOffset(Vector3.zero);
      cameraFollowTarget.RemoveTarget(this.gameObject);
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (player.GoToAndStopping)
          player.AbortGoTo();
      }
      PlayerFarming.SetStateForAllPlayers(LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle);
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
      this.activated = false;
    });
  }

  public TraderTrackerItems GetTradeItem(InventoryItem.ITEM_TYPE item)
  {
    if (this._gofernonRotburnTradeItem == null)
      this._gofernonRotburnTradeItem = new TraderTrackerItems()
      {
        itemForTrade = InventoryItem.ITEM_TYPE.MAGMA_STONE,
        SellPrice = 25,
        BuyPrice = 0,
        BuyOffsetPercent = 0,
        SellOffset = 0
      };
    return this._gofernonRotburnTradeItem;
  }

  [CompilerGenerated]
  public void \u003COnEnableInteraction\u003Eb__14_0()
  {
    this.Interactable = true;
    if (++this.barkCount % 2 == 0)
      this.bark1.gameObject.SetActive(true);
    else
      this.bark1.gameObject.SetActive(false);
    this.bark2.gameObject.SetActive(!this.bark1.gameObject.activeSelf);
    this.introConvo.Callback.RemoveAllListeners();
  }
}
