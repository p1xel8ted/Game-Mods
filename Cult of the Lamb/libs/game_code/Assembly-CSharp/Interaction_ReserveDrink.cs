// Decompiled with JetBrains decompiler
// Type: Interaction_ReserveDrink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_ReserveDrink : Interaction
{
  public Interaction_Pub pub;
  public int seat;
  public FollowerBrain targetFollower;

  public InventoryItem drink => this.pub.Brain.FoodStorage.Data.Inventory[this.seat];

  public string FollowerName
  {
    get => FollowerInfo.GetInfoByID(this.pub.Brain.GetDrinkReservedFollower(this.seat)).Name;
  }

  public string DrinkName
  {
    get => CookingData.GetLocalizedName((InventoryItem.ITEM_TYPE) this.drink.type);
  }

  public void Configure(Interaction_Pub pub, int seat)
  {
    this.pub = pub;
    this.seat = seat;
    if (pub.Brain.GetDrinkReservedFollower(seat) == -1)
      return;
    this.targetFollower = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(pub.Brain.GetDrinkReservedFollower(seat)));
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = !Structures_Pub.IsDrinking && this.pub.Brain.FoodStorage.Data.Inventory.Count > this.seat && this.pub.Brain.FoodStorage.Data.Inventory[this.seat] != null;
    if (!this.Interactable)
      this.Label = "";
    else if (this.pub.Brain.IsDrinkReserved(this.seat))
      this.Label = LocalizationManager.GetTranslation("UI/UnreserveDrink");
    else
      this.Label = string.Format(LocalizationManager.GetTranslation("UI/ReserveDrink"), (object) this.DrinkName);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.pub.Brain.IsDrinkReserved(this.seat))
    {
      this.UnreserveDrink();
    }
    else
    {
      GameManager.GetInstance().OnConversationNew();
      Time.timeScale = 0.0f;
      List<ObjectivesData> drinkObjectives = ObjectiveManager.GetCustomObjectives(Objectives.TYPES.DRINK);
      UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
      followerSelectMenu.VotingType = TwitchVoting.VotingType.DRINK;
      followerSelectMenu.Show(this.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, true, true, false, true);
      UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
      selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (info =>
      {
        this.targetFollower = FollowerBrain.GetOrCreateBrain(info);
        this.pub.Brain.SetDrinkReserved(this.seat, info.ID);
      });
      UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
      selectMenuController2.OnShow = selectMenuController2.OnShow + (System.Action) (() =>
      {
        foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
        {
          followerInfoBox.ShowPleasure(CookingData.GetPleasure((InventoryItem.ITEM_TYPE) this.pub.Brain.FoodStorage.Data.Inventory[this.seat].type));
          foreach (Objectives_Drink objectivesDrink in drinkObjectives)
          {
            if (objectivesDrink.DrinkType == (InventoryItem.ITEM_TYPE) this.drink.type && objectivesDrink.TargetFollower == followerInfoBox.FollowerInfo.ID)
              followerInfoBox.ShowObjective();
          }
        }
      });
      UIFollowerSelectMenuController selectMenuController3 = followerSelectMenu;
      selectMenuController3.OnShownCompleted = selectMenuController3.OnShownCompleted + (System.Action) (() =>
      {
        foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
        {
          followerInfoBox.ShowPleasure(CookingData.GetPleasure((InventoryItem.ITEM_TYPE) this.pub.Brain.FoodStorage.Data.Inventory[this.seat].type));
          foreach (Objectives_Drink objectivesDrink in drinkObjectives)
          {
            if (objectivesDrink.DrinkType == (InventoryItem.ITEM_TYPE) this.drink.type && objectivesDrink.TargetFollower == followerInfoBox.FollowerInfo.ID)
              followerInfoBox.ShowObjective();
          }
        }
      });
      UIFollowerSelectMenuController selectMenuController4 = followerSelectMenu;
      selectMenuController4.OnHidden = selectMenuController4.OnHidden + (System.Action) (() =>
      {
        Time.timeScale = 1f;
        GameManager.GetInstance().OnConversationEnd();
        this.HasChanged = true;
      });
    }
  }

  public void UnreserveDrink()
  {
    this.pub.Brain.SetDrinkUnreserved(this.seat);
    this.HasChanged = true;
    this.playerFarming.indicator.HideTopInfo();
    this.targetFollower = (FollowerBrain) null;
  }

  public List<FollowerSelectEntry> GetFollowerSelectEntries()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!DataManager.Instance.Followers_Recruit.Contains(follower.Brain._directInfoAccess))
      {
        if (this.pub.Brain.DoesFollowerHaveDrinkReserved(follower.Brain._directInfoAccess.ID))
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableAlreadyAssignedDrink));
        else if (follower.Brain._directInfoAccess.CursedState == Thought.OldAge)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableElderly));
        else if (follower.Brain._directInfoAccess.CursedState == Thought.Child)
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableChild));
        else if (follower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableExistentialDread));
        else if (follower.Brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableMissionaryTerrified));
        else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.UnavailableZombie));
        else
          followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain._directInfoAccess, true)));
      }
    }
    return followerSelectEntries;
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    if (!this.pub.Brain.IsDrinkReserved(this.seat))
      return;
    playerFarming.indicator.ShowTopInfo(string.Format(LocalizationManager.GetTranslation("UI/DrinkReserved"), (object) this.DrinkName, (object) this.FollowerName));
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public override void Update()
  {
    base.Update();
    if (!Structures_Pub.IsDrinking || this.targetFollower == null || this.pub.Brain.FoodStorage.Data.Inventory[this.seat] == null || !FollowerManager.FollowerLocked(this.targetFollower.Info.ID, true, excludeFreezing: true) && this.targetFollower.Info.CursedState != Thought.Zombie && this.targetFollower.Info.CursedState != Thought.OldAge && this.targetFollower.Info.CursedState != Thought.Child)
      return;
    this.UnreserveDrink();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__11_0(FollowerInfo info)
  {
    this.targetFollower = FollowerBrain.GetOrCreateBrain(info);
    this.pub.Brain.SetDrinkReserved(this.seat, info.ID);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__11_3()
  {
    Time.timeScale = 1f;
    GameManager.GetInstance().OnConversationEnd();
    this.HasChanged = true;
  }
}
