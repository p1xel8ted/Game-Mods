// Decompiled with JetBrains decompiler
// Type: RanchMatingMenuItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
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
public class RanchMatingMenuItem : RanchSelectItem, IPoolListener
{
  public Action<RanchMatingMenuItem> OnSelected;
  public Action<RanchMatingMenuItem> OnHighlighted;
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
  [Header("Misc")]
  [SerializeField]
  public LayoutElement layoutBox;
  [SerializeField]
  public GameObject chosenParent;
  [SerializeField]
  public GameObject chosen;
  [Header("Unavailability")]
  [SerializeField]
  public GameObject _unavailableContainer;
  [SerializeField]
  public TMP_Text _unavailableText;
  public Tween punchTween;

  public void OnEnable()
  {
    this.Button.onClick.AddListener(new UnityAction(this.OnItemSelected));
    this.Button.OnConfirmDenied += new System.Action(this.InvalidShake);
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
    this.Button.OnConfirmDenied -= new System.Action(this.InvalidShake);
  }

  public void Configure(StructuresData.Ranchable_Animal animalInfo)
  {
    this._animalInfo = animalInfo;
    if (animalInfo != null)
    {
      this.animalSpine.ConfigureAnimal(animalInfo);
      this.animalSpine.transform.localScale = Vector3.one * 1.2785f;
      if (animalInfo.Type == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA || animalInfo.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
        this.animalSpine.transform.localScale = Vector3.one;
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
      if (this.RanchSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available)
        this.chosenParent.SetActive(true);
      else
        this.chosenParent.SetActive(false);
    }
    if ((UnityEngine.Object) this._unavailableContainer != (UnityEngine.Object) null)
    {
      if (this.RanchSelectEntry != null && this.RanchSelectEntry.AvailabilityStatus != RanchSelectEntry.Status.Available)
      {
        this._unavailableContainer.SetActive(true);
        this._unavailableText.text = this.RanchSelectEntry.AvailabilityStatus <= RanchSelectEntry.Status.Unavailable ? ScriptLocalization.UI_RanchSelect.Unavailable : $"{ScriptLocalization.UI_RanchSelect.Unavailable}: {LocalizationManager.GetTranslation($"UI/RanchSelect/{this.RanchSelectEntry.AvailabilityStatus}")}";
      }
      else
        this._unavailableContainer.SetActive(false);
    }
    this.layoutBox.preferredHeight = Mathf.Clamp(this.animalSpine.GetComponent<RectTransform>().rect.height + 20f, 172.9f, 300f);
  }

  public void OnRecycled()
  {
    this.RemoveChosen();
    this.OnSelected = (Action<RanchMatingMenuItem>) null;
    this.OnHighlighted = (Action<RanchMatingMenuItem>) null;
    this.OnFollowerSelected = (Action<RanchSelectEntry>) null;
    this.OnFollowerHighlighted = (Action<RanchSelectEntry>) null;
  }

  public void OnItemSelected()
  {
    Action<RanchMatingMenuItem> onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected(this);
  }

  public void OnItemUnhighlighted() => this.StopAllCoroutines();

  public void OnItemHighlighted()
  {
    Action<RanchMatingMenuItem> onHighlighted = this.OnHighlighted;
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

  public void SetChosen()
  {
    this.chosen.gameObject.SetActive(true);
    this.chosen.transform.localScale = Vector3.one;
  }

  public void RemoveChosen(bool showChosenParent = true)
  {
    this.chosen.gameObject.SetActive(true);
    this.chosen.transform.localScale = Vector3.zero;
    this.chosenParent.SetActive(showChosenParent);
  }

  public void InvalidShake()
  {
    if (this.punchTween != null)
      this.punchTween.Complete();
    this.punchTween = (Tween) this.transform.DOPunchPosition(Vector3.right * 10f, 0.15f, 1).SetEase<Tweener>(Ease.InOutBack).SetUpdate<Tweener>(true);
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__23_0()
  {
    Action<RanchSelectEntry> followerSelected = this.OnFollowerSelected;
    if (followerSelected == null)
      return;
    followerSelected(this.RanchSelectEntry);
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__23_1()
  {
    Action<RanchSelectEntry> followerHighlighted = this.OnFollowerHighlighted;
    if (followerHighlighted == null)
      return;
    followerHighlighted(this.RanchSelectEntry);
  }
}
