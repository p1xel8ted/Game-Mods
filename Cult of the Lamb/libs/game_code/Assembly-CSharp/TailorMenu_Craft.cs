// Decompiled with JetBrains decompiler
// Type: TailorMenu_Craft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.Alerts;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class TailorMenu_Craft : UISubmenuBase
{
  public UITailorMenuController _UITailorMenuController;
  public RectTransform ScrollView;
  [SerializeField]
  public ClothingAlertBadge _alert;
  [SerializeField]
  public GameObject _craftPrompt;

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    this._alert.TryRemoveAlert();
    this._UITailorMenuController._recipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._specialRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._DLCRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._winterRecipesMenu.OnRecipeChosen += new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._tailorQueue.OnRecipeRemoved += new Action<TailorItem, int>(this.OnRecipeRemoved);
    this._UITailorMenuController._inMenu = TailorItem.InMenu.Craft;
    Debug.Log((object) "SHOWING CRAFT!");
    foreach (TailorItem tailorItem in this._UITailorMenuController._recipesMenu._items)
    {
      tailorItem.FadeIngredients(1f);
      tailorItem.SetGrey(0.0f);
      tailorItem.UpdateQuantity();
      tailorItem.HideAssignedFollower();
      tailorItem.UpdateAlerts();
      tailorItem.SetMenu(TailorItem.InMenu.Craft);
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._specialRecipesMenu._items)
    {
      tailorItem.FadeIngredients(1f);
      tailorItem.SetGrey(0.0f);
      tailorItem.UpdateQuantity();
      tailorItem.HideAssignedFollower();
      tailorItem.UpdateAlerts();
      tailorItem.SetMenu(TailorItem.InMenu.Craft);
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._DLCRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Craft);
      tailorItem.FadeIngredients(1f);
      tailorItem.SetGrey(0.0f);
      tailorItem.UpdateQuantity();
      tailorItem.HideAssignedFollower();
      tailorItem.UpdateAlerts();
    }
    foreach (TailorItem tailorItem in this._UITailorMenuController._winterRecipesMenu._items)
    {
      tailorItem.SetMenu(TailorItem.InMenu.Craft);
      tailorItem.FadeIngredients(1f);
      tailorItem.SetGrey(0.0f);
      tailorItem.UpdateQuantity();
      tailorItem.HideAssignedFollower();
      tailorItem.UpdateAlerts();
    }
    this._UITailorMenuController._infoCardController.Card1.SetMenu(TailorItem.InMenu.Craft);
    this._UITailorMenuController._infoCardController.Card2.SetMenu(TailorItem.InMenu.Craft);
    this.ScrollView.offsetMin = new Vector2(this.ScrollView.offsetMin.x, 350f);
    this._craftPrompt.gameObject.SetActive(this._UITailorMenuController._tailorQueue.Items.Count > 0);
    this._UITailorMenuController.ScrollToCurrentSelection();
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    this._UITailorMenuController._recipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._specialRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._DLCRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._winterRecipesMenu.OnRecipeChosen -= new Action<ClothingData, string>(this.OnRecipeChosen);
    this._UITailorMenuController._tailorQueue.OnRecipeRemoved -= new Action<TailorItem, int>(this.OnRecipeRemoved);
    Debug.Log((object) "HIDE CRAFT!");
    this._craftPrompt.gameObject.SetActive(false);
  }

  public void OnRecipeRemoved(TailorItem data, int i)
  {
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => this._craftPrompt.gameObject.SetActive(this._UITailorMenuController._tailorQueue.Items.Count > 0)));
  }

  public void OnRecipeChosen(ClothingData data, string variant)
  {
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => this._craftPrompt.gameObject.SetActive(this._UITailorMenuController._tailorQueue.Items.Count > 0)));
    this._UITailorMenuController.OnRecipeChosen(data, variant);
  }

  [CompilerGenerated]
  public void \u003COnRecipeRemoved\u003Eb__6_0()
  {
    this._craftPrompt.gameObject.SetActive(this._UITailorMenuController._tailorQueue.Items.Count > 0);
  }

  [CompilerGenerated]
  public void \u003COnRecipeChosen\u003Eb__7_0()
  {
    this._craftPrompt.gameObject.SetActive(this._UITailorMenuController._tailorQueue.Items.Count > 0);
  }
}
