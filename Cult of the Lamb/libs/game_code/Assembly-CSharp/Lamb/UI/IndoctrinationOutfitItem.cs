// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationOutfitItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using src.UI.Alerts;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Collections;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationOutfitItem : IndoctrinationCharacterItem<IndoctrinationOutfitItem>
{
  [SerializeField]
  public ClothingAlertBadge _alert;
  public ClothingData clothingData;
  public Follower follower;
  [ReadOnly]
  public bool _lockedDebug;
  [SerializeField]
  public SkeletonGraphic followerSpine;
  [SerializeField]
  public TextMeshProUGUI _itemCount;
  public FollowerBrain followerWearingClothing;
  [CompilerGenerated]
  public string \u003CVariant\u003Ek__BackingField;
  public bool foundOne;

  public string Variant
  {
    get => this.\u003CVariant\u003Ek__BackingField;
    set => this.\u003CVariant\u003Ek__BackingField = value;
  }

  public bool IsCustomizable
  {
    get
    {
      return TailorManager.GetCraftedCount(this.clothingData.ClothingType) > 0 || this.followerWearingClothing != null || this.clothingData.ClothingType == FollowerClothingType.None;
    }
  }

  public ClothingData returnOutfitData() => this.clothingData;

  public void Configure(
    ClothingData clothingData,
    Follower follower,
    bool isSpineVisible = true,
    bool setLastSibling = true)
  {
    this.clothingData = clothingData;
    this.follower = follower;
    this._skinAndData = (WorshipperData.SkinAndData) null;
    this.followerWearingClothing = TailorManager.GetCraftedCount(clothingData.ClothingType) <= 0 ? TailorManager.GetFollowerWearingOutfit(clothingData.ClothingType) : (FollowerBrain) null;
    this.Variant = this.followerWearingClothing == null ? (clothingData.ClothingType != FollowerClothingType.None ? TailorManager.GetClothingFromTailor(clothingData.ClothingType).Variant : DataManager.Instance.GetClothingVariant(clothingData.ClothingType)) : this.followerWearingClothing.Info.ClothingVariant;
    if (string.IsNullOrEmpty(this.Variant) && clothingData.Variants.Contains(this.Variant))
      this.Variant = follower.Brain.Info.ClothingVariant;
    if ((UnityEngine.Object) this._spine != (UnityEngine.Object) null)
      this._spine.ConfigureFollowerOutfit(clothingData, DataManager.Instance.GetClothingColour(clothingData.ClothingType), this.Variant);
    this._alert.Configure(clothingData.ClothingType);
    this.foundOne = false;
    this.followerSpine.gameObject.SetActive(((this.followerWearingClothing == null || clothingData.ClothingType == FollowerClothingType.None || !((UnityEngine.Object) follower != (UnityEngine.Object) null) ? 0 : (follower.Brain != this.followerWearingClothing ? 1 : 0)) & (isSpineVisible ? 1 : 0)) != 0);
    int craftedCount = TailorManager.GetCraftedCount(clothingData.ClothingType);
    if ((bool) (UnityEngine.Object) this._itemCount)
    {
      this._itemCount.text = craftedCount.ToString();
      if (clothingData.ClothingType == FollowerClothingType.None || clothingData.SpecialClothing)
        this._itemCount.gameObject.SetActive(false);
      else if (craftedCount == 0 && this.followerWearingClothing != null)
      {
        this.followerSpine.gameObject.SetActive(true);
        this._itemCount.gameObject.SetActive(false);
      }
      else if (craftedCount == 0)
        this._itemCount.color = StaticColors.RedColor;
      else
        this._itemCount.color = StaticColors.GreenColor;
    }
    this.CheckLocked(clothingData.ClothingType != 0);
  }

  public bool CheckLocked(bool setSiblings = false)
  {
    if ((UnityEngine.Object) this.clothingData == (UnityEngine.Object) null)
      return true;
    this._selected = false;
    this.UpdateState();
    if (this.followerWearingClothing != null && this.followerSpine.gameObject.activeSelf)
      FollowerBrain.SetFollowerCostume(this.followerSpine.Skeleton, this.followerWearingClothing._directInfoAccess, forceUpdate: true, setData: false);
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.UnlockedClothing)
    {
      if (followerClothingType == this.clothingData.ClothingType)
      {
        this.foundOne = true;
        this._locked = false;
      }
    }
    if (this.foundOne || this.clothingData.ClothingType == FollowerClothingType.None)
    {
      if (setSiblings)
        this.transform.SetSiblingIndex(1);
      if (!TailorManager.GetAvailableClothing().Contains(this.clothingData) && this.clothingData.ClothingType != FollowerClothingType.None && TailorManager.GetCraftedCount(this.clothingData.ClothingType) <= 0)
      {
        this._spine.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        this.Button.Confirmable = false;
        return true;
      }
      if ((this.clothingData.ClothingType == FollowerClothingType.Robes_Fancy || this.clothingData.ClothingType == FollowerClothingType.Suit_Fancy) && !this.follower.Brain.Info.MarriedToLeader && !TailorMenu_Assign.IsFollowerInWeddingDressQuest(this.follower))
      {
        this._spine.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        this.Button.Confirmable = false;
        return true;
      }
    }
    else
    {
      this.SetLocked();
      this._lockedDebug = true;
      this._locked = true;
      this._itemCount.enabled = false;
    }
    return false;
  }

  public override void SetAsSelected()
  {
    if (this.CheckLocked())
      return;
    this._selected = true;
    this.UpdateState();
  }

  public override void SetAsDeselected()
  {
    this._selected = false;
    this.UpdateState();
  }

  public override void OnButtonClickedImpl()
  {
    Action<IndoctrinationOutfitItem> onItemSelected = this.OnItemSelected;
    if (onItemSelected == null)
      return;
    onItemSelected(this);
  }

  public override void OnButtonHighlightedImpl()
  {
    Action<IndoctrinationOutfitItem> onItemHighlighted = this.OnItemHighlighted;
    if (onItemHighlighted != null)
      onItemHighlighted(this);
    if (!((UnityEngine.Object) this._alert != (UnityEngine.Object) null))
      return;
    this._alert.TryRemoveAlert();
  }

  public void UpdateIcon(string variant)
  {
    if (!((UnityEngine.Object) this._spine != (UnityEngine.Object) null))
      return;
    this.Variant = string.IsNullOrEmpty(variant) ? this.follower.Brain.Info.ClothingVariant : variant;
    this._spine.ConfigureFollowerOutfit(this.clothingData, DataManager.Instance.GetClothingColour(this.clothingData.ClothingType), variant);
  }
}
