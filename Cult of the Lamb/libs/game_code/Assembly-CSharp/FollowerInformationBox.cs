// Decompiled with JetBrains decompiler
// Type: FollowerInformationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class FollowerInformationBox : FollowerSelectItem, IPoolListener
{
  public SkeletonGraphic FollowerSpine;
  public TextMeshProUGUI FollowerName;
  public TextMeshProUGUI FollowerRole;
  public TextMeshProUGUI FollowerSpouse;
  [SerializeField]
  public Image _adorationLevel;
  [SerializeField]
  public Image _adorationDeltaLevel;
  [SerializeField]
  public GameObject _adorationContainer;
  [SerializeField]
  public Image _pleasureLevel;
  [SerializeField]
  public Image _pleasureDeltaLevel;
  [SerializeField]
  public GameObject _pleasureContainer;
  public Image HungerLevel;
  public Image IllnessLevel;
  public Image TiredLevel;
  public LayoutElement ItemLayoutElement;
  [SerializeField]
  public GameObject _difficulty;
  [SerializeField]
  public Image _difficultyBar;
  [SerializeField]
  public GameObject _unavailableContainer;
  [SerializeField]
  public TMP_Text _unavailableText;
  [SerializeField]
  public GameObject _chosenParent;
  [SerializeField]
  public GameObject _chosen;
  public List<FollowerThoughtObject> FollowerThoughts = new List<FollowerThoughtObject>();
  public string tmpstring;
  public string AgeString;
  public string SpouseString;
  public string ParentsString;
  public string AdditionalString;
  public FollowerBrain followBrain;
  public GameObject ItemParent;
  public TMP_Text itemText;
  [CompilerGenerated]
  public InventoryItem.ITEM_TYPE \u003CItemCostType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CCost\u003Ek__BackingField;
  public List<InventoryItem> Costs;
  public string LevelIcon = "\uF102";
  public Tween punchTween;
  public FollowerSelectEntry.Status cachedStatus;

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
    if (this.FollowerSelectEntry != null && this.FollowerSelectEntry.AvailabilityStatus == FollowerSelectEntry.Status.Available)
      this.Button.Confirmable = true;
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
            Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
            if (followerSelected != null)
              followerSelected(this._followerInfo);
            if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
              return;
            this._chosen.gameObject.SetActive(true);
          }
          else
            this.InvalidShake();
        }
        else if (this.Cost == 0 || Inventory.GetItemQuantity(this.ItemCostType) >= this.Cost * -1)
        {
          Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
          if (followerSelected != null)
            followerSelected(this._followerInfo);
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
        Action<FollowerInfo> followerHighlighted = this.OnFollowerHighlighted;
        if (followerHighlighted == null)
          return;
        followerHighlighted(this._followerInfo);
      });
    }
    if ((UnityEngine.Object) this._unavailableContainer != (UnityEngine.Object) null)
    {
      if (this.FollowerSelectEntry != null && this.FollowerSelectEntry.AvailabilityStatus != FollowerSelectEntry.Status.Available)
      {
        this._unavailableContainer.SetActive(true);
        this._unavailableText.text = this.FollowerSelectEntry.AvailabilityStatus <= FollowerSelectEntry.Status.Unavailable ? ScriptLocalization.UI_FollowerSelect.Unavailable : $"{ScriptLocalization.UI_FollowerSelect.Unavailable}: {LocalizationManager.GetTranslation($"UI/FollowerSelect/{this.FollowerSelectEntry.AvailabilityStatus}")}";
      }
      else
        this._unavailableContainer.SetActive(false);
    }
    this.FollowerName.text = this._followerInfo.GetNameFormatted();
    this.tmpstring = this._followerInfo.MemberDuration != 0 ? string.Format(LocalizationManager.GetTranslation("UI/FollowerInfo/MemberDuration"), (object) LocalizeIntegration.ReverseText(this._followerInfo.MemberDuration.ToString())) : LocalizationManager.GetTranslation("UI/FollowerInfo/MemberStatusNew");
    this.AgeString = string.Format(LocalizationManager.GetTranslation("UI/FollowerInfo/Age"), (object) LocalizeIntegration.ReverseText(this._followerInfo.Age.ToString()));
    if (this._followerInfo.SpouseFollowerID != -1)
      this.SpouseString = "<sprite name=\"icon_Married\"> " + FollowerInfo.GetInfoByID(this._followerInfo.SpouseFollowerID, true)?.Name;
    this.ParentsString = "";
    if (this._followerInfo.Parents.Count >= 2 && (bool) (UnityEngine.Object) this.GetComponentInParent<UIFollowerSummaryMenuController>())
    {
      string str1 = $"<color=#FFD201>{this._followerInfo.Parent1Name}</color>";
      string str2 = $"<color=#FFD201>{this._followerInfo.Parent2Name}</color>";
      this.ParentsString = string.Format(LocalizationManager.GetTranslation("UI/MatingTent/IsChild"), (object) "", (object) $"{str1} + {str2}");
      this.ParentsString = this.ParentsString.Substring(30);
      this.ParentsString = $"<br><color=#FFD201>{this._followerInfo.Name}</color>{this.ParentsString}";
    }
    if ((UnityEngine.Object) this.FollowerRole != (UnityEngine.Object) null)
      this.FollowerRole.text = $"{this.tmpstring} : {this.AgeString}{this.ParentsString}{this.AdditionalString}";
    if ((UnityEngine.Object) this.FollowerSpouse != (UnityEngine.Object) null)
    {
      if (string.IsNullOrEmpty(this.SpouseString))
      {
        this.FollowerSpouse.gameObject.SetActive(false);
      }
      else
      {
        this.FollowerSpouse.gameObject.SetActive(true);
        this.FollowerSpouse.text = this.SpouseString;
      }
    }
    if ((UnityEngine.Object) this.FollowerSpine != (UnityEngine.Object) null)
      this.FollowerSpine.ConfigureFollower(this._followerInfo);
    if ((UnityEngine.Object) this._adorationDeltaLevel != (UnityEngine.Object) null)
      this._adorationDeltaLevel.fillAmount = 0.0f;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.ID == this._followerInfo.ID)
      {
        this.followBrain = allBrain;
        break;
      }
    }
    this.FollowerSpine.transform.localScale = Vector3.one * 1.2785f;
    if (this.followBrain == null)
    {
      this._adorationLevel.fillAmount = this._followerInfo.Adoration / 100f;
      this.HungerLevel.transform.parent.parent.gameObject.SetActive(false);
      this.IllnessLevel.transform.parent.parent.gameObject.SetActive(false);
      this.TiredLevel.transform.parent.parent.gameObject.SetActive(false);
      this._adorationContainer.gameObject.SetActive(!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated));
      this._pleasureContainer.gameObject.SetActive(false);
      if (DataManager.Instance.Followers_Dead.Contains(this._followerInfo))
      {
        this.FollowerSpine.SetFaceAnimation("dead", true);
        this.FollowerSpine.SetAnimation("dead");
      }
    }
    else
    {
      this._adorationLevel.fillAmount = this.followBrain.Stats.Adoration / this.followBrain.Stats.MAX_ADORATION;
      this.HungerLevel.fillAmount = (float) (((double) this.followBrain.Stats.Satiation + (75.0 - (double) this.followBrain.Stats.Starvation)) / 175.0);
      this._adorationContainer.gameObject.SetActive(!this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated));
      this._pleasureContainer.gameObject.SetActive(false);
      this.HungerLevel.color = this.ReturnColorBasedOnValueHunger(this.HungerLevel.fillAmount);
      this.IllnessLevel.fillAmount = (float) (((double) this._followerInfo.Illness / 100.0 - 1.0) * -1.0);
      this.IllnessLevel.color = this.ReturnColorBasedOnValue(this.IllnessLevel.fillAmount);
      this.TiredLevel.fillAmount = this._followerInfo.Rest / 100f;
      this.TiredLevel.color = this.ReturnColorBasedOnValue(this.TiredLevel.fillAmount);
      if (this.followBrain.Info.CursedState == Thought.Child)
      {
        this.FollowerSpine.transform.localScale = Vector3.one;
        if (this.followBrain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
          this.FollowerSpine.SetAnimation(this.followBrain.Info.Age < 10 ? "Baby/Baby-zombie/baby-idle-sit-zombie" : "Baby/Baby-zombie/baby-idle-stand-zombie", true);
      }
      else if (this.followBrain.Info.CursedState == Thought.Ill)
        this.FollowerSpine.SetAnimation("Sick/idle-sick", true);
      else if (this.followBrain.Info.CursedState == Thought.Injured)
        this.FollowerSpine.SetAnimation("Injured/idle", true);
      else if (this.followBrain.Info.CursedState == Thought.Freezing)
        this.FollowerSpine.SetAnimation("Freezing/idle", true);
      else if (this.followBrain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
        this.FollowerSpine.SetAnimation("Zombie/zombie-idle", true);
      else if (this.followBrain.Info.HasTrait(FollowerTrait.TraitType.ExistentialDread) || this.followBrain.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
        this.FollowerSpine.SetAnimation("Existential Dread/dread-idle", true);
    }
    this.TiredLevel.transform.parent.parent.gameObject.SetActive(false);
    int index = 0;
    List<ThoughtData> thoughtDataList = new List<ThoughtData>();
    thoughtDataList.Clear();
    foreach (ThoughtData thought in this._followerInfo.Thoughts)
      thoughtDataList.Add(thought);
    thoughtDataList.Reverse();
    foreach (ThoughtData t in thoughtDataList)
    {
      if (index <= this.FollowerThoughts.Count - 1)
      {
        this.FollowerThoughts[index].gameObject.SetActive(true);
        this.FollowerThoughts[index].Init(t);
        ++index;
      }
    }
    for (; index <= this.FollowerThoughts.Count - 1; ++index)
      this.FollowerThoughts[index].gameObject.SetActive(false);
    if (this.FollowerSelectEntry == null)
      return;
    this.cachedStatus = this.FollowerSelectEntry.AvailabilityStatus;
  }

  public void AppendFollowerRole(string text)
  {
    this.FollowerRole.text = $"{this.tmpstring} : {this.AgeString}{this.ParentsString}";
    this.AdditionalString = "<br>" + text;
    this.FollowerRole.text += this.AdditionalString;
  }

  public void ShowRewardItem(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    this.itemText.text = $"{FontImageNames.GetIconByType(itemType)} x{quantity.ToString()}";
    this.ItemParent.SetActive(true);
  }

  public void ShowCostItem(InventoryItem.ITEM_TYPE itemType, int cost, bool ForceRed = true)
  {
    this.itemText.text = $"{FontImageNames.GetIconByType(itemType)} {cost.ToString()}";
    this.ItemParent.SetActive(true);
    if (ForceRed)
      this.itemText.color = Color.red;
    this.ItemCostType = itemType;
    this.Cost = cost * -1;
  }

  public void ShowItemCostValue(InventoryItem.ITEM_TYPE itemType, int cost)
  {
    StructuresData.ItemCost itemCost = new StructuresData.ItemCost(itemType, cost);
    this.ItemParent.SetActive(true);
    this.Cost = cost;
    this.ItemCostType = itemType;
    this.itemText.text = itemCost.ToStringShowQuantity();
    this.itemText.fontSizeMax = 28f;
    this.ItemLayoutElement.preferredWidth = 190f;
  }

  public void ShowItemCostValue(List<InventoryItem> cost)
  {
    this.itemText.text = "";
    this.Costs = cost;
    foreach (InventoryItem inventoryItem in cost)
    {
      StructuresData.ItemCost itemCost = new StructuresData.ItemCost((InventoryItem.ITEM_TYPE) inventoryItem.type, inventoryItem.quantity);
      this.ItemParent.SetActive(true);
      TMP_Text itemText = this.itemText;
      itemText.text = $"{itemText.text}{itemCost.ToStringShowQuantity()}<br>";
      this.itemText.fontSizeMax = 28f;
      this.ItemLayoutElement.preferredWidth = 190f;
    }
  }

  public void ShowFaithGain(int gain, float MAX_ADORATION, bool showDelta = true)
  {
    this.ItemParent.SetActive(true);
    this.itemText.text = $"<color=red> {this.LevelIcon} </color>+{((float) ((double) gain / (double) MAX_ADORATION * 100.0)).ToString()}%";
    this.itemText.color = StaticColors.OffWhiteColor;
    if (!showDelta || !((UnityEngine.Object) this._adorationDeltaLevel != (UnityEngine.Object) null))
      return;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(0.5f).SetUpdate<DG.Tweening.Sequence>(true);
    sequence.Append((Tween) this._adorationDeltaLevel.DOFillAmount(Mathf.Clamp01((float) (((double) this._followerInfo.Adoration + (double) gain) / 100.0)), 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine)).SetUpdate<DG.Tweening.Sequence>(true);
    sequence.Play<DG.Tweening.Sequence>();
  }

  public void ShowDevotionGain(int gain)
  {
    this.ItemParent.SetActive(true);
    this.itemText.text = $"<color=red> <sprite name=\"icon_Faith\"> </color>+{gain.ToString()}%";
    this.itemText.color = StaticColors.OffWhiteColor;
  }

  public void ShowMarried(bool confirmable = false)
  {
    this.itemText.text = "<sprite name=\"icon_Married\">";
    this.ItemParent.SetActive(true);
    this._button.Confirmable = confirmable;
  }

  public void InvalidShake()
  {
    if (this.punchTween != null)
      this.punchTween.Complete();
    this.punchTween = (Tween) this.transform.DOPunchPosition(Vector3.right * 10f, 0.15f, 1).SetEase<Tweener>(Ease.InOutBack).SetUpdate<Tweener>(true);
  }

  public void ShowObjective()
  {
    if (!string.IsNullOrEmpty(this.itemText.text))
      this.itemText.text += "\n";
    this.itemText.text += "<sprite name=\"icon_NewIcon\">";
    this.ItemParent.SetActive(true);
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
    this.FollowerSpine.SetFaceAnimation("Emotions/emotion-normal", true);
    this.FollowerSpine.SetAnimation("idle", true);
    this._adorationLevel.transform.parent.parent.gameObject.SetActive(true);
    this.HungerLevel.transform.parent.parent.gameObject.SetActive(true);
    this.IllnessLevel.transform.parent.parent.gameObject.SetActive(true);
    this.TiredLevel.transform.parent.parent.gameObject.SetActive(false);
    this.ItemParent.SetActive(false);
    this.followBrain = (FollowerBrain) null;
    this._followerInfo = (FollowerInfo) null;
    this._button.interactable = true;
    this._button.onClick.RemoveAllListeners();
    this._button.OnConfirmDenied = (System.Action) null;
    this._button.Confirmable = true;
    this._chosen.gameObject.SetActive(false);
    this.OnFollowerSelected = (Action<FollowerInfo>) null;
    this.itemText.text = string.Empty;
    this.itemText.fontSizeMax = 40f;
    this.ItemLayoutElement.preferredWidth = 130f;
    this._chosenParent.gameObject.SetActive(false);
    this._difficulty.gameObject.SetActive(false);
    this.SpouseString = (string) null;
    this.Costs = (List<InventoryItem>) null;
    this.Cost = 0;
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
    this.FollowerSelectEntry.AvailabilityStatus = FollowerSelectEntry.Status.Unavailable;
    this._chosen.gameObject.SetActive(true);
    this._chosen.transform.localScale = Vector3.one;
  }

  public void RemoveChosen(bool showChosenParent = true)
  {
    this.FollowerSelectEntry.AvailabilityStatus = this.cachedStatus;
    this._chosen.gameObject.SetActive(true);
    this._chosen.transform.localScale = Vector3.zero;
    this._chosenParent.SetActive(showChosenParent);
  }

  public void ShowPleasure(FollowerBrain.PleasureActions Delta, bool hideLoyalty = true)
  {
    int num = 0;
    if (this.followBrain.HasTrait(FollowerTrait.TraitType.MusicLover) && Delta == FollowerBrain.PleasureActions.DrumCircle)
      num += 5;
    this.ShowPleasure(FollowerBrain.GetPleasureAmount(Delta) + num, hideLoyalty);
  }

  public void ShowPleasure(int Delta, bool hideLoyalty = true)
  {
    this._pleasureContainer.gameObject.SetActive(true);
    this._adorationContainer.gameObject.SetActive(!hideLoyalty);
    if (!hideLoyalty && this._followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
      this._adorationContainer.gameObject.SetActive(false);
    this._pleasureLevel.fillAmount = (float) this._followerInfo.Pleasure / 65f;
    this._pleasureDeltaLevel.fillAmount = 0.0f;
    DG.Tweening.Sequence s = DOTween.Sequence();
    s.AppendInterval(0.5f).SetUpdate<DG.Tweening.Sequence>(true);
    s.Append((Tween) this._pleasureDeltaLevel.DOFillAmount(Mathf.Clamp01((float) (this._followerInfo.Pleasure + Delta) / 65f), 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine)).SetUpdate<DG.Tweening.Sequence>(true);
  }

  public void ShowDifficulty(float norm)
  {
    this.ItemParent.gameObject.SetActive(true);
    this.itemText.text = "";
    this._chosen.gameObject.SetActive(false);
    this._difficulty.gameObject.SetActive(true);
    this._difficultyBar.fillAmount = norm;
  }

  public void OnDisable()
  {
    this.AdditionalString = "";
    this.FollowerRole.text = "";
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__38_0()
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
        Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
        if (followerSelected != null)
          followerSelected(this._followerInfo);
        if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
          return;
        this._chosen.gameObject.SetActive(true);
      }
      else
        this.InvalidShake();
    }
    else if (this.Cost == 0 || Inventory.GetItemQuantity(this.ItemCostType) >= this.Cost * -1)
    {
      Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
      if (followerSelected != null)
        followerSelected(this._followerInfo);
      if (!((UnityEngine.Object) this._chosen != (UnityEngine.Object) null))
        return;
      this._chosen.gameObject.SetActive(true);
    }
    else
      this.InvalidShake();
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__38_1()
  {
    Action<FollowerInfo> followerHighlighted = this.OnFollowerHighlighted;
    if (followerHighlighted == null)
      return;
    followerHighlighted(this._followerInfo);
  }
}
