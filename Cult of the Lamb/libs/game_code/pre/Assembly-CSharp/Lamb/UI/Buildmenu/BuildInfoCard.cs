// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private TextMeshProUGUI _requirementsText;
  [SerializeField]
  private GameObject _singleBuildContainer;
  [SerializeField]
  private TextMeshProUGUI _buildStatusText;
  [SerializeField]
  private TextMeshProUGUI _buildCountBadge;
  [Header("Costs")]
  [SerializeField]
  private GameObject _costHeader;
  [SerializeField]
  private GameObject _costContainer;
  [SerializeField]
  private TextMeshProUGUI[] _costTexts;
  [Header("Graphics")]
  [SerializeField]
  private Image _icon;

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
  }
}
