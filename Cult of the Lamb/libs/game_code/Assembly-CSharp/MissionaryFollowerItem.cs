// Decompiled with JetBrains decompiler
// Type: MissionaryFollowerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MissionaryFollowerItem : FollowerSelectItem
{
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public TextMeshProUGUI _followerName;
  [SerializeField]
  public TextMeshProUGUI _missionDescription;
  [SerializeField]
  public Image _progressBar;
  [SerializeField]
  public TextMeshProUGUI _dayText;
  [SerializeField]
  public TextMeshProUGUI _typeIcon;
  [SerializeField]
  public TextMeshProUGUI _amountText;
  [SerializeField]
  public Image _successRate;
  [SerializeField]
  public TextMeshProUGUI _successText;

  public override void ConfigureImpl()
  {
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>();
    this._followerSpine.ConfigureFollower(this._followerInfo);
    this._followerName.text = this._followerInfo.GetNameFormatted();
    this._button.Confirmable = false;
    this._missionDescription.text = string.Format(ScriptLocalization.Objectives_Missionary.Description, (object) InventoryItem.LocalizedName((InventoryItem.ITEM_TYPE) this._followerInfo.MissionaryType).Colour(Color.yellow));
    this._progressBar.fillAmount = (TimeManager.TotalElapsedGameTime - this._followerInfo.MissionaryTimestamp) / this._followerInfo.MissionaryDuration;
    this._dayText.text = MissionaryManager.GetExpiryFormatted(this._followerInfo.MissionaryTimestamp + this._followerInfo.MissionaryDuration);
    this._typeIcon.text = FontImageNames.GetIconByType((InventoryItem.ITEM_TYPE) this._followerInfo.MissionaryType);
    this._amountText.text = MissionaryManager.GetRewardRange((InventoryItem.ITEM_TYPE) this._followerInfo.MissionaryType).ToString();
    float chance = MissionaryManager.GetChance((InventoryItem.ITEM_TYPE) this._followerInfo.MissionaryType, this._followerInfo, structuresOfType.Count > 0 ? structuresOfType[0].Data.Type : StructureBrain.TYPES.MISSIONARY);
    this._successRate.fillAmount = chance;
    this._successText.text = $"{(int) ((double) chance * 100.0)}%";
  }
}
