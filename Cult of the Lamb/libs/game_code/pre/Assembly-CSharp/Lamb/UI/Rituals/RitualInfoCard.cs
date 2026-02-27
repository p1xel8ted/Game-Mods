// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.RitualInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private TextMeshProUGUI _faithText;
  [SerializeField]
  private GameObject _faithContainer;
  [SerializeField]
  private BarController _faithBar;
  [SerializeField]
  private Image _icon;
  [Header("Limited Time")]
  [SerializeField]
  private GameObject _limitedTimeIcon;
  [SerializeField]
  private GameObject _limitedTimeText;
  [Header("Costs")]
  [SerializeField]
  private TextMeshProUGUI[] _costTexts;

  public override void Configure(UpgradeSystem.Type config)
  {
    this._headerText.text = UpgradeSystem.GetLocalizedName(config);
    this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(config);
    this._icon.sprite = DoctrineUpgradeSystem.GetIconForRitual(config);
    this._limitedTimeIcon.SetActive(UpgradeSystem.IsSpecialRitual(config));
    this._limitedTimeText.SetActive(UpgradeSystem.IsSpecialRitual(config));
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
    if (!((Object) this._faithContainer != (Object) null))
      return;
    float ritualFaithChange = UpgradeSystem.GetRitualFaithChange(config);
    FollowerTrait.TraitType ritualTrait = UpgradeSystem.GetRitualTrait(config);
    this._faithContainer.SetActive((double) ritualFaithChange != 0.0);
    Color colour = (double) ritualFaithChange > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
    this._faithText.text = (((double) ritualFaithChange > 0.0 ? (object) "<sprite name=\"icon_FaithUp\">" : (object) "<sprite name=\"icon_FaithDown\">").ToString() + (object) Mathf.Abs(ritualFaithChange)).Colour(colour) + (ritualTrait == FollowerTrait.TraitType.None || !DataManager.Instance.CultTraits.Contains(ritualTrait) ? "" : $" ({FollowerTrait.GetLocalizedTitle(ritualTrait)})");
    if ((double) ritualFaithChange < 0.0)
      this._faithBar.SetBarSizeForInfo(CultFaithManager.CultFaithNormalised, (float) (((double) CultFaithManager.CurrentFaith + (double) ritualFaithChange) / 85.0), FollowerBrainStats.BrainWashed);
    else
      this._faithBar.SetBarSizeForInfo((float) (((double) CultFaithManager.CurrentFaith + (double) ritualFaithChange) / 85.0), CultFaithManager.CultFaithNormalised, FollowerBrainStats.BrainWashed);
  }
}
