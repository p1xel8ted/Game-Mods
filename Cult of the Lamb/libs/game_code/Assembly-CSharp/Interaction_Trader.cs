// Decompiled with JetBrains decompiler
// Type: Interaction_Trader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Trader : Interaction
{
  public TraderTracker TraderInfo;
  public bool SellOnly;
  [SerializeField]
  public string _buyKey;
  [SerializeField]
  public string _sellKey;
  public string sSell;
  public string sBuy;
  public UnityEvent TraderOpen;
  public UnityEvent TraderClosed;
  public bool ShowCoinsQuantity = true;

  public void Start()
  {
    this.TraderInfo.traderName = this.gameObject.name;
    if (DataManager.Instance.ReturnTrader(this.TraderInfo.location) == null)
      DataManager.Instance.Traders.Add(this.TraderInfo);
    TraderTracker traderTracker = DataManager.Instance.ReturnTrader(this.TraderInfo.location);
    traderTracker.GetItemsForSale();
    this.TraderInfo.GetItemsForSale();
    if (this.TraderInfo.itemsForSale.Count == traderTracker.itemsForSale.Count)
      this.TraderInfo = traderTracker;
    else
      DataManager.Instance.SetTrader(this.TraderInfo);
    TimeManager.OnNewPhaseStarted += new System.Action(this.UpdatePrices);
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sSell = ScriptLocalization.Interactions.Sell;
    this.sBuy = ScriptLocalization.Interactions.Buy;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    TimeManager.OnNewPhaseStarted -= new System.Action(this.UpdatePrices);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    this.HasSecondaryInteraction = !this.SellOnly;
    this.SecondaryInteractable = this.HasSecondaryInteraction;
  }

  public void UpdatePrices()
  {
    for (int index = 0; index < this.TraderInfo.itemsToTrade.Count; ++index)
    {
      int num = TimeManager.CurrentDay - this.TraderInfo.itemsToTrade[index].LastDayChecked;
      this.TraderInfo.itemsToTrade[index].BuyOffset = Mathf.Clamp(this.TraderInfo.itemsToTrade[index].BuyOffset - num * 5, 0, 100);
      this.TraderInfo.itemsToTrade[index].LastDayChecked = TimeManager.CurrentDay;
    }
  }

  public override void GetLabel()
  {
    if (this.SellOnly)
      this.Label = this.sSell;
    else
      this.Label = string.Format(this.sBuy, (object) "");
  }

  public override void GetSecondaryLabel() => this.Label = this.sSell;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Interactable = false;
    this.SecondaryInteractable = false;
    if (this.SellOnly)
      this.DoSell();
    else
      this.DoBuy();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.DoSell();
  }

  public void DoSell()
  {
    if (Inventory.GetItemQuantities(this.TraderInfo.itemsForSale) == 0)
      return;
    this.TraderOpen?.Invoke();
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    HUD_Manager.Instance.Hide(false, 0);
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, this.TraderInfo.itemsForSale, new ItemSelector.Params()
    {
      Key = this._sellKey,
      Context = ItemSelector.Context.Sell,
      Offset = new Vector2(0.0f, 100f),
      ShowCoins = this.ShowCoinsQuantity
    });
    itemSelector.CostProvider = new Func<InventoryItem.ITEM_TYPE, TraderTrackerItems>(this.GetTradeItem);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      TraderTrackerItems tradeItem = this.GetTradeItem(chosenItem);
      int cost = tradeItem.BuyPriceActual;
      Inventory.ChangeItemQuantity((int) chosenItem, -1);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.playerFarming.transform.position);
      ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, chosenItem, (System.Action) (() => this.FinalizeTransaction(InventoryItem.ITEM_TYPE.BLACK_GOLD, cost)));
      tradeItem.BuyOffset = Mathf.Min(tradeItem.BuyOffset + 2, 100);
    });
    UIItemSelectorOverlayController overlayController1 = itemSelector;
    overlayController1.OnCancel = overlayController1.OnCancel + (System.Action) (() => HUD_Manager.Instance.Show(0));
    UIItemSelectorOverlayController overlayController2 = itemSelector;
    overlayController2.OnHidden = overlayController2.OnHidden + (System.Action) (() =>
    {
      this.TraderClosed?.Invoke();
      cameraFollowTarget.SetOffset(Vector3.zero);
      cameraFollowTarget.RemoveTarget(this.gameObject);
      this.state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.HasChanged = true;
      this.Interactable = true;
      this.SecondaryInteractable = this.HasSecondaryInteraction;
    });
  }

  public void DoBuy()
  {
    this.state.CURRENT_STATE = StateMachine.State.InActive;
    this.state.facingAngle = Utils.GetAngle(this.state.transform.position, this.transform.position);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    HUD_Manager.Instance.Hide(false, 0);
    List<InventoryItem> items = new List<InventoryItem>();
    foreach (InventoryItem.ITEM_TYPE Type in this.TraderInfo.itemsForSale)
      items.Add(new InventoryItem(Type, 999));
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, items, new ItemSelector.Params()
    {
      Key = this._buyKey,
      Context = ItemSelector.Context.Buy,
      Offset = new Vector2(0.0f, 100f),
      ShowEmpty = true,
      RequiresDiscovery = false,
      HideQuantity = true
    });
    itemSelector.CostProvider = new Func<InventoryItem.ITEM_TYPE, TraderTrackerItems>(this.GetTradeItem);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      TraderTrackerItems tradeItem = this.GetTradeItem(chosenItem);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.playerFarming.transform.position);
      Inventory.ChangeItemQuantity(20, -tradeItem.SellPriceActual);
      ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => this.FinalizeTransaction(chosenItem, 1)));
      tradeItem.BuyOffset = Mathf.Min(tradeItem.BuyOffset + 2, 100);
    });
    UIItemSelectorOverlayController overlayController1 = itemSelector;
    overlayController1.OnCancel = overlayController1.OnCancel + (System.Action) (() => HUD_Manager.Instance.Show(0));
    UIItemSelectorOverlayController overlayController2 = itemSelector;
    overlayController2.OnHidden = overlayController2.OnHidden + (System.Action) (() =>
    {
      cameraFollowTarget.SetOffset(Vector3.zero);
      cameraFollowTarget.RemoveTarget(this.gameObject);
      this.state.CURRENT_STATE = StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.HasChanged = true;
      this.Interactable = true;
      this.SecondaryInteractable = this.HasSecondaryInteraction;
    });
  }

  public TraderTrackerItems GetTradeItem(InventoryItem.ITEM_TYPE item)
  {
    foreach (TraderTrackerItems tradeItem in this.TraderInfo.itemsToTrade)
    {
      if (tradeItem.itemForTrade == item)
        return tradeItem;
    }
    return (TraderTrackerItems) null;
  }

  public void FinalizeTransaction(InventoryItem.ITEM_TYPE itemType, int cost)
  {
    InventoryItem.Spawn(itemType, cost, this.gameObject.transform.position);
  }
}
