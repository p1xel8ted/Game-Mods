// Decompiled with JetBrains decompiler
// Type: src.UI.Prompts.UIRelicPickupPromptController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Prompts;

public class UIRelicPickupPromptController : UIPromptBase
{
  [Header("Content")]
  [SerializeField]
  public TextMeshProUGUI _title;
  [SerializeField]
  public TextMeshProUGUI _lore;
  [SerializeField]
  public TextMeshProUGUI _description;
  [Header("Stats")]
  [SerializeField]
  public TextMeshProUGUI _fragile;
  [SerializeField]
  public GameObject _fragileIcon;
  public RelicData _relicData;
  public RelicData _compareTo;

  public override bool _addToActiveMenus => false;

  public void Show(
    RelicData relicData,
    RelicData compareTo,
    PlayerFarming playerFarming,
    bool instant = false)
  {
    this.Show(instant);
    this._relicData = relicData;
    this._compareTo = compareTo;
    this.playerFarming = playerFarming;
    this._fragile.gameObject.SetActive(this._relicData.InteractionType == RelicInteractionType.Fragile || TrinketManager.AreRelicsFragile(playerFarming));
  }

  public override void Localize()
  {
    this._title.text = RelicData.GetTitleLocalisation(this._relicData.RelicType);
    this._lore.text = RelicData.GetLoreLocalization(this._relicData.RelicType);
    this._description.text = RelicData.GetDescriptionLocalisation(this._relicData.RelicType);
    this._fragile.gameObject.SetActive(this._relicData.InteractionType == RelicInteractionType.Fragile || TrinketManager.AreRelicsFragile(this.playerFarming));
    this._fragileIcon.gameObject.SetActive(this._relicData.InteractionType == RelicInteractionType.Fragile || TrinketManager.AreRelicsFragile(this.playerFarming));
  }
}
