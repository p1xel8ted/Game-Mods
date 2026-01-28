// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.RitualInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI.Assets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class RitualInfoCard : UIInfoCardBase<UpgradeSystem.Type>
{
  [Header("Copy")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public TextMeshProUGUI _faithText;
  [SerializeField]
  public GameObject _faithContainer;
  [SerializeField]
  public BarController _faithBar;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public RitualIconMapping _iconMapping;
  [SerializeField]
  public TextMeshProUGUI _sinText;
  [SerializeField]
  public GameObject _sinContainer;
  [SerializeField]
  public TextMeshProUGUI _sinTypeText;
  [SerializeField]
  public Image _sinBar;
  [Header("Limited Time")]
  [SerializeField]
  public GameObject _limitedTimeIcon;
  [SerializeField]
  public GameObject _limitedTimeContainer;
  [SerializeField]
  public TMP_Text _limitedTimeText;
  [Header("Costs")]
  [SerializeField]
  public GameObject _costContainer;
  [SerializeField]
  public TextMeshProUGUI[] _costTexts;
  [Header("Winter")]
  [SerializeField]
  public TextMeshProUGUI _warmthText;
  [SerializeField]
  public GameObject _warmthContainer;
  [SerializeField]
  public BarController _warmthBar;

  public override void Configure(UpgradeSystem.Type config)
  {
    this._headerText.text = UpgradeSystem.GetLocalizedName(config);
    this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(config);
    this._icon.sprite = this._iconMapping.GetImage(config);
    this._limitedTimeIcon.SetActive(UpgradeSystem.IsSpecialRitual(config));
    this._limitedTimeContainer.SetActive(UpgradeSystem.IsSpecialRitual(config));
    this._limitedTimeText.text = LocalizationManager.GetTranslation("UI/RitualMenu/LimitedTime");
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(config);
    for (int index = 0; index < this._costTexts.Length; ++index)
    {
      if (index >= cost.Count)
      {
        this._costTexts[index].gameObject.SetActive(false);
      }
      else
      {
        this._costTexts[index].text = cost[index].ToStringShowQuantity();
        this._costTexts[index].gameObject.SetActive(true);
      }
    }
    if ((UnityEngine.Object) this._costContainer != (UnityEngine.Object) null && cost.Count == 0)
      this._costContainer.SetActive(false);
    if ((UnityEngine.Object) this._faithContainer != (UnityEngine.Object) null)
    {
      float ritualFaithChange = UpgradeSystem.GetRitualFaithChange(config);
      FollowerTrait.TraitType ritualTrait = UpgradeSystem.GetRitualTrait(config);
      this._faithContainer.SetActive((double) ritualFaithChange != 0.0);
      Color colour = (double) ritualFaithChange > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
      this._faithText.text = ((double) ritualFaithChange > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + LocalizeIntegration.ReverseText(Mathf.Abs(ritualFaithChange).ToString()).Colour(colour) + (ritualTrait == FollowerTrait.TraitType.None || !DataManager.Instance.CultTraits.Contains(ritualTrait) ? "" : $" ({FollowerTrait.GetLocalizedTitle(ritualTrait)})");
      if ((double) ritualFaithChange < 0.0)
        this._faithBar.SetBarSizeForInfo(CultFaithManager.CultFaithNormalised, (float) (((double) CultFaithManager.CurrentFaith + (double) ritualFaithChange) / 85.0), FollowerBrainStats.BrainWashed);
      else
        this._faithBar.SetBarSizeForInfo((float) (((double) CultFaithManager.CurrentFaith + (double) ritualFaithChange) / 85.0), CultFaithManager.CultFaithNormalised, FollowerBrainStats.BrainWashed);
    }
    float ritualWarmthChange = UpgradeSystem.GetRitualWarmthChange(config);
    if ((double) ritualWarmthChange != 0.0 && (UnityEngine.Object) this._warmthContainer != (UnityEngine.Object) null && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      int ritualTrait = (int) UpgradeSystem.GetRitualTrait(config);
      this._warmthContainer.SetActive((double) ritualWarmthChange != 0.0);
      Color colour = (double) ritualWarmthChange > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
      this._warmthText.text = (((double) ritualWarmthChange > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + Mathf.Abs(ritualWarmthChange).ToString()).Colour(colour);
      if ((double) ritualWarmthChange < 0.0)
        this._warmthBar.SetBarSizeForInfo(WarmthBar.WarmthNormalized, WarmthBar.WarmthNormalized + ritualWarmthChange / WarmthBar.MAX_WARMTH, FollowerBrainStats.LockedWarmth || config == UpgradeSystem.Type.Ritual_RanchHarvest);
      else
        this._warmthBar.SetBarSizeForInfo(WarmthBar.WarmthNormalized + ritualWarmthChange / WarmthBar.MAX_WARMTH, WarmthBar.WarmthNormalized, FollowerBrainStats.LockedWarmth || config == UpgradeSystem.Type.Ritual_RanchHarvest);
    }
    else if ((UnityEngine.Object) this._warmthContainer != (UnityEngine.Object) null)
      this._warmthContainer.gameObject.SetActive(false);
    if ((UnityEngine.Object) this._sinContainer != (UnityEngine.Object) null)
    {
      this._sinContainer.gameObject.SetActive(false);
      if (config == UpgradeSystem.Type.Ritual_Nudism || config == UpgradeSystem.Type.Ritual_Purge || config == UpgradeSystem.Type.Ritual_AtoneSin)
      {
        this._sinContainer.gameObject.SetActive(true);
        if ((bool) (UnityEngine.Object) this._faithContainer)
          this._faithContainer.gameObject.SetActive(false);
        this._sinTypeText.text = "\uF007";
        float faithDelta = (float) UpgradeSystem.GetRitualSinChange(config);
        if (!DataManager.Instance.HasPerformedPleasureRitual)
          faithDelta = 65f;
        Color greenColor = StaticColors.GreenColor;
        this._sinText.text = (((double) faithDelta > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + Mathf.Abs(faithDelta).ToString()).Colour(greenColor);
        GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => this._sinBar.fillAmount = faithDelta / 65f));
      }
    }
    switch (config)
    {
      case UpgradeSystem.Type.Ritual_BecomeDisciple:
        bool flag1 = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.XPLevel >= 10 && !allBrain.Info.IsDisciple)
            flag1 = true;
        }
        if (flag1)
          break;
        this._limitedTimeContainer.gameObject.SetActive(true);
        this._limitedTimeIcon.gameObject.SetActive(true);
        this._limitedTimeText.text = "<color=#FFD201>" + string.Format(LocalizationManager.GetTranslation("UI/NoFollowersAvailableToBecomeDisciple"), (object) 10.ToNumeral());
        break;
      case UpgradeSystem.Type.Ritual_Snowman:
        if (StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.SNOWMAN).Count > 0)
          break;
        this._limitedTimeContainer.gameObject.SetActive(true);
        this._limitedTimeIcon.gameObject.SetActive(true);
        this._limitedTimeText.text = "<color=#FFD201>" + string.Format(LocalizationManager.GetTranslation("UI/NoSnowmenAvailable"), (object) 10.ToNumeral());
        break;
      case UpgradeSystem.Type.Ritual_Divorce:
        bool flag2 = false;
        foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        {
          if (!FollowerManager.FollowerLocked(allBrain.Info.ID) && allBrain.Info.MarriedToLeader)
            flag2 = true;
        }
        if (flag2)
          break;
        this._limitedTimeContainer.gameObject.SetActive(true);
        this._limitedTimeIcon.gameObject.SetActive(true);
        this._limitedTimeText.text = "<color=#FFD201>" + string.Format(LocalizationManager.GetTranslation("UI/NoMarriedFollowers"), (object) 10.ToNumeral());
        break;
      default:
        if (config != UpgradeSystem.Type.Ritual_RanchHarvest && config != UpgradeSystem.Type.Ritual_RanchMeat || StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH).Count + StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.RANCH_2).Count > 0)
          break;
        this._limitedTimeContainer.gameObject.SetActive(true);
        this._limitedTimeIcon.gameObject.SetActive(true);
        this._limitedTimeText.text = "<color=#FFD201>" + string.Format(LocalizationManager.GetTranslation("UI/NoRanchAvailable"), (object) 10.ToNumeral());
        break;
    }
  }

  public void RemoveCost() => this._costContainer.gameObject.SetActive(false);

  public void ShakeLimitedTimeText()
  {
    this._limitedTimeContainer.transform.DOKill();
    this._limitedTimeContainer.transform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }
}
