// Decompiled with JetBrains decompiler
// Type: FollowerInformationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Spine.Unity;
using System;
using System.Collections.Generic;
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
  [SerializeField]
  private Image _adorationLevel;
  public Image HungerLevel;
  public Image IllnessLevel;
  public Image TiredLevel;
  public List<FollowerThoughtObject> FollowerThoughts = new List<FollowerThoughtObject>();
  private string tmpstring;
  private string AgeString;
  public FollowerBrain followBrain;
  public GameObject ItemParent;
  public TMP_Text itemText;
  private string LevelIcon = "\uF102";
  private Tween punchTween;

  public InventoryItem.ITEM_TYPE ItemCostType { get; private set; }

  public int Cost { get; private set; }

  protected override void ConfigureImpl()
  {
    this.ItemParent.SetActive(false);
    if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
    {
      this._button.onClick.AddListener((UnityAction) (() =>
      {
        if (this.Cost == 0 || Inventory.GetItemQuantity(this.ItemCostType) >= this.Cost * -1)
        {
          Action<FollowerInfo> followerSelected = this.OnFollowerSelected;
          if (followerSelected == null)
            return;
          followerSelected(this._followerInfo);
        }
        else
          this.InvalidShake();
      }));
      this._button.OnConfirmDenied += new System.Action(this.InvalidShake);
    }
    this.FollowerName.text = this._followerInfo.GetNameFormatted();
    this.tmpstring = this._followerInfo.MemberDuration != 0 ? string.Format(LocalizationManager.GetTranslation("UI/FollowerInfo/MemberDuration"), (object) this._followerInfo.MemberDuration) : LocalizationManager.GetTranslation("UI/FollowerInfo/MemberStatusNew");
    this.AgeString = string.Format(LocalizationManager.GetTranslation("UI/FollowerInfo/Age"), (object) this._followerInfo.Age);
    this.FollowerRole.text = $"{this.tmpstring} | {this.AgeString}";
    if ((UnityEngine.Object) this.FollowerSpine != (UnityEngine.Object) null)
      this.FollowerSpine.ConfigureFollower(this._followerInfo);
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Info.ID == this._followerInfo.ID)
        this.followBrain = allBrain;
    }
    if (this.followBrain == null)
    {
      this._adorationLevel.transform.parent.parent.gameObject.SetActive(false);
      this.HungerLevel.transform.parent.parent.gameObject.SetActive(false);
      this.IllnessLevel.transform.parent.parent.gameObject.SetActive(false);
      this.TiredLevel.transform.parent.parent.gameObject.SetActive(false);
    }
    else
    {
      this._adorationLevel.fillAmount = this.followBrain.Stats.Adoration / this.followBrain.Stats.MAX_ADORATION;
      this.HungerLevel.fillAmount = (float) (((double) this.followBrain.Stats.Satiation + (75.0 - (double) this.followBrain.Stats.Starvation)) / 175.0);
      this.HungerLevel.color = this.ReturnColorBasedOnValueHunger(this.HungerLevel.fillAmount);
      this.IllnessLevel.fillAmount = (float) (((double) this._followerInfo.Illness / 100.0 - 1.0) * -1.0);
      this.IllnessLevel.color = this.ReturnColorBasedOnValue(this.IllnessLevel.fillAmount);
      this.TiredLevel.fillAmount = this._followerInfo.Rest / 100f;
      this.TiredLevel.color = this.ReturnColorBasedOnValue(this.TiredLevel.fillAmount);
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
  }

  public void ShowRewardItem(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    this.itemText.text = $"{FontImageNames.GetIconByType(itemType)} x{(object) quantity}";
    this.ItemParent.SetActive(true);
  }

  public void ShowCostItem(InventoryItem.ITEM_TYPE itemType, int cost, bool ForceRed = true)
  {
    this.itemText.text = $"{FontImageNames.GetIconByType(itemType)} {(object) cost}";
    this.ItemParent.SetActive(true);
    if (ForceRed)
      this.itemText.color = Color.red;
    this.ItemCostType = itemType;
    this.Cost = cost * -1;
  }

  public void ShowFaithGain(int gain, float MAX_ADORATION)
  {
    this.ItemParent.SetActive(true);
    this.itemText.text = $"<font=\"Font Awesome 6 Pro-Solid-900 SDF\"><color=red> {this.LevelIcon} </color></font>+{(object) (float) ((double) gain / (double) MAX_ADORATION * 100.0)}%";
    this.itemText.color = StaticColors.OffWhiteColor;
  }

  public void ShowDevotionGain(int gain)
  {
    this.ItemParent.SetActive(true);
    this.itemText.text = $"<font=\"Font Awesome 6 Pro-Solid-900 SDF\"><color=red> <sprite name=\"icon_Faith\"> </color></font>+{(object) gain}%";
    this.itemText.color = StaticColors.OffWhiteColor;
  }

  public void ShowMarried()
  {
    this.itemText.text = "<sprite name=\"icon_Married\">";
    this.ItemParent.SetActive(true);
    this._button.Confirmable = false;
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

  private Color ReturnColorBasedOnValue(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.25)
      return StaticColors.RedColor;
    return (double) f >= 0.25 && (double) f < 0.5 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  private Color ReturnColorBasedOnValueHunger(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.5)
      return StaticColors.RedColor;
    return (double) f >= 0.5 && (double) f < 0.7 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public void OnRecycled()
  {
    this._adorationLevel.transform.parent.parent.gameObject.SetActive(true);
    this.HungerLevel.transform.parent.parent.gameObject.SetActive(true);
    this.IllnessLevel.transform.parent.parent.gameObject.SetActive(true);
    this.TiredLevel.transform.parent.parent.gameObject.SetActive(false);
    this.ItemParent.SetActive(false);
    this._followerInfo = (FollowerInfo) null;
    this._button.interactable = true;
    this._button.onClick.RemoveAllListeners();
    this._button.OnConfirmDenied = (System.Action) null;
    this._button.Confirmable = true;
    this.OnFollowerSelected = (Action<FollowerInfo>) null;
    this.itemText.text = string.Empty;
  }
}
