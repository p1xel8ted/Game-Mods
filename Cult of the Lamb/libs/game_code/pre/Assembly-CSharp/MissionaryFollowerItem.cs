// Decompiled with JetBrains decompiler
// Type: MissionaryFollowerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private SkeletonGraphic _followerSpine;
  [SerializeField]
  private TextMeshProUGUI _followerName;
  [SerializeField]
  private TextMeshProUGUI _missionDescription;
  [SerializeField]
  private Image _progressBar;
  [SerializeField]
  private TextMeshProUGUI _dayText;
  [SerializeField]
  private TextMeshProUGUI _typeIcon;
  [SerializeField]
  private TextMeshProUGUI _amountText;
  [SerializeField]
  private Image _successRate;
  [SerializeField]
  private TextMeshProUGUI _successText;

  protected override void ConfigureImpl()
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
