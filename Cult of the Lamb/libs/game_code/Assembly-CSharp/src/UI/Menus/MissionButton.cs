// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.MissionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public InventoryItem.ITEM_TYPE _type;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public TextMeshProUGUI _chanceText;
  [SerializeField]
  public TextMeshProUGUI _icon;
  [SerializeField]
  public TextMeshProUGUI _titleText;
  [SerializeField]
  public TextMeshProUGUI _amountText;
  [SerializeField]
  public TextMeshProUGUI _durationText;
  [SerializeField]
  public Image _chanceWheel;

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

  public void OnMissionClicked()
  {
    Action<InventoryItem.ITEM_TYPE> onMissionSelected = this.OnMissionSelected;
    if (onMissionSelected == null)
      return;
    onMissionSelected(this._type);
  }
}
