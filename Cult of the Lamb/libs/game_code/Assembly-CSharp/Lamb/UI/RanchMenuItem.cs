// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.RanchSelect;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class RanchMenuItem : RanchSelectItem, IPoolListener
{
  public Action<RanchMenuItem> OnSelected;
  public Action<RanchMenuItem> OnHighlighted;
  [Header("States")]
  [SerializeField]
  public RectTransform purchased;
  [SerializeField]
  public RectTransform locked;
  [SerializeField]
  public RectTransform empty;
  [Header("Details")]
  [SerializeField]
  public SkeletonGraphic animalSpine;
  [SerializeField]
  public TextMeshProUGUI animalName;
  [SerializeField]
  public Image HungerLevel;
  [SerializeField]
  public TextMeshProUGUI ageText;
  [SerializeField]
  public Image adorationBar;
  [SerializeField]
  public TextMeshProUGUI harvestItem;
  [SerializeField]
  public TextMeshProUGUI sacrificeItem;
  [SerializeField]
  public TextMeshProUGUI favouriteFood;
  [SerializeField]
  public RectTransform agelevelDivider;
  [SerializeField]
  public RectTransform itemContainer;
  [SerializeField]
  public TextMeshProUGUI itemText;
  [Header("Misc")]
  [SerializeField]
  public GameObject resourceBox;
  [SerializeField]
  public LayoutElement layoutBox;

  public void OnEnable()
  {
    this.Button.onClick.AddListener(new UnityAction(this.OnItemSelected));
    this.Button.OnSelected += new System.Action(this.OnItemHighlighted);
    this.Button.OnDeselected += new System.Action(this.OnItemUnhighlighted);
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) this.Button != (UnityEngine.Object) null))
      return;
    this.Button.onClick.RemoveAllListeners();
    this.Button.OnSelected -= new System.Action(this.OnItemHighlighted);
    this.Button.OnDeselected -= new System.Action(this.OnItemUnhighlighted);
  }

  public void Configure(StructuresData.Ranchable_Animal animalInfo, bool ShowResources = true)
  {
    this._animalInfo = animalInfo;
    if (animalInfo != null)
    {
      this.animalSpine.ConfigureAnimal(animalInfo);
      this.animalSpine.transform.localScale = Vector3.one * 1.2785f;
      string format = this.AnimalInfo.Age < 15 ? (this.AnimalInfo.Age >= 2 ? "" : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Baby")) : LocalizationManager.GetTranslation("UI/RanchAssignMenu/Old");
      if (!string.IsNullOrEmpty(format))
      {
        if (this.AnimalInfo.GivenName != null)
          this.animalName.text = string.Format(format, (object) this.AnimalInfo.GivenName);
        else
          this.animalName.text = string.Format(format, (object) InventoryItem.LocalizedName(this.AnimalInfo.Type));
      }
      else if (this.AnimalInfo.GivenName != null)
        this.animalName.text = this.AnimalInfo.GivenName;
      else
        this.animalName.text = InventoryItem.LocalizedName(this.AnimalInfo.Type);
      string str = string.Join(" - ", this.animalName.text, $"{ScriptLocalization.Interactions.Level} {this.AnimalInfo.Level.ToNumeral()}");
      if (animalInfo.BestFriend)
        this.animalName.text = "\uF004 " + str;
      else
        this.animalName.text = str;
      this.adorationBar.fillAmount = animalInfo.Adoration / 100f;
      this.HungerLevel.fillAmount = animalInfo.Satiation / 100f;
      this.HungerLevel.color = this.ReturnColorBasedOnValueHunger(this.HungerLevel.fillAmount);
      this.ageText.text = string.Format(ScriptLocalization.UI_FollowerInfo.Age, (object) LocalizeIntegration.ReverseText(animalInfo.Age.ToString()));
      List<InventoryItem> workLoot = Interaction_Ranchable.GetWorkLoot(animalInfo);
      this.harvestItem.text = CostFormatter.FormatCost((InventoryItem.ITEM_TYPE) workLoot[0].type, workLoot[0].quantity, ignoreAffordability: true);
      List<InventoryItem> meatLoot = Interaction_Ranchable.GetMeatLoot(animalInfo);
      this.sacrificeItem.text = CostFormatter.FormatCost((InventoryItem.ITEM_TYPE) meatLoot[0].type, meatLoot[0].quantity, ignoreAffordability: true);
      if (animalInfo.IsFavouriteFoodRevealed)
      {
        this.favouriteFood.gameObject.SetActive(true);
        this.agelevelDivider.gameObject.SetActive(true);
        this.favouriteFood.text = "\uF004 " + FontImageNames.GetIconByType(animalInfo.FavouriteFood);
      }
      else
      {
        this.favouriteFood.gameObject.SetActive(false);
        this.agelevelDivider.gameObject.SetActive(false);
      }
      this.favouriteFood.gameObject.SetActive(false);
      this.purchased.gameObject.SetActive(true);
      this.empty.gameObject.SetActive(false);
    }
    else
    {
      this.purchased.gameObject.SetActive(false);
      this.empty.gameObject.SetActive(true);
    }
    this.layoutBox.preferredHeight = Mathf.Clamp(this.animalSpine.GetComponent<RectTransform>().rect.height + 20f, 172.9f, 300f);
    this.itemContainer.gameObject.SetActive(false);
  }

  public void OnRecycled()
  {
    this.OnSelected = (Action<RanchMenuItem>) null;
    this.OnHighlighted = (Action<RanchMenuItem>) null;
  }

  public void OnItemSelected()
  {
    Action<RanchMenuItem> onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected(this);
  }

  public void OnItemUnhighlighted() => this.StopAllCoroutines();

  public void OnItemHighlighted()
  {
    Action<RanchMenuItem> onHighlighted = this.OnHighlighted;
    if (onHighlighted == null)
      return;
    onHighlighted(this);
  }

  public Color ReturnColorBasedOnValueHunger(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.5)
      return StaticColors.RedColor;
    return (double) f >= 0.5 && (double) f < 0.7 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public override void ConfigureImpl()
  {
    this.Configure(this.AnimalInfo);
    if (!((UnityEngine.Object) this._button != (UnityEngine.Object) null))
      return;
    this._button.onClick.AddListener((UnityAction) (() =>
    {
      Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
      if (followerSelected == null)
        return;
      followerSelected(this.RanchSelectEntry);
    }));
    this._button.OnSelected += (System.Action) (() =>
    {
      Action<RanchSelectEntry> followerHighlighted = this.OnFollowerHighlighted;
      if (followerHighlighted == null)
        return;
      followerHighlighted(this.RanchSelectEntry);
    });
  }

  public void ShowDropItem()
  {
    this.itemText.text = FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) Interaction_Ranchable.GetNecklaceLoot(this.AnimalInfo).type);
    this.itemContainer.gameObject.SetActive(true);
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__26_0()
  {
    Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
    if (followerSelected == null)
      return;
    followerSelected(this.RanchSelectEntry);
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__26_1()
  {
    Action<RanchSelectEntry> followerHighlighted = this.OnFollowerHighlighted;
    if (followerHighlighted == null)
      return;
    followerHighlighted(this.RanchSelectEntry);
  }
}
