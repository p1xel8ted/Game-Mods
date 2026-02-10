// Decompiled with JetBrains decompiler
// Type: RanchSelectMenuInformationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI.RanchSelect;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class RanchSelectMenuInformationBox : RanchSelectItem, IPoolListener
{
  public SkeletonGraphic FollowerSpine;
  public TextMeshProUGUI FollowerName;
  public TextMeshProUGUI FollowerRole;
  public Image HungerLevel;
  public Image IllnessLevel;
  public LayoutElement ItemLayoutElement;
  [SerializeField]
  public GameObject _unavailableContainer;
  [SerializeField]
  public TMP_Text _unavailableText;
  [SerializeField]
  public GameObject _chosenParent;
  [SerializeField]
  public GameObject _chosen;
  public string AgeString;
  public GameObject ItemParent;
  public TMP_Text itemText;
  public TMP_Text itemQuantityText;
  public GameObject SecondaryItemParent;
  public TMP_Text SecondaryitemText;
  public TMP_Text SecondaryitemQuantityText;
  [SerializeField]
  public GameObject _availableSlot;
  [SerializeField]
  public CanvasGroup _mainCanvasGroup;
  [CompilerGenerated]
  public InventoryItem.ITEM_TYPE \u003CItemCostType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CCost\u003Ek__BackingField;
  public List<InventoryItem> Costs;
  public Tween punchTween;
  public RanchSelectEntry.Status cachedStatus;

  public InventoryItem.ITEM_TYPE ItemCostType
  {
    get => this.\u003CItemCostType\u003Ek__BackingField;
    set => this.\u003CItemCostType\u003Ek__BackingField = value;
  }

  public int Cost
  {
    get => this.\u003CCost\u003Ek__BackingField;
    set => this.\u003CCost\u003Ek__BackingField = value;
  }

  public override void ConfigureImpl()
  {
    if (this.RanchSelectEntry.AnimalInfo.Type == InventoryItem.ITEM_TYPE.NONE)
    {
      this._availableSlot.SetActive(true);
      this._mainCanvasGroup.alpha = 0.0f;
    }
    else
    {
      this._availableSlot.SetActive(false);
      this.ItemParent.SetActive(false);
      if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
      {
        this._button.onClick.AddListener((UnityAction) (() =>
        {
          if (!this._button.Confirmable)
            return;
          if (this.Costs != null)
          {
            bool flag = true;
            foreach (InventoryItem cost in this.Costs)
            {
              if (Inventory.GetItemQuantity(cost.type) < cost.quantity)
                flag = false;
            }
            if (flag)
            {
              Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
              if (followerSelected != null)
                followerSelected(this.RanchSelectEntry);
              if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
                return;
              this._chosen.gameObject.SetActive(true);
            }
            else
              this.InvalidShake();
          }
          else if (this.Cost == 0 || Inventory.GetItemQuantity(this.ItemCostType) >= this.Cost * -1)
          {
            Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
            if (followerSelected != null)
              followerSelected(this.RanchSelectEntry);
            if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
              return;
            this._chosen.gameObject.SetActive(true);
          }
          else
            this.InvalidShake();
        }));
        this._button.OnConfirmDenied += new System.Action(this.InvalidShake);
        this._button.OnSelected += (System.Action) (() =>
        {
          Action<RanchSelectEntry> followerHighlighted = this.OnFollowerHighlighted;
          if (followerHighlighted == null)
            return;
          followerHighlighted(this.RanchSelectEntry);
        });
      }
      if ((UnityEngine.Object) this._unavailableContainer != (UnityEngine.Object) null)
      {
        if (this.RanchSelectEntry != null && this.RanchSelectEntry.AvailabilityStatus != RanchSelectEntry.Status.Available)
        {
          this._unavailableContainer.SetActive(true);
          this._unavailableText.text = this.RanchSelectEntry.AvailabilityStatus <= RanchSelectEntry.Status.Unavailable ? ScriptLocalization.UI_FollowerSelect.Unavailable : $"{ScriptLocalization.UI_FollowerSelect.Unavailable}: {LocalizationManager.GetTranslation($"UI/FollowerSelect/{this.RanchSelectEntry.AvailabilityStatus}")}";
        }
        else
          this._unavailableContainer.SetActive(false);
      }
      if (this.AnimalInfo.GivenName != null)
        this.FollowerName.text = this.AnimalInfo.GivenName;
      else
        this.FollowerName.text = InventoryItem.LocalizedName(this.AnimalInfo.Type);
      this.AgeString = string.Format(LocalizationManager.GetTranslation("UI/FollowerInfo/Age"), (object) LocalizeIntegration.ReverseText(this.AnimalInfo.Age.ToString()));
      this.FollowerRole.text = this.AgeString;
      this.SetDropItems();
      if ((UnityEngine.Object) this.FollowerSpine != (UnityEngine.Object) null)
      {
        this.FollowerSpine.Skeleton.SetSkin((Skin) null);
        this.FollowerSpine.ConfigureAnimal(this.AnimalInfo);
      }
      this.FollowerSpine.transform.localScale = Vector3.one * 1.2785f;
      this.HungerLevel.fillAmount = this.RanchSelectEntry.AnimalInfo.Satiation / 100f;
      this.HungerLevel.color = this.ReturnColorBasedOnValueHunger(this.HungerLevel.fillAmount);
      if (this.RanchSelectEntry == null)
        return;
      this.cachedStatus = this.RanchSelectEntry.AvailabilityStatus;
    }
  }

  public void SetDropItems()
  {
    this.itemQuantityText.gameObject.SetActive(false);
    this.SecondaryItemParent.SetActive(false);
    this.itemText.text = FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) Interaction_Ranchable.GetNecklaceLoot(this.AnimalInfo).type);
    this.ItemParent.SetActive(this.ShowNecklaceReward);
  }

  public void InvalidShake()
  {
    if (this.punchTween != null)
      this.punchTween.Complete();
    this.punchTween = (Tween) this.transform.DOPunchPosition(Vector3.right * 10f, 0.15f, 1).SetEase<Tweener>(Ease.InOutBack).SetUpdate<Tweener>(true);
  }

  public Color ReturnColorBasedOnValue(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.25)
      return StaticColors.RedColor;
    return (double) f >= 0.25 && (double) f < 0.5 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public Color ReturnColorBasedOnValueHunger(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.5)
      return StaticColors.RedColor;
    return (double) f >= 0.5 && (double) f < 0.7 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public void OnRecycled()
  {
    this.FollowerSpine.SetAnimation("idle", true);
    this.HungerLevel.transform.parent.parent.gameObject.SetActive(true);
    this.IllnessLevel.transform.parent.parent.gameObject.SetActive(true);
    this.ItemParent.SetActive(false);
    this._animalInfo = (StructuresData.Ranchable_Animal) null;
    this._button.interactable = true;
    this._button.onClick.RemoveAllListeners();
    this._button.OnConfirmDenied = (System.Action) null;
    this._button.Confirmable = true;
    this._chosen.gameObject.SetActive(false);
    this.OnFollowerSelected = (Action<RanchSelectEntry>) null;
    this.itemText.text = string.Empty;
    this.itemText.fontSizeMax = 40f;
    this.ItemLayoutElement.preferredWidth = 130f;
    this._chosenParent.gameObject.SetActive(false);
  }

  public void EnableChosen()
  {
    this.ItemParent.gameObject.SetActive(true);
    this._chosen.gameObject.SetActive(false);
    this._chosen.transform.localScale = Vector3.zero;
    this._chosenParent.gameObject.SetActive(true);
  }

  public void SetChosen()
  {
    Debug.Log((object) ("SET CHOSEN " + Time.realtimeSinceStartup.ToString()));
    this.RanchSelectEntry.AvailabilityStatus = RanchSelectEntry.Status.Unavailable;
    this._chosen.gameObject.SetActive(true);
    this._chosen.transform.localScale = Vector3.one;
  }

  public void RemoveChosen(bool showChosenParent = true)
  {
    this.RanchSelectEntry.AvailabilityStatus = this.cachedStatus;
    this._chosen.gameObject.SetActive(true);
    this._chosen.transform.localScale = Vector3.zero;
    this._chosenParent.SetActive(showChosenParent);
  }

  public void OnDisable() => this.FollowerRole.text = "";

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__28_0()
  {
    if (!this._button.Confirmable)
      return;
    if (this.Costs != null)
    {
      bool flag = true;
      foreach (InventoryItem cost in this.Costs)
      {
        if (Inventory.GetItemQuantity(cost.type) < cost.quantity)
          flag = false;
      }
      if (flag)
      {
        Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
        if (followerSelected != null)
          followerSelected(this.RanchSelectEntry);
        if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
          return;
        this._chosen.gameObject.SetActive(true);
      }
      else
        this.InvalidShake();
    }
    else if (this.Cost == 0 || Inventory.GetItemQuantity(this.ItemCostType) >= this.Cost * -1)
    {
      Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
      if (followerSelected != null)
        followerSelected(this.RanchSelectEntry);
      if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
        return;
      this._chosen.gameObject.SetActive(true);
    }
    else
      this.InvalidShake();
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__28_1()
  {
    Action<RanchSelectEntry> followerHighlighted = this.OnFollowerHighlighted;
    if (followerHighlighted == null)
      return;
    followerHighlighted(this.RanchSelectEntry);
  }
}
