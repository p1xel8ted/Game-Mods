// Decompiled with JetBrains decompiler
// Type: Interaction_SeedShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_SeedShop : Interaction
{
  public SkeletonAnimation ShopKeeperSpine;
  private GameObject g;
  public TraderTracker TraderInfo;

  private void Start()
  {
    if (!DataManager.Instance.HasMetChefShop && DataManager.Instance.BossesCompleted.Count <= 0)
      this.gameObject.SetActive(false);
    this.TraderInfo.traderName = this.gameObject.name;
    this.AddShopItems();
    if (!(bool) (UnityEngine.Object) this.ShopKeeperSpine || this.ShopKeeperSpine.AnimationState == null)
      return;
    if (DataManager.Instance.ShopKeeperChefState == 1)
    {
      this.ShopKeeperSpine.AnimationState.SetAnimation(0, "animation-angry", true);
      foreach (TraderTrackerItems traderTrackerItems in this.TraderInfo.itemsToTrade)
        traderTrackerItems.SellOffset = traderTrackerItems.SellPrice;
    }
    else
    {
      if (DataManager.Instance.ShopKeeperChefState != 2)
        return;
      this.ShopKeeperSpine.AnimationState.SetAnimation(0, "scared", true);
    }
  }

  private void AddShopItems()
  {
    this.TraderInfo.blackList.Clear();
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2))
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_PUMPKIN);
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_3))
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER);
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_4))
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_BEETROOT);
    this.TraderInfo.GetItemsForSale();
  }

  private List<InventoryItem> GetItemsForSale()
  {
    List<InventoryItem> itemsForSale = new List<InventoryItem>();
    foreach (InventoryItem.ITEM_TYPE Type in this.TraderInfo.itemsForSale)
      itemsForSale.Add(new InventoryItem(Type, 999));
    return itemsForSale;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.AddShopItems();
    LocationManager.OnPlayerLocationSet += new System.Action(this.CheckIfShouldShow);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    LocationManager.OnPlayerLocationSet -= new System.Action(this.CheckIfShouldShow);
  }

  private void CheckIfShouldShow()
  {
  }

  public override void GetLabel()
  {
    this.Label = LocalizationManager.GetTranslation("Interactions/SeedShop");
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    List<InventoryItem> itemsForSale = this.GetItemsForSale();
    state.CURRENT_STATE = StateMachine.State.InActive;
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    HUD_Manager.Instance.Hide(false, 0);
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(itemsForSale, new ItemSelector.Params()
    {
      Key = "seed_shop",
      Context = ItemSelector.Context.Buy,
      Offset = new Vector2(0.0f, 150f),
      ShowEmpty = true,
      RequiresDiscovery = false,
      HideQuantity = true
    });
    itemSelector.CostProvider = new Func<InventoryItem.ITEM_TYPE, TraderTrackerItems>(this.GetTradeItem);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      TraderTrackerItems tradeItem = this.GetTradeItem(chosenItem);
      Inventory.ChangeItemQuantity(20, -tradeItem.SellPriceActual);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.gameObject);
      ResourceCustomTarget.Create(this.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => this.FinalizeTransaction(chosenItem, 1)));
      tradeItem.BuyOffset = Mathf.Min(tradeItem.BuyOffset + 2, 100);
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

  private void FinalizeTransaction(InventoryItem.ITEM_TYPE itemType, int amount)
  {
    InventoryItem.Spawn(itemType, amount, this.gameObject.transform.position);
  }

  private TraderTrackerItems GetTradeItem(InventoryItem.ITEM_TYPE item)
  {
    foreach (TraderTrackerItems tradeItem in this.TraderInfo.itemsToTrade)
    {
      if (tradeItem.itemForTrade == item)
        return tradeItem;
    }
    return (TraderTrackerItems) null;
  }
}
