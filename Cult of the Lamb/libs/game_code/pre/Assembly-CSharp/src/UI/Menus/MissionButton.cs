// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.MissionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus;

public class MissionButton : BaseMonoBehaviour
{
  public Action<InventoryItem.ITEM_TYPE> OnMissionSelected;
  [SerializeField]
  private InventoryItem.ITEM_TYPE _type;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private TextMeshProUGUI _chanceText;
  [SerializeField]
  private TextMeshProUGUI _icon;
  [SerializeField]
  private TextMeshProUGUI _titleText;
  [SerializeField]
  private TextMeshProUGUI _amountText;
  [SerializeField]
  private TextMeshProUGUI _durationText;
  [SerializeField]
  private Image _chanceWheel;

  public InventoryItem.ITEM_TYPE Type => this._type;

  public MMButton Button => this._button;

  public void Start() => this._button.onClick.AddListener(new UnityAction(this.OnMissionClicked));

  public void Configure(FollowerInfo followerInfo)
  {
    List<Structures_Missionary> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Missionary>();
    if (this._type == InventoryItem.ITEM_TYPE.SEEDS)
      this._type = (InventoryItem.ITEM_TYPE) MissionaryManager.GetSeedsReward(this._type).type;
    float chance = MissionaryManager.GetChance(this._type, followerInfo, structuresOfType.Count > 0 ? structuresOfType[0].Data.Type : StructureBrain.TYPES.MISSIONARY);
    this._chanceWheel.fillAmount = chance;
    this._chanceText.text = $"{(int) ((double) chance * 100.0)}%";
    this._chanceText.color = StaticColors.ColorForThreshold(chance);
    this._icon.text = FontImageNames.GetIconByType(this._type);
    this._titleText.text = InventoryItem.LocalizedName(this._type);
    this._amountText.text = string.Join(" ", MissionaryManager.GetRewardRange(this._type).ToString(), $"({Inventory.GetItemQuantity(this._type)})".Colour(StaticColors.GreyColor));
    this._durationText.text = string.Format(ScriptLocalization.UI_Generic.Days, (object) MissionaryManager.GetDurationDeterministic(followerInfo, this._type));
  }

  private void OnMissionClicked()
  {
    Action<InventoryItem.ITEM_TYPE> onMissionSelected = this.OnMissionSelected;
    if (onMissionSelected == null)
      return;
    onMissionSelected(this._type);
  }
}
