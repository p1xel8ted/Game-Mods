// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.LoreInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class LoreInfoCard : UIInfoCardBase<int>
{
  [Header("Lore Info")]
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  public string TargetDescriptionText;

  public override void Configure(int config)
  {
    this._itemHeader.text = LocalizationManager.GetTranslation($"Lore/Lore_{config}/Name");
    this._itemLore.text = LocalizationManager.GetTranslation($"Lore/Lore_{config}/Lore");
    this.TargetDescriptionText = LocalizationManager.GetTranslation($"Lore/Lore_{config}/Description");
    this._itemDescription.text = this.TargetDescriptionText;
    if (LoreSystem.IsDLCLore(config))
      this._itemDescription.color = StaticColors.DLC1Blue;
    else
      this._itemDescription.color = StaticColors.RedColor;
  }
}
