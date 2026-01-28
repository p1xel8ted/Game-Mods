// Decompiled with JetBrains decompiler
// Type: Interaction_SeedShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.UINavigator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_SeedShop : Interaction
{
  [SerializeField]
  public GameObject postGameConvo;
  public SkeletonAnimation ShopKeeperSpine;
  public GameObject g;
  public TraderTracker TraderInfo;

  public void Start()
  {
    if (!DataManager.Instance.HasMetChefShop && DataManager.Instance.BossesCompleted.Count <= 0)
      this.gameObject.SetActive(false);
    this.TraderInfo.traderName = this.gameObject.name;
    this.AddShopItems();
    this.ApplyAnimationState();
    if (DataManager.Instance.ShopKeeperChefState == 1)
    {
      foreach (TraderTrackerItems traderTrackerItems in this.TraderInfo.itemsToTrade)
        traderTrackerItems.SellOffset = traderTrackerItems.SellPrice;
    }
    if (DataManager.Instance.DeathCatBeaten)
    {
      foreach (TraderTrackerItems traderTrackerItems in this.TraderInfo.itemsToTrade)
        traderTrackerItems.SellPrice *= 2;
    }
    else
    {
      if (!((UnityEngine.Object) this.postGameConvo != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.postGameConvo);
    }
  }

  public void AddShopItems()
  {
    this.TraderInfo.blackList.Clear();
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_2) && !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_2) && !DataManager.Instance.DeathCatBeaten)
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_PUMPKIN);
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_3) && !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_3) && !DataManager.Instance.DeathCatBeaten)
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_CAULIFLOWER);
    if (!DataManager.Instance.UnlockedDungeonDoor.Contains(FollowerLocation.Dungeon1_4) && !DataManager.Instance.DungeonCompleted(FollowerLocation.Dungeon1_4) && !DataManager.Instance.DeathCatBeaten)
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_BEETROOT);
    if (!DataManager.Instance.PleasureEnabled)
    {
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_HOPS);
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_GRAPES);
    }
    if (!DataManager.Instance.TailorEnabled && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_MatingTent))
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_COTTON);
    if (!SeasonsManager.Active)
      this.TraderInfo.blackList.Add(InventoryItem.ITEM_TYPE.SEED_CHILLI);
    this.TraderInfo.GetItemsForSale();
  }

  public List<InventoryItem> GetItemsForSale()
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

  public void CheckIfShouldShow()
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
    state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
    PlayerFarming playerFarming = state.GetComponent<PlayerFarming>();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    playerFarming.GoToAndStop(playerFarming.transform.position, playerFarming.LookToObject, forcePositionOnTimeout: true, groupAction: true, forceAstar: true);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    HUD_Manager.Instance.Hide(false, 0);
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(playerFarming, itemsForSale, new ItemSelector.Params()
    {
      Key = "seed_shop",
      Context = ItemSelector.Context.Buy,
      Offset = new Vector2(0.0f, 150f),
      ShowEmpty = true,
      RequiresDiscovery = false,
      HideQuantity = true,
      AllowInputOnlyFromPlayer = playerFarming
    });
    if (this.InputOnlyFromInteractingPlayer)
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
    itemSelector.CostProvider = new Func<InventoryItem.ITEM_TYPE, TraderTrackerItems>(this.GetTradeItem);
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      TraderTrackerItems tradeItem = this.GetTradeItem(chosenItem);
      Inventory.ChangeItemQuantity(20, -tradeItem.SellPriceActual);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.gameObject);
      ResourceCustomTarget.Create(this.gameObject, playerFarming.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) (() => this.FinalizeTransaction(chosenItem, 1)));
      tradeItem.BuyOffset = Mathf.Min(tradeItem.BuyOffset + 2, 100);
    });
    UIItemSelectorOverlayController overlayController1 = itemSelector;
    overlayController1.OnCancel = overlayController1.OnCancel + (System.Action) (() => HUD_Manager.Instance.Show(0));
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
    });
  }

  public void ApplyAnimationState()
  {
    if (!(bool) (UnityEngine.Object) this.ShopKeeperSpine || this.ShopKeeperSpine.AnimationState == null)
      return;
    if (DataManager.Instance.ShopKeeperChefState == 1)
    {
      this.ShopKeeperSpine.AnimationState.SetAnimation(0, "animation-angry", true);
    }
    else
    {
      if (DataManager.Instance.ShopKeeperChefState != 2)
        return;
      this.ShopKeeperSpine.AnimationState.SetAnimation(0, "scared", true);
    }
  }

  public void FinalizeTransaction(InventoryItem.ITEM_TYPE itemType, int amount)
  {
    InventoryItem.Spawn(itemType, amount, this.gameObject.transform.position);
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
}
