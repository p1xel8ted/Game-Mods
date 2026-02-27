// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeInfoCard : UIInfoCardBase<UpgradeSystem.Type>
{
  [Header("Copy")]
  [SerializeField]
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [Header("Costs")]
  [SerializeField]
  private GameObject _costHeader;
  [SerializeField]
  private TextMeshProUGUI _costText;
  [Header("Graphics")]
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private SpriteAtlas _atlas;

  public override void Configure(UpgradeSystem.Type config)
  {
    this._icon.sprite = this._atlas.GetSprite(config.ToString());
    this._headerText.text = UpgradeSystem.GetLocalizedName(config);
    this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(config);
    if (UpgradeSystem.GetUnlocked(config))
    {
      this._costHeader.SetActive(false);
      this._costText.text = "";
    }
    else
      this._costText.text = StructuresData.ItemCost.GetCostStringWithQuantity(UpgradeSystem.GetCost(config));
  }
}
