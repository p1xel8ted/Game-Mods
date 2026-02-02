// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.BuildMenu;

public class BuildInfoCard : UIInfoCardBase<StructureBrain.TYPES>
{
  [Header("Copy")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public TextMeshProUGUI _requirementsText;
  [SerializeField]
  public GameObject _singleBuildContainer;
  [SerializeField]
  public TextMeshProUGUI _buildStatusText;
  [SerializeField]
  public TextMeshProUGUI _buildCountBadge;
  [Header("Costs")]
  [SerializeField]
  public GameObject _costHeader;
  [SerializeField]
  public GameObject _costContainer;
  [SerializeField]
  public TextMeshProUGUI[] _costTexts;
  [Header("Graphics")]
  [SerializeField]
  public Image _icon;
  [Header("Winter")]
  [SerializeField]
  public TextMeshProUGUI _warmthText;
  [SerializeField]
  public GameObject _warmthContainer;
  [SerializeField]
  public BarController _warmthBar;

  public override void Configure(StructureBrain.TYPES structureType)
  {
    this._icon.sprite = TypeAndPlacementObjects.GetByType(structureType).IconImage;
    this._headerText.text = StructuresData.LocalizedName(structureType);
    this._descriptionText.text = StructuresData.LocalizedDescription(structureType);
    this._requirementsText.gameObject.SetActive(false);
    this._buildStatusText.gameObject.SetActive(false);
    this._costHeader.SetActive(true);
    this._costContainer.SetActive(true);
    List<StructuresData.ItemCost> cost = StructuresData.GetCost(structureType);
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
      this._costTexts[index].isRightToLeftText = LocalizeIntegration.IsArabic();
    }
    this._costHeader.SetActive(cost.Count > 0);
    this._costContainer.SetActive(cost.Count > 0);
    if (StructuresData.IsUpgradeStructure(structureType))
    {
      StructureBrain.TYPES upgradePrerequisite = StructuresData.GetUpgradePrerequisite(structureType);
      if (StructureManager.GetAllStructuresOfType(upgradePrerequisite).Count <= 0)
      {
        this._requirementsText.text = $"{ScriptLocalization.Interactions.Requires} <color=#FFD201>{StructuresData.LocalizedName(upgradePrerequisite)}";
        this._requirementsText.gameObject.SetActive(true);
      }
    }
    if (StructuresData.RequiresTempleToBuild(structureType) && !StructuresData.HasTemple())
    {
      this._requirementsText.text = $"{ScriptLocalization.Interactions.Requires} <color=#FFD201>{StructuresData.LocalizedName(StructureBrain.TYPES.TEMPLE)}";
      this._requirementsText.gameObject.SetActive(true);
    }
    if (StructuresData.RequiresRanchToBuild(structureType) && StructureManager.GetAllStructuresOfType<Structures_Ranch>().Count <= 0)
    {
      this._requirementsText.text = $"{ScriptLocalization.Interactions.Requires} <color=#FFD201>{StructuresData.LocalizedName(StructureBrain.TYPES.RANCH)}";
      this._requirementsText.gameObject.SetActive(true);
    }
    bool buildOnlyOne = StructuresData.GetBuildOnlyOne(structureType);
    this._singleBuildContainer.gameObject.SetActive(buildOnlyOne);
    if (buildOnlyOne)
    {
      if (StructureManager.IsBuilding(structureType))
      {
        this._buildStatusText.gameObject.SetActive(true);
        this._buildStatusText.text = ScriptLocalization.UI_Settings_Controls_Header.Building.Colour(StaticColors.YellowColorHex);
        this._costHeader.SetActive(false);
        this._costContainer.SetActive(false);
      }
      else if (StructureManager.IsBuilt(structureType) || StructureManager.IsAnyUpgradeBuiltOrBuilding(structureType))
      {
        this._buildStatusText.gameObject.SetActive(true);
        this._buildStatusText.text = ScriptLocalization.UI_BuildingMenu.AlreadyBuilt.Colour(StaticColors.YellowColorHex);
        this._costHeader.SetActive(false);
        this._costContainer.SetActive(false);
      }
      this._buildCountBadge.text = "1";
    }
    else
      this._buildCountBadge.text = "<sprite name=\"icon_Infinity\">";
    float buildingWarmth = (float) StructureManager.GetBuildingWarmth(structureType);
    if ((double) buildingWarmth != 0.0 && (Object) this._warmthContainer != (Object) null && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      this._warmthContainer.SetActive((double) buildingWarmth != 0.0);
      Color colour = (double) buildingWarmth > 0.0 ? StaticColors.GreenColor : StaticColors.RedColor;
      this._warmthText.text = (((double) buildingWarmth > 0.0 ? "<sprite name=\"icon_FaithUp\">" : "<sprite name=\"icon_FaithDown\">") + Mathf.Abs(buildingWarmth).ToString()).Colour(colour);
      if ((double) buildingWarmth < 0.0)
        this._warmthBar.SetBarSizeForInfo(WarmthBar.WarmthNormalized, WarmthBar.WarmthNormalized + buildingWarmth / WarmthBar.MAX_WARMTH, FollowerBrainStats.LockedWarmth);
      else
        this._warmthBar.SetBarSizeForInfo(WarmthBar.WarmthNormalized + buildingWarmth / WarmthBar.MAX_WARMTH, WarmthBar.WarmthNormalized, FollowerBrainStats.LockedWarmth);
    }
    else
    {
      if (!((Object) this._warmthContainer != (Object) null))
        return;
      this._warmthContainer.gameObject.SetActive(false);
    }
  }
}
