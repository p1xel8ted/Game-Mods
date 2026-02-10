// Decompiled with JetBrains decompiler
// Type: TailorMenu_Customise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using src.UI.Alerts;
using src.UINavigator;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TailorMenu_Customise : UISubmenuBase
{
  public UITailorMenuController _UITailorMenuController;
  [SerializeField]
  public RectTransform ScrollView;
  [SerializeField]
  public ClothingCustomiseAlertBadge _alert;
  public bool hasCrafted;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this._alert.TryRemoveAlert();
    this._UITailorMenuController._recipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._specialRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._DLCRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._winterRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._inMenu = TailorItem.InMenu.Customise;
    Debug.Log((object) "SHOWING CUSTOMISE!");
    foreach (TailorItem tailorItem in this._UITailorMenuController._recipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Customise);
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(0.0f);
      tailorItem.ShowTick(false);
      tailorItem.HideAssignedFollower();
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.CheckIfCrafted();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._specialRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Customise);
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(0.0f);
      tailorItem.ShowTick(false);
      tailorItem.HideAssignedFollower();
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.CheckIfCrafted();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._DLCRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Customise);
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(0.0f);
      tailorItem.ShowTick(false);
      tailorItem.HideAssignedFollower();
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.CheckIfCrafted();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._winterRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Customise);
      tailorItem.FadeIngredients(0.0f);
      tailorItem.SetGrey(0.0f);
      tailorItem.ShowTick(false);
      tailorItem.HideAssignedFollower();
      if (!TailorManager.GetClothingAvailable(tailorItem.ClothingData.ClothingType) && tailorItem.ClothingData.ClothingType != FollowerClothingType.None)
        tailorItem.setLocked();
      tailorItem.CheckIfCrafted();
      tailorItem.UpdateAlerts();
    }
    this._UITailorMenuController._infoCardController.Card1.SetMenu(TailorItem.InMenu.Customise);
    this._UITailorMenuController._infoCardController.Card2.SetMenu(TailorItem.InMenu.Customise);
    this.ScrollView.offsetMin = new Vector2(this.ScrollView.offsetMin.x, 125f);
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    this._UITailorMenuController._recipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._specialRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._DLCRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._winterRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    Debug.Log((object) "HIDE CUSTOMISE!");
  }

  public void OnRecipeChosen(ClothingData arg1, string arg2)
  {
    TailorItem component = MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponent<TailorItem>();
    ClothingData clothingData = component.ClothingData;
    UICustomizeClothesMenuController_Colour menu = MonoSingleton<UIManager>.Instance.CustomizeClothesColourTemplate.Instantiate<UICustomizeClothesMenuController_Colour>();
    menu.Show(clothingData.ClothingType, component.Variant);
    menu.OnClothingChanged += new Action<ClothingData, int, string>(this.UpdateFollowerInfoCard);
    menu.OnClothingSelected += (Action<ClothingData, int, string>) ((d, i, v) =>
    {
      this._UITailorMenuController._recipesMenu.UpdateItems(d.ClothingType, v);
      this._UITailorMenuController._specialRecipesMenu.UpdateItems(d.ClothingType, v);
      this._UITailorMenuController._DLCRecipesMenu.UpdateItems(d.ClothingType, v);
      this._UITailorMenuController._winterRecipesMenu.UpdateItems(d.ClothingType, v);
      this._UITailorMenuController._tailorQueue.UpdateItems(d.ClothingType, v);
      this.UpdateFollowerInfoCard(d, i, v);
    });
    this.PushInstance<UICustomizeClothesMenuController_Colour>(menu);
  }

  public void UpdateFollowerInfoCard(ClothingData data, int i, string v)
  {
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.Info.Clothing == data.ClothingType)
        FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
    }
    if ((UnityEngine.Object) this._UITailorMenuController._infoCardController.CurrentCard == (UnityEngine.Object) null)
    {
      this._UITailorMenuController._infoCardController.ForceCurrentCard(this._UITailorMenuController._infoCardController.Card1, data);
      this._UITailorMenuController._infoCardController.Card1.Configure(data, this._UITailorMenuController.tailor, v, TailorItem.InMenu.Customise);
      this._UITailorMenuController._infoCardController.Card2.Configure(data, this._UITailorMenuController.tailor, v, TailorItem.InMenu.Customise);
      this._UITailorMenuController._infoCardController.Card1.Show();
    }
    else
    {
      this._UITailorMenuController._infoCardController.Card1.Configure(data, this._UITailorMenuController.tailor, v, TailorItem.InMenu.Customise);
      this._UITailorMenuController._infoCardController.Card2.Configure(data, this._UITailorMenuController.tailor, v, TailorItem.InMenu.Customise);
    }
  }

  [CompilerGenerated]
  public void \u003COnRecipeChosen\u003Eb__6_0(ClothingData d, int i, string v)
  {
    this._UITailorMenuController._recipesMenu.UpdateItems(d.ClothingType, v);
    this._UITailorMenuController._specialRecipesMenu.UpdateItems(d.ClothingType, v);
    this._UITailorMenuController._DLCRecipesMenu.UpdateItems(d.ClothingType, v);
    this._UITailorMenuController._winterRecipesMenu.UpdateItems(d.ClothingType, v);
    this._UITailorMenuController._tailorQueue.UpdateItems(d.ClothingType, v);
    this.UpdateFollowerInfoCard(d, i, v);
  }
}
