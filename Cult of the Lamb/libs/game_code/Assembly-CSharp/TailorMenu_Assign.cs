// Decompiled with JetBrains decompiler
// Type: TailorMenu_Assign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using src.UI.InfoCards;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class TailorMenu_Assign : UISubmenuBase
{
  public UITailorMenuController _UITailorMenuController;
  public RectTransform ScrollView;
  [SerializeField]
  public TextMeshProUGUI noOutfitText;
  [SerializeField]
  public ClothingAssignAlertBadge _alert;
  public static FollowerClothingType CurrentAssigningClothingType;
  public const int WEDDING_DRESS_QUEST_INDEX = 53;
  public bool hasUnlocked;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    Debug.Log((object) "SHOWING ASSIGN!");
    this._alert.TryRemoveAlert();
    this._UITailorMenuController._recipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._specialRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._DLCRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._winterRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._inMenu = TailorItem.InMenu.Assign;
    this.UpdateItems();
  }

  public void CheckAnyAvailable()
  {
    this.noOutfitText.gameObject.SetActive(false);
    this.hasUnlocked = false;
    foreach (TailorItem tailorItem in this._UITailorMenuController._recipesMenu._items)
    {
      if (TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) > 0)
        this.hasUnlocked = true;
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._specialRecipesMenu._items)
    {
      FollowerBrain followerWearingOutfit = TailorManager.GetFollowerWearingOutfit(tailorItem.ClothingData.ClothingType);
      if ((TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) > 0 ? 1 : (followerWearingOutfit != null ? 1 : 0)) != 0)
        this.hasUnlocked = true;
    }
    if (this.hasUnlocked)
      return;
    this.noOutfitText.gameObject.SetActive(true);
  }

  public void UpdateItems()
  {
    foreach (TailorItem tailorItem in this._UITailorMenuController._recipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Assign);
      tailorItem.UpdateQuantity();
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) <= 0 ? 0.75f : 0.0f);
      tailorItem.ShowTick(false);
      tailorItem.Button.Confirmable = TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) > 0;
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._specialRecipesMenu._items)
    {
      FollowerBrain followerWearingOutfit = TailorManager.GetFollowerWearingOutfit(tailorItem.ClothingData.ClothingType);
      bool flag = TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) > 0 || followerWearingOutfit != null;
      tailorItem.SetMenu(TailorItem.InMenu.Assign);
      tailorItem.UpdateQuantity();
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(flag ? 0.0f : 0.75f);
      tailorItem.ShowTick(false);
      tailorItem.Button.Confirmable = flag;
      if (followerWearingOutfit != null)
        tailorItem.ShowAssignedFollower(followerWearingOutfit);
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._DLCRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Assign);
      tailorItem.UpdateQuantity();
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) <= 0 ? 0.75f : 0.0f);
      tailorItem.ShowTick(false);
      tailorItem.Button.Confirmable = TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) > 0;
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._winterRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Assign);
      tailorItem.UpdateQuantity();
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) <= 0 ? 0.75f : 0.0f);
      tailorItem.ShowTick(false);
      tailorItem.Button.Confirmable = TailorManager.GetCraftedCount(tailorItem.ClothingData.ClothingType) > 0;
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.UpdateAlerts();
    }
    this._UITailorMenuController._infoCardController.Card1.SetMenu(TailorItem.InMenu.Assign);
    this._UITailorMenuController._infoCardController.Card2.SetMenu(TailorItem.InMenu.Assign);
    this.ScrollView.offsetMin = new Vector2(this.ScrollView.offsetMin.x, 125f);
    this.CheckAnyAvailable();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    Debug.Log((object) "HIDE ASSIGN!");
    this._UITailorMenuController._recipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._specialRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._DLCRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._winterRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
  }

  public static bool IsFollowerInWeddingDressQuest(Follower follower)
  {
    bool flag = false;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Index == 53 && objective.Follower == follower.Brain.Info.ID)
        flag = true;
    }
    return flag;
  }

  public void OnRecipeChosen(ClothingData arg1, string arg2)
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (FollowerManager.GetFollowerAvailabilityStatus(follower.Brain) != FollowerSelectEntry.Status.UnavailableNewRecruit)
      {
        if (follower.Brain.Info.Clothing == arg1.ClothingType && follower.Brain.Info.ClothingVariant == arg2)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.UnavailableAlreadyWearingOutfit));
        else if (follower.Brain.Info.CursedState == Thought.OldAge && arg1.ClothingType == FollowerClothingType.None)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.UnavailableElderly));
        else if ((arg1.ClothingType == FollowerClothingType.Robes_Fancy || arg1.ClothingType == FollowerClothingType.Suit_Fancy) && !follower.Brain.Info.MarriedToLeader && !TailorMenu_Assign.IsFollowerInWeddingDressQuest(follower))
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.UnavailableNotMarried));
        else if (FollowerManager.FollowerLocked(follower.Brain.Info.ID) || follower.Brain._directInfoAccess.IsSnowman)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
        else if (follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
        else
          followerSelectEntries.Add(new FollowerSelectEntry(follower));
      }
    }
    TailorMenu_Assign.CurrentAssigningClothingType = arg1.ClothingType;
    UITailorMenuController controller = this.GetComponentInParent<UITailorMenuController>();
    UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    menu.VotingType = TwitchVoting.VotingType.ASSIGN_CLOTHING;
    menu.SetBackgroundState(false);
    menu.Show(followerSelectEntries, followerSelectionType: UpgradeSystem.Type.Building_Tailor);
    UIFollowerSelectMenuController selectMenuController1 = menu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (Action<FollowerInfo>) (follower =>
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
      controller._infoCardController.Card1.SetTargetFollower(brain);
      controller._infoCardController.Card2.SetTargetFollower(brain);
      if (brain.Info.Clothing != FollowerClothingType.None)
        this._UITailorMenuController.tailor.Data.AllClothing.Add(new StructuresData.ClothingStruct(brain.Info.Clothing, brain.Info.ClothingVariant));
      if (arg1.SpecialClothing)
        TailorManager.RemoveClothingFromDeadFollower(arg1.ClothingType);
      if (TailorManager.GetCraftedCount(arg1.ClothingType) <= 0)
        TailorManager.RemoveClothingFromFollower(arg1.ClothingType);
      TailorManager.RemoveClothingFromTailor(arg1.ClothingType);
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null))
        return;
      this._UITailorMenuController.Hide();
      Interaction_Tailor.Instance.AssignFollowerClothing(followerById, arg1.ClothingType, arg2);
      DataManager.Instance.previouslyAssignedClothing = arg1.ClothingType;
    });
    UIFollowerSelectMenuController selectMenuController2 = menu;
    selectMenuController2.OnFollowerHighlighted = selectMenuController2.OnFollowerHighlighted + (Action<FollowerInfo>) (info =>
    {
      this.SetCard(controller._infoCardController.Card1, arg1, arg2, info);
      this.SetCard(controller._infoCardController.Card2, arg1, arg2, info);
      controller._infoCardController.ForceCurrentCard((TailorInfoCard) null, arg1);
    });
    this.PushInstance<UIFollowerSelectMenuController>(menu);
    UIFollowerSelectMenuController selectMenuController3 = menu;
    selectMenuController3.OnShown = selectMenuController3.OnShown + (System.Action) (() => controller._infoCardController.ShowCardWithParam(arg1));
    UIFollowerSelectMenuController selectMenuController4 = menu;
    selectMenuController4.OnHide = selectMenuController4.OnHide + (System.Action) (() =>
    {
      controller._infoCardController.Card1.Configure(arg1, this._UITailorMenuController.tailor, arg2, TailorItem.InMenu.Assign);
      controller._infoCardController.Card2.Configure(arg1, this._UITailorMenuController.tailor, arg2, TailorItem.InMenu.Assign);
    });
  }

  public void SetCard(TailorInfoCard card, ClothingData arg1, string arg2, FollowerInfo info)
  {
    card.SetTargetFollower(FollowerBrain.GetOrCreateBrain(info));
    card.Configure(arg1, this._UITailorMenuController.tailor, arg2, TailorItem.InMenu.Assign, false);
    card.Show();
    card.CheckIfAlreadyAssigned(info);
  }
}
